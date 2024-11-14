using GithubAnalyzeAPI.Data;
using GithubAnalyzeAPI.Dtos;
using GithubAnalyzeAPI.Enums;
using GithubAnalyzeAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GithubAnalyzeAPI.Services;

public interface ITrafficService
{
    Task<ResponseDto<bool>> SyncDataAsync(SyncType type);
    Task<ResponseDto<HistorySyncDto>> GetLastSyncAsync();
    Task<ResponseDto<List<HistorySyncDto>>> GetHistorySyncAsync();
    Task<ResponseDto<int>> CalTimeSync();
    Task<ResponseDto<SummaryDataDto>> GetSummaryDataAsync(string startDate = "", string endDate = "", long repoId = 0);
}

public class TrafficService(
    IApplicationDbContext dbContext,
    IGithubService github,
    IConfiguration configuration) : ITrafficService
{
    const int DELAY = 100;

    public async Task<ResponseDto<int>> CalTimeSync()
    {
        var username = configuration["GithubConfig:Username"] ?? string.Empty;
        var token = configuration["GithubConfig:Token"] ?? string.Empty;
        var authorizationHeader = new Dictionary<string, string>()
            {
                { "Authorization", $"token {token}" },
            };

        var repositories = await github.GetRepositoriesAsync(username, "1", authorizationHeader);

        return new ResponseDto<int>() { Success = true, Data = repositories.Count * DELAY };
    }

    public async Task<ResponseDto<List<HistorySyncDto>>> GetHistorySyncAsync()
    {
        var result = await dbContext.HistorySyncs.OrderByDescending(s => s.SyncAt).ToListAsync();
        var data = result.Adapt<List<HistorySyncDto>>();

        foreach (var item in data)
        {
            item.SyncAtDisplay = item.SyncAt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        return new ResponseDto<List<HistorySyncDto>>() 
        {
            Data = data,
            Success = true,
            Message = "Success"
        };
    }

    public async Task<ResponseDto<HistorySyncDto>> GetLastSyncAsync()
    {
        var result = await dbContext.HistorySyncs.OrderByDescending(o => o.SyncAt).FirstOrDefaultAsync();
        var data = result.Adapt<HistorySyncDto>();

        if (result != null)
        {
            data.SyncAtDisplay = result.SyncAt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        return new ResponseDto<HistorySyncDto>()
        {
            Data = data,
            Success = true,
            Message = "Success"
        };
    }

    public async Task<ResponseDto<SummaryDataDto>> GetSummaryDataAsync(string startDate = "", string endDate = "", long repoId = 0)
    {
        DateTime currentDate = DateTime.Now;

        if (string.IsNullOrEmpty(startDate))
        {
            startDate = new DateTime(currentDate.Year, currentDate.Month, 1).ToString("yyyy-MM-dd");
        }

        if (string.IsNullOrEmpty(endDate))
        {
            endDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)).ToString("yyyy-MM-dd");
        }

        var dateListAsStrings = new List<string>();
        for (DateTime date = DateTime.Parse(startDate); date <= DateTime.Parse(endDate); date = date.AddDays(1))
        {
            dateListAsStrings.Add(date.ToString("yyyy-MM-dd"));
        }

        bool d1 = DateOnly.TryParse(startDate, out DateOnly start);
        bool d2 = DateOnly.TryParse(endDate, out DateOnly end);

        if (!(await dbContext.Repositories.AnyAsync()))
        {
            await SyncDataAsync(SyncType.Manual);
        }

        var repos = await dbContext.Repositories.ToListAsync();
        var maxViews = await dbContext.Views.OrderByDescending(s => s.Count).FirstOrDefaultAsync();
        var repoFilter = repos.First();

        if (maxViews != null)
        {
            repoFilter = repos.First(s => s.RepoId == maxViews.RepoId);
        }

        if (repoId > 0)
        {
            repoFilter = repos.First(s => s.RepoId == repoId);
        }

        var cloneQuery = from repo in repos
                         join clone in dbContext.Clones on repo.RepoId equals clone.RepoId into repoClones
                         from clone in repoClones.DefaultIfEmpty()
                         select new CloneDto
                         {
                             RepoId = repo.RepoId,
                             RepoName = repo.Name,
                             Timestamp = clone == null ? string.Empty : clone.Timestamp,
                             Count = clone == null ? 0 : clone.Count,
                             Clones = clone == null ? 0 : clone.Count,
                             Uniques = clone == null ? 0 : clone.Uniques
                         };

        var viewQuery = from repo in repos
                        join view in dbContext.Views on repo.RepoId equals view.RepoId into repoViews
                        from view in repoViews.DefaultIfEmpty()
                        select new ViewDto
                        {
                            RepoId = repo.RepoId,
                            RepoName = repo.Name,
                            Timestamp = view == null ? string.Empty : view.Timestamp,
                            Count = view == null ? 0 : view.Count,
                            Views = view == null ? 0 : view.Count,
                            Uniques = view == null ? 0 : view.Uniques
                        };

        var cloneData = cloneQuery.ToList();
        var viewData = viewQuery.ToList();
        var cloneDataFilter = cloneData.Where(s => s.RepoId == repoFilter.RepoId).ToList();
        var viewDataFilter = viewData.Where(s => s.RepoId == repoFilter.RepoId).ToList();

        var cloneFilterJoinedData = from date in dateListAsStrings
                         join data in cloneDataFilter
                             on date equals data.Timestamp into dateDataGroup
                         from data in dateDataGroup.DefaultIfEmpty()
                         where data?.RepoId == repoFilter.RepoId || data == null
                         orderby date
                         select new CloneDto
                         {
                             RepoId = repoFilter.RepoId,
                             RepoName = repoFilter.Name,
                             Timestamp = date,
                             Count = data?.Count ?? 0,
                             Clones = data?.Clones ?? 0,
                             Uniques = data?.Uniques ?? 0
                         };

        var viewFilterJoinedData = from date in dateListAsStrings
                                    join data in viewDataFilter
                                        on date equals data.Timestamp into dateDataGroup
                                    from data in dateDataGroup.DefaultIfEmpty()
                                    where data?.RepoId == repoFilter.RepoId || data == null
                                    orderby date
                                    select new ViewDto
                                    {
                                        RepoId = repoFilter.RepoId,
                                        RepoName = repoFilter.Name,
                                        Timestamp = date,
                                        Count = data?.Count ?? 0,
                                        Views = data?.Views ?? 0,
                                        Uniques = data?.Uniques ?? 0
                                    };

        var finalResult = new ResponseDto<SummaryDataDto>()
        {
            Data = new SummaryDataDto()
            {
                FilterData = new FilterDataDto()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    RepoId = repoFilter.RepoId,
                    RepoName = repoFilter.Name
                },
                FilterClones = new FilterCloneDto()
                {
                    Items = cloneFilterJoinedData.ToList(),
                    TotalClones = cloneData.Where(s => s.RepoId == repoFilter.RepoId).Sum(s => s.Count),
                    TotalUniqueCloners = cloneData.Where(s => s.RepoId == repoFilter.RepoId).Sum(s => s.Uniques),
                    Clones = cloneDataFilter.Sum(s => s.Count),
                    UniqueCloners = cloneDataFilter.Sum(s => s.Uniques)
                },
                Clone = new SummaryCloneDto()
                {
                    TotalClones = cloneData.Sum(s => s.Count),
                    UniqueCloners = cloneData.Sum(s => s.Uniques),
                    SummaryClones = MapSummaryClones(cloneData)
                },
                FilterViews = new FilterViewDto()
                {
                    Items = viewFilterJoinedData.ToList(),
                    TotalViews = viewData.Where(s => s.RepoId == repoFilter.RepoId).Sum(s => s.Count),
                    TotalUniqueVisitors = viewData.Where(s => s.RepoId == repoFilter.RepoId).Sum(s => s.Uniques),
                    Views = viewDataFilter.Sum(s => s.Count),
                    UniqueVisitors = viewDataFilter.Sum(s => s.Uniques)
                },
                View = new SummaryViewDto()
                {
                    TotalViews = viewData.Sum(s => s.Count),
                    UniqueVisitors = viewData.Sum(s => s.Uniques),
                    SummaryViews = MapSummaryViews(viewData)
                },
                Repository = new SummaryRepositoryDto()
                {
                    Repositories = repos.Adapt<List<RepositoryDto>>(),
                    Total = repos.Count
                }
            },
            Success = true,
            Message = "Success"
        };

        return finalResult;
    }

    public async Task<ResponseDto<bool>> SyncDataAsync(SyncType type)
    {
        var username = configuration["GithubConfig:Username"] ?? string.Empty;
        var token = configuration["GithubConfig:Token"] ?? string.Empty;
        var authorizationHeader = new Dictionary<string, string>()
            {
                { "Authorization", $"token {token}" },
            };
        var response = new ResponseDto<bool>() { Success = true, Data = true };
        var sync = new HistorySync() { SyncAt = DateTime.UtcNow, Type = type.ToString() };
        var repositories = await github.GetRepositoriesAsync(username, "1", authorizationHeader);

        var reposEntity = MapToRepositories(repositories);
        var viewsEntity = new List<View>();
        var clonesEntity = new List<Clone>();

        foreach (var repo in repositories)
        {
            var views = await github.GetViewsAysnc(username, repo.Name, authorizationHeader);

            await Task.Delay(DELAY);

            var clones = await github.GetClonesAysnc(username, repo.Name, authorizationHeader);

            foreach (var v in views.Views) v.RepoId = repo.Id;

            foreach (var c in clones.Clones) c.RepoId = repo.Id;

            viewsEntity.AddRange(views.Views.Adapt<List<View>>());
            clonesEntity.AddRange(clones.Clones.Adapt<List<Clone>>());
        }

        foreach (var item in viewsEntity) item.Timestamp = item.Timestamp.Replace("T00:00:00Z", string.Empty);
        foreach (var item in clonesEntity) item.Timestamp = item.Timestamp.Replace("T00:00:00Z", string.Empty);

        await RemoveUpdateExistsDataAsync(reposEntity, viewsEntity, clonesEntity);

        if (reposEntity.Count > 0) await dbContext.Repositories.AddRangeAsync(reposEntity);
        if (viewsEntity.Count > 0) await dbContext.Views.AddRangeAsync(viewsEntity);
        if (clonesEntity.Count > 0) await dbContext.Clones.AddRangeAsync(clonesEntity);

        await dbContext.HistorySyncs.AddAsync(sync);

        await dbContext.SaveChangesAsync(new CancellationToken());

        return response;
    }

    private List<ViewDto> MapSummaryViews(List<ViewDto> views)
    {
        return views.GroupBy(g => g.RepoId).Select(s => new ViewDto()
        {
            RepoId = s.Key,
            Count = s.Sum(s => s.Count),
            Views = s.Sum(s => s.Count),
            Uniques = s.Sum(s => s.Uniques),
            RepoName = s.FirstOrDefault()?.RepoName ?? string.Empty
        }).ToList();
    }

    private List<CloneDto> MapSummaryClones(List<CloneDto> clones)
    {
        return clones.GroupBy(g => g.RepoId).Select(s => new CloneDto()
        {
            RepoId = s.Key,
            Count = s.Sum(s => s.Count),
            Clones = s.Sum(s => s.Count),
            Uniques = s.Sum(s => s.Uniques),
            RepoName = s.FirstOrDefault()?.RepoName ?? string.Empty
        }).ToList();
    }

    private List<Repository> MapToRepositories(List<RepositoryDto> repositories)
    {
        return repositories.Select(s => new Repository()
        {
            FullName = s.FullName,
            RepoId = s.Id,
            Name = s.Name,
            HtmlUrl = s.HtmlUrl,
            Language = s.Language ?? string.Empty
        }).ToList();
    }

    private async Task RemoveUpdateExistsDataAsync(List<Repository> repos, List<View> views, List<Clone> clones)
    {
        var existsViews = await dbContext.Views.ToListAsync();
        var existsClones = await dbContext.Clones.ToListAsync();
        var existsRepos = await dbContext.Repositories.ToListAsync();

        var removeViews = existsViews.Where(s => views.Select(x => x.Timestamp).Contains(s.Timestamp)).ToList();
        var removeClones = existsClones.Where(s => clones.Select(x => x.Timestamp).Contains(s.Timestamp)).ToList();

        dbContext.Views.RemoveRange(removeViews);
        dbContext.Clones.RemoveRange(removeClones);
        dbContext.Repositories.RemoveRange(existsRepos);

        await dbContext.SaveChangesAsync(new CancellationToken());
    }
}

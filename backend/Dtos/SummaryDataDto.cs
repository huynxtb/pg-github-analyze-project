using System.Collections.Generic;

namespace GithubAnalyzeAPI.Dtos;

public class SummaryDataDto
{
    public FilterDataDto FilterData { get; set; } = new();
    public SummaryRepositoryDto Repository { get; set; } = new();
    public SummaryViewDto View { get; set; } = new();
    public SummaryCloneDto Clone { get; set; } = new();
    public FilterViewDto FilterViews { get; set; } = new();
    public FilterCloneDto FilterClones { get; set; } = new();
}

public class SummaryRepositoryDto
{
    public int Total { get; set; }
    public List<RepositoryDto> Repositories { get; set; } = new();
}

public class SummaryViewDto
{
    public int TotalViews { get; set; }
    public int UniqueVisitors { get; set; }
    public List<ViewDto> SummaryViews { get; set; } = new();
}

public class SummaryCloneDto
{
    public int TotalClones { get; set; }
    public int UniqueCloners { get; set; }
    public List<CloneDto> SummaryClones { get; set; } = new();
}

public class FilterViewDto
{
    public int TotalViews { get; set; }
    public int TotalUniqueVisitors { get; set; }
    public int Views { get; set; }
    public int UniqueVisitors { get; set; }
    public List<ViewDto> Items { get; set; } = new();
}

public class FilterCloneDto
{
    public int TotalClones { get; set; }
    public int TotalUniqueCloners { get; set; }
    public int Clones { get; set; }
    public int UniqueCloners { get; set; }
    public List<CloneDto> Items { get; set; } = new();
}

public class FilterDataDto
{
    public long RepoId { get; set; }
    public string RepoName { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}
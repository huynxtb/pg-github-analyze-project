using GithubAnalyzeAPI.Dtos;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GithubAnalyzeAPI.Services;

[Headers("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36")]
public interface IGithubService
{
    [Get("/users/{username}/repos?per_page=100&page={page}")]
    Task<List<RepositoryDto>> GetRepositoriesAsync(string username, string page, [HeaderCollection] IDictionary<string, string> headers);

    [Get("/repos/{username}/{repo}/traffic/views")]
    Task<ResponseViewDto> GetViewsAysnc(string username, string repo, [HeaderCollection] IDictionary<string, string> headers);

    [Get("/repos/{username}/{repo}/traffic/clones")]
    Task<ResponseCloneDto> GetClonesAysnc(string username, string repo, [HeaderCollection] IDictionary<string, string> headers);
}

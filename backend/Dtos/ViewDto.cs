using System.Collections.Generic;

namespace GithubAnalyzeAPI.Dtos;

public class ViewDto
{
    public long Id { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public int Count { get; set; }
    public int Views { get; set; }
    public int Uniques { get; set; }
    public long RepoId { get; set; }
    public string RepoName { get; set; } = string.Empty;
}
public class ResponseViewDto
{
    public int Count { get; set; }
    public int View { get; set; }
    public int Uniques { get; set; }
    public List<ViewDto> Views { get; set; } = new();
}
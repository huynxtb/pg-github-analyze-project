using System.Collections.Generic;

namespace GithubAnalyzeAPI.Dtos;

public class CloneDto
{
    public long Id { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public int Count { get; set; }
    public int Clones { get; set; }
    public int Uniques { get; set; }
    public long RepoId { get; set; }
    public string RepoName { get; set; } = string.Empty;
}

public class ResponseCloneDto
{
    public int Count { get; set; }
    public int Clone { get; set; }
    public int Uniques { get; set; }
    public List<CloneDto> Clones { get; set; } = new();
}
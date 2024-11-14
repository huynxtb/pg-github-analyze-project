namespace GithubAnalyzeAPI.Models;

public class Clone
{
    public long Id { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public int Count { get; set; }
    public int Uniques { get; set; }
    public long RepoId { get; set; }
}

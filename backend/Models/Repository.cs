using System.Text.Json.Serialization;

namespace GithubAnalyzeAPI.Models;

public class Repository
{
    public long Id { get; set; }
    public long RepoId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string HtmlUrl { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}

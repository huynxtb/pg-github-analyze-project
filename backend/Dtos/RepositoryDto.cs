using System.Text.Json.Serialization;

namespace GithubAnalyzeAPI.Dtos;

public class RepositoryDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;
    public long RepoId { get; set; }
}

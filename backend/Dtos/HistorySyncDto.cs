using System;

namespace GithubAnalyzeAPI.Dtos;

public class HistorySyncDto
{
    public long Id { get; set; }
    public string Summary { get; set; } = string.Empty;
    public DateTime SyncAt { get; set; }
    public string SyncAtDisplay { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

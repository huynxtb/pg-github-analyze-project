using System;

namespace GithubAnalyzeAPI.Models;

public class HistorySync
{
    public long Id { get; set; }
    public string Summary { get; set; } = string.Empty;
    public DateTime SyncAt { get; set; }
    public string Type { get; set; } = string.Empty;
}

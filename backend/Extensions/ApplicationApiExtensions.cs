using GithubAnalyzeAPI.Services;
using Microsoft.AspNetCore.Builder;

namespace GithubAnalyzeAPI.Extensions;

public static class ApplicationApiExtensions
{
    public static WebApplication MapApis(this WebApplication app)
    {
        #region Traffic
        app.MapGet("/api/traffic/sync/history/last-sync", async (ITrafficService service) =>
        {
            var result = await service.GetLastSyncAsync();
            return result;
        });

        app.MapGet("/api/traffic/sync/history", async (ITrafficService service) =>
        {
            var result = await service.GetHistorySyncAsync();
            return result;
        });

        app.MapGet("/api/traffic/data/summary", async (ITrafficService service,
            string startDate = "", string endDate = "", long repoId = 0) =>
        {
            var result = await service.GetSummaryDataAsync(startDate, endDate, repoId);
            return result;
        });

        app.MapPost("/api/traffic/sync", async (ITrafficService service) =>
        {
            var result = await service.SyncDataAsync(Enums.SyncType.Manual);

            return result;
        });

        app.MapPost("/api/traffic/sync/calculation-time", async (ITrafficService service) =>
        {
            var result = await service.CalTimeSync();

            return result;
        });
        #endregion

        return app;
    }
}

using Quartz;
using System.Threading.Tasks;

namespace GithubAnalyzeAPI.Services;

public class GithubJobSyncService(ITrafficService traffic) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await traffic.SyncDataAsync(Enums.SyncType.Job);
    }
}
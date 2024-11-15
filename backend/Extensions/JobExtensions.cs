using GithubAnalyzeAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace GithubAnalyzeAPI.Extensions;

public static class JobExtensions
{
    public static IServiceCollection AddJob(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("GithubJobSyncService");
            q.AddJob<GithubJobSyncService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("GithubJobSyncService-trigger")
                .WithCronSchedule("0 0 0/6 1/1 * ? *")
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = false);

        return services;
    }
}

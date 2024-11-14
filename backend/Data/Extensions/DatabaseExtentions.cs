using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GithubAnalyzeAPI.Data.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
    }
}
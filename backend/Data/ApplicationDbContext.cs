using GithubAnalyzeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GithubAnalyzeAPI.Data;
public interface IApplicationDbContext
{
    DbSet<Clone> Clones { get; }
    DbSet<View> Views { get; }
    DbSet<Repository> Repositories { get; }
    DbSet<HistorySync> HistorySyncs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Clone> Clones => Set<Clone>();

    public DbSet<View> Views => Set<View>();

    public DbSet<Repository> Repositories => Set<Repository>();

    public DbSet<HistorySync> HistorySyncs => Set<HistorySync>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GithubAnalyzeAPI.Models;

namespace GithubAnalyzeAPI.Data.Configurations;

public class HistorySyncConfiguration : IEntityTypeConfiguration<HistorySync>
{
    public void Configure(EntityTypeBuilder<HistorySync> builder)
    {
        builder.HasKey(o => o.Id);

        builder
          .Property(e => e.Id)
          .ValueGeneratedOnAdd();
    }
}
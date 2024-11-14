using GithubAnalyzeAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GithubAnalyzeAPI.Data.Configurations;

public class RepositoryConfiguration : IEntityTypeConfiguration<Repository>
{
    public void Configure(EntityTypeBuilder<Repository> builder)
    {
        builder.HasKey(o => o.Id);

        builder
          .Property(e => e.Id)
          .ValueGeneratedOnAdd();
    }
}
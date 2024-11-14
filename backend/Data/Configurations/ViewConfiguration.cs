using GithubAnalyzeAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GithubAnalyzeAPI.Data.Configurations;

public class ViewConfiguration : IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        builder.HasKey(o => o.Id);

        builder
          .Property(e => e.Id)
          .ValueGeneratedOnAdd();
    }
}
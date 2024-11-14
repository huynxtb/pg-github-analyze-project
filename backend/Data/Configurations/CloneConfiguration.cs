using GithubAnalyzeAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GithubAnalyzeAPI.Data.Configurations;

public class CloneConfiguration : IEntityTypeConfiguration<Clone>
{
    public void Configure(EntityTypeBuilder<Clone> builder)
    {
        builder.HasKey(o => o.Id);

        builder
           .Property(e => e.Id)
           .ValueGeneratedOnAdd();
    }
}
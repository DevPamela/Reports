using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Configs;
internal class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasIndex(e => e.Name)
               .IsUnique();
    }
            
}

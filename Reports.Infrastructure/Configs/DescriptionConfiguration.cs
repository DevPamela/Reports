using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Configs
{
    public class DescriptionConfiguration : IEntityTypeConfiguration<Description>
    {
        public void Configure(EntityTypeBuilder<Description> builder)
        {
            builder
            .HasKey(d => new { d.Goal, d.GoalId });

            builder
                .HasOne(a => a.Goal)
                .WithMany(s => s.Description)
                .HasForeignKey(a => a.GoalId);
        }
    }
}
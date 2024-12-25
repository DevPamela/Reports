using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs;
internal class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder
        .HasKey(g => g.Id);

        builder
            .HasOne(g => g.Teacher)
            .WithMany(t => t.Goals)
            .HasForeignKey(g => g.TeacherId)
            .IsRequired(false); // Optional relationship

        builder
            .HasMany(g => g.Students)
            .WithMany(s => s.Goals)
            .UsingEntity<Assessment>(
                j => j
                    .HasOne(a => a.Student)
                    .WithMany(s => s.Assessments)
                    .HasForeignKey(a => a.StudentId),
                j => j
                    .HasOne(a => a.Goal)
                    .WithMany(g => g.Assessments)
                    .HasForeignKey(a => a.GoalId)
            );
    }
            
}

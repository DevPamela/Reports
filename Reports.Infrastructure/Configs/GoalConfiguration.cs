using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;
using System.Security.AccessControl;

namespace Reports.Infrastructure.Configs;
internal class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder
        .HasKey(g => g.Id);

        builder.HasDiscriminator<string>("GoalType") 
           .HasValue<Goal>("Goal") 
           .HasValue<GirlsGoal>("GirlsGoal"); 
        builder
            .HasOne(g => g.Teacher)
            .WithMany(t => t.Goals)
            .HasForeignKey(g => g.TeacherId)
            .IsRequired(false); 

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

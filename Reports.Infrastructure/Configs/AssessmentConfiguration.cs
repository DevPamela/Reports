using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder
            .HasKey(a => new { a.StudentId, a.GoalId }); // Composite key

            builder
                .HasOne(a => a.Student)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.StudentId);

            builder
                .HasOne(a => a.Goal)
                .WithMany(g => g.Assessments)
                .HasForeignKey(a => a.GoalId);
        }
    }
}

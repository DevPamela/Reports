using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder
            .HasKey(t => t.Id);

            builder
                .HasMany(t => t.Class)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId);

            builder
                .HasMany(t => t.Students)
                .WithOne(s => s.Teacher)
                .HasForeignKey(s => s.TeacherId);

            builder
                .HasMany(t => t.Goals)
                .WithOne(g => g.Teacher)
                .HasForeignKey(g => g.TeacherId);
        }
    }
}

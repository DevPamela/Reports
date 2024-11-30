using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Enums;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs;
internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
        .HasKey(s => s.Id);

        builder
            .HasOne(s => s.Teacher)
            .WithMany(t => t.Students)
            .HasForeignKey(s => s.TeacherId);

        builder
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassRoomId);

        builder
            .Property(s => s.Gender)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)) // Convert to/from string
            .HasColumnType("ENUM('Male', 'Female')"); // MySQL ENUM type
    }

}

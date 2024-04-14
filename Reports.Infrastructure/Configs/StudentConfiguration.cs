using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Configs;
internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder) =>
        builder
            .HasMany(a => a.Goals)
            .WithMany(g => g.Students)
            .UsingEntity<Assessment>();
}

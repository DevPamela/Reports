using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Configs;
internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
           .HasMany(a => a.Goals)
           .WithMany(g => g.Students)
           .UsingEntity<Assessment>();


        builder.HasKey(a => a.Id);
        builder.HasMany(x => x.Reports)
            .WithOne(r => r.Student);
        builder.HasIndex(s => s.Name)
                   .IsUnique();

        builder.Property(s => s.Gender)
            .HasConversion<string>();
    }

}

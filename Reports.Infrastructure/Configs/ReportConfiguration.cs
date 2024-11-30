using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs;
internal class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder
            .HasKey(r => r.Id);

        builder
            .HasOne(r => r.Student)
            .WithMany(s => s.Reports)
            .HasForeignKey(r => r.StudentId);
    }
}

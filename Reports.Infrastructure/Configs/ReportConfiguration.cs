using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Configs;
internal class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(x => x.Student);
    }
}

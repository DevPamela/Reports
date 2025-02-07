using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Configs
{
    internal class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder
            .HasKey(c => c.Id);

            builder
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Class)
                .HasForeignKey(c => c.TeacherId);

            builder
                .HasMany(c => c.Students)
                .WithOne(s => s.Class)
                .HasForeignKey(s => s.ClassRoomId);
        }
    }
}
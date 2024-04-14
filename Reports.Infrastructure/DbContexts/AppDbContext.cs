using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Infrastructure.DbContexts;

public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}

using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.Configs;

namespace Reports.Infrastructure.DbContexts;

public class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<ClassRoom> Classroom { get; set; } = null!;
    public DbSet<Assessment> Assessments { get; set; } = null!;
    public DbSet<Teacher> Teacher { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new GoalConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
    }
        
}

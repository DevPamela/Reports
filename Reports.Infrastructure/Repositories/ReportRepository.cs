using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;
using Reports.Infrastructure.DbContexts;


namespace Reports.Infrastructure.Repositories
{
    internal class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _dbContext;

        public ReportRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        /// <inheritdoc />
        public void Add(Report report) =>
            _dbContext.Report.Add(report);

        /// <inheritdoc />
        public async Task<List<Report>> GetAllAsync(int id) =>
            await _dbContext.Report.ToListAsync();

        /// <inheritdoc />
        public async Task<Report?> GetByNameAsync(string name) =>
            await _dbContext.Report
                .FirstOrDefaultAsync(s => s.Name == name);
        public async Task<Report> GetReportById(int id) =>
            await  _dbContext.Report
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);
    }
}


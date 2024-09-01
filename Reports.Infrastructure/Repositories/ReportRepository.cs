using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
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

        public Task<Student?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Report?> GetByNameAsync(string name) =>
            await _dbContext.Report
                .FirstOrDefaultAsync(s => s.Name == name);
        public async Task<Report> ObterRelatorioPorId(int idDoRelatorio) =>
              _dbContext.Report
                .Include(r => r.Student)
                .FirstOrDefault(r => r.Id == idDoRelatorio);

        //   return await relatorio;
    }
}


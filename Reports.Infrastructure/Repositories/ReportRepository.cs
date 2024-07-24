using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
            _dbContext.Reports.Add(report);

        /// <inheritdoc />
        public async Task<List<Report>> GetAllAsync(int id) =>
            await _dbContext.Reports.ToListAsync();

        public Task<Student?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Report?> GetByNameAsync(string name) =>
            await _dbContext.Reports
                .FirstOrDefaultAsync(s => s.Name == name);
        public async Task<Report> ObterRelatorioPorId(int idDoRelatorio) =>
              _dbContext.Reports
                .Include(r => r.Student)
                .FirstOrDefault(r => r.Id == idDoRelatorio); 

         //   return await relatorio;
    }
}


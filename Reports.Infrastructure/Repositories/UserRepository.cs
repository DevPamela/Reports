using Microsoft.EntityFrameworkCore;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Repositories;
using Reports.Infrastructure.DbContexts;
using System.Linq.Expressions;

namespace Reports.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.AnyAsync(predicate);
        }
        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }
    }
}

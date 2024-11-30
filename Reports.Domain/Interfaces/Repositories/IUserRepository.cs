using Reports.Domain.Entities;
using System.Linq.Expressions;

namespace Reports.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}

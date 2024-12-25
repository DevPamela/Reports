using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Service
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<User?> ValidateUserAsync(string username, string password);
    }
}

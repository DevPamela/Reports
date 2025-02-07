using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;

namespace Reports.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        // Register a new user
        public async Task<User> RegisterUserAsync(string username, string email, string password)
        {
            if (await _userRepository.AnyAsync(u => u.Email == email))
            {
                throw new InvalidOperationException("A user with the same username or email already exists.");
            }

            var hashedPassword = HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddUserAsync(user); // Use the repository to add the user
            await _unitOfWork.SaveChangesAsync(); // Commit changes via Unit of Work

            return user;
        }

        // Validate user credentials
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        // Helper: Hash the password
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Helper: Verify the password
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

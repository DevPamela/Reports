using Moq;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Service;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUnitOfWork.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var email = "test@example.com";
        var username = "testuser";
        var password = "password123";

            _mockUserRepository
    .Setup(repo => repo.AnyAsync(It.Is<Expression<Func<User, bool>>>(expr => expr.Compile()(new User { Email = email }))))
    .ReturnsAsync(true); 


        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.RegisterUserAsync(username, email, password));

        Assert.Equal("A user with the same username or email already exists.", exception.Message);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnUser_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var email = "test@example.com";
        var username = "testuser";
        var password = "password123";

        _mockUserRepository
           .Setup(repo => repo.AnyAsync(It.Is<Expression<Func<User, bool>>>(expr => expr.Compile()(new User { Email = email }))))
            .ReturnsAsync(false); 

        _mockUserRepository
            .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask); 

        _mockUnitOfWork
     .Setup(uow => uow.SaveChangesAsync(default))
     .Callback(() => Task.CompletedTask); 



        // Act
        var result = await _userService.RegisterUserAsync(username, email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task ValidateUserAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "password123";

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null); 

        // Act
        var result = await _userService.ValidateUserAsync(email, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidateUserAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        // Arrange
        var email = "test@example.com";
        var password = "wrongpassword";
        var storedUser = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword") 
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(storedUser); 

        // Act
        var result = await _userService.ValidateUserAsync(email, password);

        // Assert
        Assert.Null(result); 
    }

    [Fact]
    public async Task ValidateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
    {
        // Arrange
        var email = "test@example.com";
        var password = "correctpassword";
        var storedUser = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) 
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(storedUser); 

        // Act
        var result = await _userService.ValidateUserAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }
}

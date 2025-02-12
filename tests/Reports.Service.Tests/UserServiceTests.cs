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
    .ReturnsAsync(true); // Simula que o email já está registrado


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
            .ReturnsAsync(false); // Simula que o email não está registrado

        _mockUserRepository
            .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask); // Simula sucesso na adição do usuário

        _mockUnitOfWork
     .Setup(uow => uow.SaveChangesAsync(default))
     .Callback(() => Task.CompletedTask); // Chama diretamente o Task.CompletedTask



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
            .ReturnsAsync((User?)null); // Simula que o usuário não foi encontrado

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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword") // Senha correta é "correctpassword"
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(storedUser); // Simula que o usuário foi encontrado

        // Act
        var result = await _userService.ValidateUserAsync(email, password);

        // Assert
        Assert.Null(result); // Senha incorreta
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // Senha correta
        };

        _mockUserRepository
            .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(storedUser); // Simula que o usuário foi encontrado

        // Act
        var result = await _userService.ValidateUserAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }
}

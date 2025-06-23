using Moq;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Service;

public class GoalServiceTests
{
    private readonly Mock<IStudentService> _mockStudentService;
    private readonly Mock<IGoalRepository> _mockGoalRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAssessmentService> _mockAssessmentService;
    private readonly GoalService _goalService;

    public GoalServiceTests()
    {
        _mockStudentService = new Mock<IStudentService>();
        _mockGoalRepository = new Mock<IGoalRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssessmentService = new Mock<IAssessmentService>();
        _goalService = new GoalService(
            _mockStudentService.Object,
            _mockGoalRepository.Object,
            _mockUnitOfWork.Object,
            _mockAssessmentService.Object
        );
    }


    [Fact]
    public async Task AddGoalsAsync_ShouldAddNewGoal_WhenGoalDoesNotExist()
    {
        // Arrange
        var goalName = "NewGoal";
        var collectionName = "Satisfatório";
        var description = "Description of the goal";
        var isGirlsGoal = false;
        var teacherId = 1;

        // Simula a ausência da meta
       // _mockGoalRepository.Setup(r => r.GetGirlsGoalByNameAsync(goalName)).ReturnsAsync((Goal)null);

        // Act
        await _goalService.AddGoalsAsync(collectionName, goalName, description, isGirlsGoal, teacherId);

        // Assert
        _mockGoalRepository.Verify(r => r.Add(It.IsAny<Goal>()), Times.Once); // Verifica se a meta foi adicionada
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verifica se SaveChangesAsync foi chamado
    }

    [Fact]
    public async Task GetDescriptionForParagrafAsync_ShouldReturnCorrectDescription()
    {
        // Arrange
        var assessment = new Assessment
        {
            Goal = new Goal { Name = "Goal1" },
            IsSatisfactory = "S"
        };
        var teacherId = 1;
        var isGirl = false;

        var goal = new GirlsGoal
        {
            Name = "Goal1",
            GirlsAtendedDescription = "Girls attended description"
        };

        _mockGoalRepository.Setup(r => r.GetGirlsGoalsByNameAndTeacherIdAsync("Goal1", teacherId)).ReturnsAsync(goal);

        // Act
        var result = await _goalService.GetDescriptionForParagrafAsync(assessment, teacherId, isGirl);

        // Assert
        Assert.Equal("Girls attended description", result); // Verifica se a descrição correta foi retornada
    }
}

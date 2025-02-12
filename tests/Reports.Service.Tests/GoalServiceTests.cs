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
    private readonly Mock<IBulkInsertHelper> _mockBulkInsertHelper;
    private readonly GoalService _goalService;

    public GoalServiceTests()
    {
        _mockStudentService = new Mock<IStudentService>();
        _mockGoalRepository = new Mock<IGoalRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAssessmentService = new Mock<IAssessmentService>();
        _mockBulkInsertHelper = new Mock<IBulkInsertHelper>();
        _goalService = new GoalService(
            _mockStudentService.Object,
            _mockGoalRepository.Object,
            _mockUnitOfWork.Object,
            _mockAssessmentService.Object,
            _mockBulkInsertHelper.Object
        );
    }

    [Fact]
    public async Task SaveStudentsGoalsAsync_ShouldSaveSelectedGoalsForStudents()
    {
        // Arrange
        var goals = new List<string> { "Goal1", "Goal2" };
        var goal1 = new Goal { Name = "Goal1", IsActive = false };
        var goal2 = new Goal { Name = "Goal2", IsActive = false };

        _mockGoalRepository.Setup(r => r.GetByNameAsync("Goal1")).ReturnsAsync(goal1);
        _mockGoalRepository.Setup(r => r.GetByNameAsync("Goal2")).ReturnsAsync(goal2);

        // Simula o GetByIsSelectedProperty retornando uma lista de alunos selecionados
        var students = new List<Student>
        {
            new Student { Id = 1, Name = "Student1", Goals = new List<Goal>() },
            new Student { Id = 2, Name = "Student2", Goals = new List<Goal>() }
        };
        _mockStudentService.Setup(s => s.GetByIsSelectedProperty()).ReturnsAsync(students);

        _mockGoalRepository.Setup(r => r.GetByIsSelectedProperty()).ReturnsAsync(new List<Goal> { goal1, goal2 });

        // Act
        var result = await _goalService.SaveStudentsGoalsAsync(goals);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // Deveria retornar a lista de estudantes
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verifica se SaveChangesAsync foi chamado uma vez
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
        _mockGoalRepository.Setup(r => r.GetGirlsGoalByNameAsync(goalName)).ReturnsAsync((Goal)null);

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

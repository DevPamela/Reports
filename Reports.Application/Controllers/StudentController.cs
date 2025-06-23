using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Extensions;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;
using Reports.Service.UseCases.RegisterStudentsFromFile;

namespace Reports.Application.Controllers;

[Authorize]
public class StudentController : BaseController<StudentController>
{
    private readonly IStudentService _studentService;
    private readonly IClassroomService _classroomService;
    private readonly ITeacherService _teacherService;

    public StudentController(
        IStudentService studentService,
        IClassroomService classroomService,
        ITeacherService teacherService,
        ILogger<StudentController> logger) : base(logger)
    {
        _studentService = studentService;
        _classroomService = classroomService;
        _teacherService = teacherService;
    }

    public IActionResult Index()
    {
        Logger.LogInformation("Student Index page accessed.");
        return View();
    }

    public ActionResult Error()
    {
        Logger.LogWarning("An error page was accessed.");
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> RegisterStudentsFromFile(
        [FromServices] RegisterStudentsFromFileUseCase useCase,
        [FromServices] IValidator<RegisterStudentsFromFileViewModel> validator,
        RegisterStudentsFromFileViewModel viewModel)
    {
        var userId = GetUserIdFromSession();

        try
        {
            Logger.LogInformation("Registering students from file: {FileName}", viewModel.ExcelFile.FileName);

            var validationResult = await validator.ValidateAsync(viewModel);
            if (!validationResult.IsValid)
            {
                Logger.LogWarning("Validation failed for file: {FileName}.", viewModel.ExcelFile.FileName);
                validationResult.AddToModelState(ModelState);
                return View(nameof(Index), viewModel);
            }

            var teacher = await _teacherService.GetByUserIdAsync(int.Parse(userId));
            var stream = viewModel.ExcelFile.OpenReadStream();
            var request = new RegisterStudentsFromFileRequest(stream, viewModel.ClassroomName, teacher.Id);
            var response = await useCase.ExecuteAsync(request);

            if (response.IsFailed)
            {
                Logger.LogWarning("Failed to register students from file: {FileName}.", viewModel.ExcelFile.FileName);
                response.AddToModelState(ModelState);
                return View(nameof(Index), viewModel);
            }
            SetNotification("Turma cadastrada com sucesso!", Enums.NotificationType.Success);
            Logger.LogInformation("Students registered successfully from file: {FileName}.", viewModel.ExcelFile.FileName);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred for User {UserId} during student registration.", userId);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            return View(nameof(Index), viewModel);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetStudentByName(string name)
    {
        Logger.LogInformation("Retrieving student by name: {StudentName}", name);
        var userId = GetUserIdFromSession();
        var student = await _studentService.GetStudentByNameAsync(name, int.Parse(userId));
        return Ok(student);
    }

    [HttpGet]
    public async Task<ActionResult> SelectStudentsForReport()
     {
        var userId = GetUserIdFromSession();
        var selectedClasses = TempData["SelectedClasses"] as IEnumerable<int>;
        var students = await _studentService.GetStudentsListAsync(int.Parse(userId));
        var filteredStudents = await _studentService.GetStudentsSelectedByClasses(selectedClasses, int.Parse(userId));
        await _studentService.ClearSelection(students, int.Parse(userId));

        var filteredStudentsViewModel = filteredStudents.Select(s => new StudentViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Birthdate = s.Birthdate,
        }).ToList();

        return View(new StudentViewModelList { Students = filteredStudentsViewModel });
    }

    [HttpPost]
    public async Task<ActionResult> SetIsSelectedPropertyStudentAsTrue(List<int> studentsId)
     {
        Logger.LogInformation("Setting 'IsSelected' property to true for students.");

        var userId = GetUserIdFromSession();
        var students = await _studentService.GetStudentsByIdListAsync(studentsId, int.Parse(userId));
        await _studentService.SetStudentAsTrue(students, int.Parse(userId));

        return RedirectToAction("GetAbsencesNumber", "Student");
    }

    [HttpGet]
    public async Task<ActionResult> GetAbsencesNumber()
    {
        Logger.LogInformation("Accessing student absence data.");
        var userId = GetUserIdFromSession();
        var studentList = await _studentService.GetByIsSelectedProperty(int.Parse(userId));
        var filteredStudentsViewModel = studentList.Select(s => new StudentViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Birthdate = s.Birthdate,
        }).ToList();

        return View(new StudentViewModelList { Students = filteredStudentsViewModel });
    }

    [HttpPost]
    public async Task<ActionResult> SetAbsencesNumber(List<Student> students)
    {
        Logger.LogInformation("Setting absence numbers for students.");
        var userId = GetUserIdFromSession();
        await _studentService.SetAbsencesNumber(students, int.Parse(userId));
        return RedirectToAction("SelectGoalsForReport", "Goals");
    }

    [HttpGet]
    public async Task<ActionResult> StudentsList()
    {
        var userId = GetUserIdFromSession();
        Logger.LogInformation($"Retrieving the student list for teacher id: {userId}.");
        
        var students = await _studentService.GetStudentsListAsync(int.Parse(userId));
        var filteredStudentsViewModel = students.Select(s => new StudentViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Birthdate = s.Birthdate,
            Gender= s.Gender,
        }).ToList();

        return View(new StudentViewModelList { Students = filteredStudentsViewModel });
    }

    [HttpGet]
    public async Task<ActionResult> EditStudent(int id)
    {
        Logger.LogInformation("Accessing edit page for student with ID: {StudentId}", id);

        var userId = GetUserIdFromSession();
        var teacherId = await _teacherService.GetByUserIdAsync(int.Parse(userId));
        var student = await _studentService.GetByIdAsync(id);
        var classroom = await _classroomService.GetByTeacherId(teacherId.Id);

        return View(new ClassroomAndStudentViewModel { Student = student, Classes = classroom });
    }

    [HttpPost]
    public async Task<ActionResult> UpdateStudent(Student student)
    {
        Logger.LogInformation("Updating student with ID: {StudentId}", student.Id);

        await _studentService.UpdateStudent(student);
        SetNotification("Dados do aluno alterados com sucesso!", Enums.NotificationType.Success);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<ActionResult> RemoveStudent(int id)
    {
        Logger.LogInformation("Accessing remove page for student with ID: {StudentId}", id);

        var student = await _studentService.GetByIdAsync(id);

        var studentViewModel = new StudentViewModel
        {
            Id = student.Id,
            Name = student.Name,
            Birthdate = student.Birthdate,
        };
        return View(new StudentViewModelList { Student = studentViewModel });
    }

    [HttpPost]
    public async Task<ActionResult> DeleteStudent(Student student)
    {
        try
        {
            Logger.LogInformation("Deleting student with ID: {StudentId}", student.Id);

            await _studentService.DeleteStudent(student);
            SetNotification("Aluno deletado da base de dados!", Enums.NotificationType.Success);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            var studentViewModel = new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Birthdate = student.Birthdate,
              
            };
            Logger.LogError(ex, "An error occurred while deleting student with ID: {StudentId}.", student.Id);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            return View("RemoveStudent", new StudentViewModelList { Student = studentViewModel });
        }
    }

    public async Task<ActionResult> AddNewStudent()
    {
        Logger.LogInformation("Accessing add new student page.");
        var userId = GetUserIdFromSession();
        var classes = await _classroomService.GetAllAsync(int.Parse(userId));
        return View(new ClassroomAndStudentViewModel { Classes = classes });
    }

    [HttpPost]
    public async Task<ActionResult> AddStudent(Student student)
    {
        try
        {
            Logger.LogInformation("Adding a new student.");

            var userId = GetUserIdFromSession();
            var teacher = await _teacherService.GetByUserIdAsync(int.Parse(userId));
            student.Teacher = teacher;
            student.TeacherId = teacher.Id;
            await _studentService.AddStudentAsync(student);

            Logger.LogInformation("Successfully added student with ID: {StudentId}", student.Id);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred for User {UserId} while adding a student.", GetUserIdFromSession());
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            return View("AddNewStudent");
        }
    }
}
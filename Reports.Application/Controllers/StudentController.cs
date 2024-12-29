using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Extensions;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using Reports.Service.UseCases.RegisterStudentsFromFile;
using Microsoft.AspNetCore.Authorization;


namespace Reports.Application.Controllers;

[Authorize]
public class StudentController : BaseController
{
    private readonly IStudentService _studentService;
    private readonly IClassroomService _classroomService;
    private readonly ITeacherService _teacherService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(
        IStudentService studentService,
        IClassroomService classroomService,
        ITeacherService teacherService,
        ILogger<StudentController> logger) : base(logger)
    {
        _studentService = studentService;
        _classroomService = classroomService;
        _teacherService = teacherService;
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }
    public ActionResult Error()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> RegisterStudentsFromFile(
        [FromServices] RegisterStudentsFromFileUseCase useCase,
        [FromServices] IValidator<RegisterStudentsFromFileViewModel> validator,
        RegisterStudentsFromFileViewModel viewModel)
    {
        var validationResult = await validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(nameof(Index), viewModel);
        }
        var user = User.FindFirst("UserId")?.Value;

        var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
        var stream = viewModel.ExcelFile.OpenReadStream();
        var request = new RegisterStudentsFromFileRequest(stream, viewModel.ClassroomName, teacher.Id);
        var response = await useCase.ExecuteAsync(request);

        if (response.IsFailed)
        {
            response.AddToModelState(ModelState);
            return View(nameof(Index), viewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<ActionResult> GetStudentByName(string name)
    {
        var student = await _studentService.GetStudentByNameAsync(name);
        return Ok(student);
    }

    [HttpGet]
    public async Task<ActionResult> SelectStudentsForReport()
    {
        var selectedClasses = TempData["SelectedClasses"] as IEnumerable<int>;

        var students = await _studentService.GetStudentsListAsync();


        var filteredStudents = await _studentService.GetStudentsSelectedByClasses(selectedClasses);
        await _studentService.ClearSelection(students);
        var model = new StudentViewModel()
        {
            Students = filteredStudents
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> SetIsSelectedPropertyStudentAstrue(List<int> studentsId)
    {
        var students = await _studentService.GetStudentsByIdListAsync(studentsId);

        await _studentService.SetStudentAsTrue(students);

        return RedirectToAction("GetAbsencesNumber", "Student");
    }

    [HttpGet]
    public async Task<ActionResult> GetAbsencesNumber()
    {
        var studentList = await _studentService.GetByIsSelectedProperty();

        var model = new StudentViewModel
        {
            Students = studentList
        };
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> SetAbsencesNumber(List<Student> students)
    {
        await _studentService.SetAbsencesNumber(students);

        return RedirectToAction("SelectGoalsForReport", "Goals");
    }

    [HttpGet]
    public async Task<ActionResult> StudentsList()
    {
        var students = await _studentService.GetStudentsListAsync();
        var model = new StudentViewModel
        {
            Students = students
        };

        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> EditStudent(int id)
    {
        var user = User.FindFirst("UserId")?.Value;
        var teacherId = await _teacherService.GetByUserIdAsync(int.Parse(user));
        var student = await _studentService.GetByIdAsync(id);
        var classroom = await _classroomService.GetByTeacherId(teacherId.Id);
        var model = new ClassroomAndStudentViewModel
        {
            Student = student,
            Classes = classroom
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateStudent(Student student)
    {
        await _studentService.UpdateStudent(student); //TODO ver genero chegando errado, ver data de nascimento que não está preencida, acrescentar botões de voltar, ver cookies de autenticação

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<ActionResult> RemoveStudent(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        var model = new StudentViewModel
        {
            Student = student
        };
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteStudent(Student student)
    {
        await _studentService.DeleteStudent(student);

        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> AddNewStudent()
    {
        var classes = await _classroomService.GetAllAsync();

        var model = new ClassroomAndStudentViewModel()
        {
            // Classes = classes //TODO
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> AddStudent(Student student)
    {
        var user = User.FindFirst("UserId")?.Value;
        var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
        student.Teacher = teacher;
        student.TeacherId = teacher.Id;
        await _studentService.AddStudentAsync(student);

        return RedirectToAction("Index", "Home");
    }
}
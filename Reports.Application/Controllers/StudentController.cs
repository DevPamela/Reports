using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Extensions;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using Reports.Service.UseCases.RegisterStudentsFromFile;

namespace Reports.Application.Controllers;

public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IClassroomService _classroomService;

    public StudentController(
        IStudentService studentService,
        IClassroomService classroomService)
    {
        _studentService = studentService;
        _classroomService = classroomService;
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

        var stream = viewModel.ExcelFile.OpenReadStream();
        var request = new RegisterStudentsFromFileRequest(stream, viewModel.ClassroomName, 1);
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
        var student = await _studentService.GetByIdAsync(id);
        var classroom = await _classroomService.GetAllAsync();
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
        await _studentService.UpdateStudent(student);

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
            Classes = classes
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> AddStudent(Student student)
    {
        await _studentService.AddStudentAsync(student);

        return RedirectToAction("Index", "Home");
    }
}
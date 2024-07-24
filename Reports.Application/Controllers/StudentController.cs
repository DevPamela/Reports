using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using Reports.Service;
using System.Linq;

namespace Reports.Application.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IClassroomService _classroomService;

        public StudentController(IStudentService studentService, IClassroomService classroomService)
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

        public ActionResult Success(int id)
        {
            var students = _studentService.GetStudentsListAsync().Result;
            var selectedStudents = students.Where(_ => _.ClassRoomId == id).ToList();
            var viewModel = new StudentViewModel()
            {
                Students = selectedStudents
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterStudentByList([FromForm] IFormFile excelFile, [FromForm] string classroomName)
        {
            try
            {
                //TODO resolver object tamanho das fontes navmenu ellipse object problema datalist home que não acha a class css
                //TODO bloquear mais de um aluno com mesmo nome
                string[] param = { "nome", "data de nascimento" };

                var students = await _studentService.AddStudentByListAsync(excelFile, param);
                await _classroomService.SetClassRommInformation(classroomName, students, 1);
                int classroomId = students.FirstOrDefault()?.Class.Id ?? 0;
                //return RedirectToAction("Success", new { id = classroomId });
                return Json(new { redirectUrl = Url.Action("Success", new { id = classroomId }) });
            }
            catch (Exception e)
            {
                //TODO incluir mensagem de erro na view
                return Json(new { redirectUrl = Url.Action("Error") });
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveStudentGender(StudentViewModel model)
        {
            await _studentService.UpdateStudentList(model.Students);

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

            var filteredStudents = students.Where(student => selectedClasses.Contains(student.ClassRoomId ?? default)).ToList();
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
            var students = await _studentService.GetStudentsListAsync();
            var selectedStudents = students.Where(_ => studentsId.Contains(_.Id)).ToList();
            await _studentService.SetStudentAsTrue(selectedStudents);
          

            return RedirectToAction("GetAbsencesNumber", "Student");
        }

        [HttpGet]
        public async Task<ActionResult> GetAbsencesNumber()
        {
            var students = await _studentService.GetStudentsListAsync();
            var studentList = students.Where(_ => _.IsSelected == true).ToList();
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
}
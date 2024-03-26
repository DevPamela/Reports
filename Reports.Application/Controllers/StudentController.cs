using Microsoft.AspNetCore.Mvc;
using Reports.Domain.Interfaces;
using Reports.Service;

namespace Reports.Application.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;

        }
        public IActionResult Index()
        {

            return View();
        }
        //TODO showdata terá uma view que irá exibir a lista de alunos com a possibilidade de editar essa lista
        //TODO mudar o nome do metodo
        //TODO resolver o tratamento caso, a tabela não seja encontrada
        [HttpPost]
        public ActionResult ShowData([FromForm] IFormFile excelFile)
        {
            //TODO resolver object tamanho das fontes navmenu ellipse object problema datalist home que não acha a class css
            var header = new ExcelFileHeaderService(_studentService);
            string[] param = { "nome", "faltas", "data de nascimento" };
            var students = _studentService.AddStudentByList(excelFile, param);

            return View(students);
        }

        [HttpGet]
        public ActionResult GetStudentByName(string name)
        {
            var student = _studentService.GetStudentByName(name);
            return Ok(student);
        }

        //[HttpGet]
        //public ActionResult GetStudentsList(List<Student>students)
        //{

        //    return View(students);
        //}
        //[HttpPut]
        //public ActionResult PutStudent(Student student)
        //{
        //    _studentService.
        //}
        //TODO ajustar avaliações para expandir opções, melhorar layout
        //[HttpPost]
        //public ActionResult SaveGoals(Dictionary<string, Dictionary<string, string>> avaliacao)
        //{
        //    _studentService.SalvarAvaliacoes(avaliacao);
        //    var any = _studentService.GetStudentsList();

        //    return View();
        //}

        //public ActionResult SelecionarObjetivos()
        //{


        //    List<string> objetivos = new List<string>() { "Adaptação", "interação", "roda De Conversa", "Coordenação Motora", "Resolucao De Conflitos" };

        //    ViewBag.Objetivos = objetivos;
        //    return View(objetivos);
        //}

        //[HttpPost]
        //public ActionResult GetStudentsList(List<string> goals)
        //{
        //    var students = _studentService.SaveStudentsGoals(goals);
        //    return View(students);

        //}

    }
}


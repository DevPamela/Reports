using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using System.Diagnostics;

namespace Reports.Application.Controllers
{
    public class HomeController : Controller
    {
       private readonly IClassroomService _classroomService;
        ITeacherService _teacherService;
        public HomeController(IClassroomService classroomService, ITeacherService teacherService)
        {
            _classroomService = classroomService;
            _teacherService = teacherService;
        }

        public async Task<IActionResult> Index()
        {
           var classrooms= await _classroomService.GetAllAsync();
            var teacherName = new Teacher();
            foreach (var classroom in classrooms)
            {
                 teacherName = await _teacherService.GetByIdAsync(classroom.TeacherId);
            }

            HomeModel home = new HomeModel()
            {
                Name = teacherName.Name,
                Classrooms = classrooms
            };

            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
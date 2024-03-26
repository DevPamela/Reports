using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using System.Diagnostics;

namespace Reports.Application.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            HomeModel home = new HomeModel();

            home.Nome = "Pamela";
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
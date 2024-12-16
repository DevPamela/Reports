using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Domain.Interfaces.Service;
using System.Security.Claims;
using Reports.Application.Models;
using Reports.Service;
using Reports.Domain.Entities;

namespace Reports.Application.Controllers
{
    [AllowAnonymous] // Allow unauthenticated access
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        public UserController(IUserService userService, ITeacherService teacherService)
        {
            _userService = userService;
            _teacherService = teacherService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.ValidateUserAsync(model.email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync("CookieAuthentication", new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userService.RegisterUserAsync(model.Username, model.Email, model.Password);//chamar o register teacher
                //cadastrar teacher 
                var teacher = new Teacher { Name = model.Username, UserId = user.Id };
                await _teacherService.RegisterTeacher(teacher);
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuthentication");
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Login", "User");
        }
    }
}

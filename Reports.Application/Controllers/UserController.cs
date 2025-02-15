using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Reports.Application.Models;
using Reports.Application.Models.Configuration;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;
using System.Security.Claims;

namespace Reports.Application.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly int _sessionTimeoutMinutes;

        public UserController(IUserService userService, ITeacherService teacherService, ILogger<UserController> logger, IOptions<SessionConfig> sessionConfig) : base(logger)
        {
            _userService = userService;
            _teacherService = teacherService;
            _sessionTimeoutMinutes = sessionConfig.Value.IdleTimeoutMinutes;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Validate user credentials
                var user = await _userService.ValidateUserAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                // Store user details in session
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("LastActiveTime", DateTime.UtcNow.ToString());

                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Email, user.Email),
                    new("UserId", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user with a cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(_sessionTimeoutMinutes)
                });

                SetNotification("Login successful!", Enums.NotificationType.Success);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Logger.LogError("Unexpected error during login: {Message}", ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Register the user
                var user = await _userService.RegisterUserAsync(model.Username, model.Email, model.Password);

                // Register a teacher if applicable
                await _teacherService.RegisterTeacher(new Teacher { Name = model.Username, UserId = user.Id });

                SetNotification("User registered successfully!", Enums.NotificationType.Success);
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError("Error during user registration: {Message}", ex.Message);
                ModelState.AddModelError("", "Invalid operation. Please review the provided data.");
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Unexpected error during registration: {Message}", ex.Message);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Clear session and sign out user
                HttpContext.Session.Clear();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                Logger.LogError("Unexpected error during logout: {Message}", ex.Message);
                SetNotification("An unexpected error occurred during logout. Please try again.", Enums.NotificationType.Error);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

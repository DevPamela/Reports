using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Reports.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Reports.Application.Controllers
{
    public class AuthorizationController : BaseController
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IUserService _userService;
        public AuthorizationController(IOpenIddictApplicationManager applicationManager, ILogger<AuthorizationController> logger, IUserService userService) : base(logger)
        {
            _applicationManager = applicationManager;
            _logger = logger;
            _userService = userService;
        }


        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Login(string email, string password, HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid credentials.");
            }

            // Validate user credentials
            var user = await _userService.ValidateUserAsync(email, password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Store user details in session
            httpContext.Session.SetString("UserId", user.Id.ToString());
            httpContext.Session.SetString("Username", user.Username);

            // Optional: Issue cookie for session management
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok(new { message = "Login successful." });
        }

        /// <summary>
        /// Handles logout requests.
        /// </summary>
        [HttpPost("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore;
using Microsoft.IdentityModel.Tokens;

namespace Reports.Application.Controllers
{
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request == null)
            {
                return BadRequest(new { error = "Invalid request." });
            }

            if (request.IsPasswordGrantType())
            {
                return await HandlePasswordGrantAsync(request);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                return await HandleRefreshTokenGrantAsync(request);
            }

            return BadRequest(new { error = "Unsupported grant type." });
        }

        private async Task<IActionResult> HandlePasswordGrantAsync(OpenIddictRequest request)
        {
            var user = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            var principal = CreateClaimsPrincipal(user);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> HandleRefreshTokenGrantAsync(OpenIddictRequest request)
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Unauthorized(new { error = "Invalid refresh token." });
            }

            var userIdClaim = result.Principal?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { error = "Invalid token." });
            }

            var user = await _userService.GetUserByIdAsync(userIdClaim.Value);
            if (user == null)
            {
                return Unauthorized(new { error = "User not found." });
            }

            var principal = CreateClaimsPrincipal(user);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(Claims.Subject, user.Id.ToString()),
                new(Claims.PreferredUsername, user.Username)
            };

            var identity = new ClaimsIdentity(claims, TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile, Scopes.OfflineAccess);
            principal.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));

            return principal;
        }
    }
}

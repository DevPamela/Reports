using Microsoft.AspNetCore.Mvc;

namespace Reports.Application.Controllers
{
    public class SessionController : BaseController<SessionController>
    {
        public SessionController(ILogger<SessionController> logger) : base(logger) { }

        [HttpGet("check-session")]
        public IActionResult CheckSession()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                Logger.LogInformation("user is Unauthorized");
                return Unauthorized();
            }
            Logger.LogInformation("user is authorized");
            return Ok();
        }
    }
}

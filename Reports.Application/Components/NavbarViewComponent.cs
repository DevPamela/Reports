using Microsoft.AspNetCore.Mvc;

namespace Reports.Application.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Example: Pass data to the view if needed
            var userIsAuthenticated = User.Identity.IsAuthenticated;
            var userName = userIsAuthenticated ? User.Identity.Name : null;

            ViewData["UserName"] = userName;

            return View();
        }
    }
}

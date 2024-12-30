using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Enums;
using Reports.Application.Models;
using System.Diagnostics;

namespace Reports.Application.Controllers
{
    /// <summary>
    /// A base controller providing shared functionality for all controllers in the application.
    /// </summary>
    /// <typeparam name="TController">The type of the derived controller.</typeparam>
    public abstract class BaseController<TController> : Controller
    {
        /// <summary>
        /// The logger used for logging informational, warning, and error messages.
        /// </summary>
        protected readonly ILogger<TController> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{TController}"/> class.
        /// </summary>
        /// <param name="logger">An <see cref="ILogger{TController}"/> instance for logging.</param>
        protected BaseController(ILogger<TController> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logs an error and displays the error page.
        /// </summary>
        /// <param name="statusCode">Optional HTTP status code.</param>
        /// <param name="message">Optional custom error message.</param>
        /// <returns>The error view.</returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error(int? statusCode = null, string? message = null)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            Logger.LogError("An error occurred. StatusCode: {StatusCode}, RequestId: {RequestId}, Message: {Message}",
                statusCode, requestId, message ?? "An unexpected error occurred.");

            var model = new ErrorViewModel
            {
                RequestId = requestId,
                StatusCode = statusCode,
                Message = message ?? "An unexpected error occurred."
            };

            return View("Error", model);
        }

        /// <summary>
        /// Handles asynchronous error scenarios and returns the error view.
        /// </summary>
        /// <param name="statusCode">Optional HTTP status code.</param>
        /// <param name="message">Optional custom error message.</param>
        /// <returns>A Task<IActionResult> for the error view.</returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> ErrorAsync(int? statusCode = null, string? message = null)
        {
            return await Task.FromResult(Error(statusCode, message));
        }

        /// <summary>
        /// Sets a notification message to be displayed to the user.
        /// </summary>
        /// <param name="message">The notification message.</param>
        /// <param name="type">The type of notification (e.g., "info", "success", "warning", "error").</param>
        protected void SetNotification(string message, NotificationType type = NotificationType.Info)
        {
            TempData["Notification"] = message;
            TempData["NotificationType"] = type;
        }

        /// <summary>
        /// Executes an action with centralized exception handling.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The result of the executed action, or an error redirection if an exception occurs.</returns>
        protected IActionResult ExecuteWithHandling(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred during execution.");
                SetNotification("An unexpected error occurred.", NotificationType.Error);
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Retrieves the UserId from the session.
        /// </summary>
        /// <returns>The UserId if available, otherwise null.</returns>
        protected string GetUserIdFromSession()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                Logger.LogWarning("UserId not found in session.");
                return null;
            }
            return userId;
        }

        /// <summary>
        /// Retrieves the Username from the session.
        /// </summary>
        /// <returns>The Username if available, otherwise null.</returns>
        protected string GetUsernameFromSession()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                Logger.LogWarning("Username not found in session.");
                return null;
            }
            return username;
        }
    }
}

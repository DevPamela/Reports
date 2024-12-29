using Microsoft.AspNetCore.Mvc;
using Reports.Application.Enums;

namespace Reports.Application.Controllers
{
    /// <summary>
    /// A base controller providing shared functionality for all controllers in the application.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// The logger used for logging informational, warning, and error messages.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
        public BaseController(ILogger<BaseController> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        protected void LogInfo(string message) => Logger.LogInformation(message);
        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        protected void LogWarning(string message) => Logger.LogWarning(message);
        /// <summary>
        /// Logs an error message with an optional exception.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="ex">The exception to include in the log (optional).</param>
        protected void LogError(string message, Exception ex = null) => Logger.LogError(ex, message);

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
                LogError("An error occurred during execution.", ex);
                SetNotification("An unexpected error occurred.", NotificationType.Error);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

using ComplaintTracking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ComplaintTracking.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IErrorLogger _errorLogger;

        public ErrorController(IErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }

        [Route("Error")]
        public async Task<IActionResult> Error()
        {
            // Get the details of the exception that occurred
            IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                string currentUser = (User == null) ? "Unknown" : User.Identity.Name;

                string route = string.Concat(exceptionFeature.Path, Request.QueryString.ToString());

                // Log Error and return short ID for support purposes
                ViewData["shortId"] = await _errorLogger.LogErrorAsync(exceptionFeature.Error, currentUser, route);
            }

            return View("Error");
        }

        [Route("Error/Status/{statusCode}")]
        public async Task<IActionResult> ErrorStatusAsync(int statusCode)
        {
            // Get status code details
            string statusCodeDesc = "";
            if (ErrorLogger.StatusCodeDescriptions.ContainsKey(statusCode))
            {
                statusCodeDesc = ErrorLogger.StatusCodeDescriptions[statusCode];
            }

            ViewData["statusCode"] = statusCode.ToString();
            ViewData["statusCodeDesc"] = statusCodeDesc;

            // 404 & 400 errors don't get logged
            if (statusCode != StatusCodes.Status404NotFound && statusCode != StatusCodes.Status400BadRequest)
            {
                // Get the details of the exception that occurred
                string message = string.Concat("HTTP Error: ", statusCode);
                if (statusCodeDesc != null)
                {
                    message = string.Concat(message, " ", statusCodeDesc);
                }

                string currentUser = (User == null) ? "Unknown" : User.Identity.Name;

                string route;
                var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                if (feature != null)
                {
                    route = string.Concat(feature.OriginalPath, feature.OriginalQueryString);
                }
                else
                {
                    route = string.Concat(Request.Path.ToString(), Request.QueryString.ToString());
                }

                // Log Error
                ViewData["shortId"] = await _errorLogger.LogErrorAsync(message, currentUser, route, statusCode);
            }

            return View("Error");
        }
    }
}

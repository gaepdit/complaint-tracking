using ComplaintTracking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                // Log Error and return short ID for support purposes
                string pathAndQueryString = string.Concat(exceptionFeature.Path, Request.QueryString.ToString());
                ViewData["shortId"] = await _errorLogger.LogErrorAsync(exceptionFeature.Error, $"Error controller, full URL: {pathAndQueryString}");
            }

            return View("Error");
        }

        [Route("Error/Status/{statusCode}")]
        public async Task<IActionResult> ErrorStatusAsync(int statusCode)
        {
            // Get status code details
            string statusCodeDesc = "";
            if (_statusCodeDescriptions.ContainsKey(statusCode))
            {
                statusCodeDesc = _statusCodeDescriptions[statusCode];
            }

            ViewData["statusCode"] = statusCode.ToString();
            ViewData["statusCodeDesc"] = statusCodeDesc;

            // 404 and 400 errors don't get logged
            if (statusCode != StatusCodes.Status404NotFound && statusCode != StatusCodes.Status400BadRequest)
            {
                // Get the details of the exception that occurred
                string message = string.Concat("HTTP Error: ", statusCode);
                if (statusCodeDesc != null)
                {
                    message = string.Concat(message, " ", statusCodeDesc);
                }

                string pathAndQueryString;
                var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                if (feature != null)
                {
                    pathAndQueryString = string.Concat(feature.OriginalPath, feature.OriginalQueryString);
                }
                else
                {
                    pathAndQueryString = string.Concat(Request.Path.ToString(), Request.QueryString.ToString());
                }

                var customData = new Dictionary<string, object>
                {
                    { "HTTP Status Code", statusCode.ToString() },
                    { "HTTP Status Code Description", statusCodeDesc }
                };

                // Log Error by throwing new exception
                try
                {
                    throw new CtsStatusCodeException(message);
                }
                catch (CtsStatusCodeException ex)
                {
                    ViewData["shortId"] = await _errorLogger.LogErrorAsync(ex, $"ErrorStatusAsync, full URL: {pathAndQueryString}", customData);
                }
            }

            return View("Error");
        }

        //[Route("Error/TestUnhandledException")]
        //public IActionResult TestUnhandledException()
        //{
        //    throw new ArgumentException("Testing Unhandled Exception");
        //}

        //[Route("Error/TestHandledException")]
        //public async Task<IActionResult> TestHandledException()
        //{
        //    try
        //    {
        //        throw new ArgumentException("Testing Handled Exception");
        //    }
        //    catch (Exception ex)
        //    {
        //        var customData = new Dictionary<string, object>
        //        {
        //            { "Custom Data", "Some data" }
        //        };
        //        await _errorLogger.LogErrorAsync(ex, "Error/TestHandledException", customData);
        //    }

        //    return Ok();
        //}

        private static readonly Dictionary<int, string> _statusCodeDescriptions = new Dictionary<int, string>()
        {
            { 100, "Continue"},
            { 101, "Switching Protocols"},
            { 102, "Processing"},

            { 200, "OK"},
            { 201, "Created"},
            { 202, "Accepted"},
            { 203, "Non-Authoritative Information"},
            { 204, "No Content"},
            { 205, "Reset Content"},
            { 206, "Partial Content"},
            { 207, "Multi-Status"},
            { 208, "Already Reported"},
            { 226, "IM Used"},

            { 300, "Multiple Choices"},
            { 301, "Moved Permanently"},
            { 302, "Found"},
            { 303, "See Other"},
            { 304, "Not Modified"},
            { 305, "Use Proxy"},
            { 307, "Temporary Redirect"},
            { 308, "Permanent Redirect"},

            { 400, "Bad Request"},
            { 401, "Unauthorized"},
            { 402, "Payment Required"},
            { 403, "Forbidden"},
            { 404, "Not Found"},
            { 405, "Method Not Allowed"},
            { 406, "Not Acceptable"},
            { 407, "Proxy Authentication Required"},
            { 408, "Request Timeout"},
            { 409, "Conflict"},
            { 410, "Gone"},
            { 411, "Length Required"},
            { 412, "Precondition Failed"},
            { 413, "Payload Too Large"}, // RFC 7231
            { 414, "URI Too Long"}, // RFC 7231
            { 415, "Unsupported Media Type"},
            { 416, "Range Not Satisfiable"}, // RFC 7233
            { 417, "Expectation Failed"},
            { 418, "I'm A Teapot"},
            { 419, "Authentication Timeout"}, // Not defined in any RFC
            { 421, "Misdirected Request"},
            { 422, "Unprocessable Entity"},
            { 423, "Locked"},
            { 424, "Failed Dependency"},
            { 426, "Upgrade Required"},
            { 428, "Precondition Required"},
            { 429, "Too Many Requests"},
            { 431, "Request Header Fields Too Large"},
            { 451, "Unavailable For Legal Reasons"},

            { 500, "Internal Server Error"},
            { 501, "Not Implemented"},
            { 502, "Bad Gateway"},
            { 503, "Service Unavailable"},
            { 504, "Gateway Timeout"},
            { 505, "HTTP Version Not Supported"},
            { 506, "Variant Also Negotiates"},
            { 507, "Insufficient Storage"},
            { 508, "Loop Detected"},
            { 510, "Not Extended"},
            { 511, "Network Authentication Required"}
        };
    }

    [Serializable]
    internal class CtsStatusCodeException : Exception
    {
        public CtsStatusCodeException(string message) : base(message) { }
    }
}

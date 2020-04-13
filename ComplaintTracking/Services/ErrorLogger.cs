using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComplaintTracking.Services
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly IRavenClient _ravenClient;

        public ErrorLogger(
            IRavenClient ravenClient)
        {
            _ravenClient = ravenClient;
        }

        public async Task<string> LogErrorAsync(
            Exception exception,
            string currentUser = "",
            string route = "",
            int statusCode = 0)
        {
            if (SkipThisException(exception))
            {
                return null;
            }

            string shortId = ShortID.GetShortID();

            // Gather extra data
            exception.Data.Add("CTS Environment", CTS.CurrentEnvironment.ToString());
            exception.Data.Add("CTS Error ID", shortId);
            if (!string.IsNullOrEmpty(currentUser))
            {
                exception.Data.Add("Current CTS User", currentUser);
            }
            if (!string.IsNullOrEmpty(route))
            {
                exception.Data.Add("Route", route);
            }
            if (statusCode != 0)
            {
                exception.Data.Add("HTTP Status Code", statusCode);
                if (StatusCodeDescriptions.ContainsKey(statusCode))
                {
                    exception.Data.Add("HTTP Status Code Description", StatusCodeDescriptions[statusCode]);
                }
            }

            // Send to Sentry.io
            if (CTS.CurrentEnvironment != ServerEnvironment.Development)
            {
                await _ravenClient.CaptureAsync(new SentryEvent(exception));
            }

            return shortId;
        }

        private bool SkipThisException(Exception exception)
        {
            //if (exception is Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking.UvException 
            //    && exception.Message.Contains("Error -4077 ECONNRESET"))
            //{
            //    return true;
            //}

            return false;
        }

        public async Task<string> LogErrorAsync(
            string message,
            string currentUser,
            string route,
            int statusCode = 0)
        {
            var shortId = ShortID.GetShortID();

            // Gather extra data
            var extra = new Dictionary<string, string>
            {
                ["CTS Environment"] = CTS.CurrentEnvironment.ToString(),
                ["CTS Error ID"] = shortId
            };
            if (!string.IsNullOrEmpty(currentUser))
            {
                extra["Current CTS User"] = currentUser;
            }
            if (!string.IsNullOrEmpty(route))
            {
                extra["Route"] = route;
            }
            if (statusCode != 0)
            {
                extra["HTTP Status Code"] = statusCode.ToString();
                if (StatusCodeDescriptions.ContainsKey(statusCode))
                {
                    extra["HTTP Status Code Description"] = StatusCodeDescriptions[statusCode];
                }
            }

            // Send to Sentry.io
            if (CTS.CurrentEnvironment != ServerEnvironment.Development)
            {
                await _ravenClient.CaptureAsync(new SentryEvent(message) { Extra = extra });
            }

            return shortId;
        }

        public static Dictionary<int, string> StatusCodeDescriptions = new Dictionary<int, string>()
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

    public interface IErrorLogger
    {
        Task<string> LogErrorAsync(Exception exception, string currentUser = "", string route = "", int statusCode = 0);
        Task<string> LogErrorAsync(string eventDescription, string currentUser, string route, int statusCode = 0);
    }
}

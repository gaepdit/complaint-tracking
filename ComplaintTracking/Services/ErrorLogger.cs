using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;

namespace ComplaintTracking.Services
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly IRaygunAspNetCoreClientProvider _clientProvider;
        private readonly IOptions<RaygunSettings> _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorLogger(
            IRaygunAspNetCoreClientProvider clientProvider,
            IOptions<RaygunSettings> settings,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientProvider = clientProvider;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LogErrorAsync(
            Exception ex,
            string context = "",
            Dictionary<string, object> customData = null)
        {
            string shortId = ShortID.GetShortID();

            // Custom data
            customData = customData ?? new Dictionary<string, object>();
            customData.Add("CTS Error ID", shortId);
            if (!string.IsNullOrEmpty(context))
            {
                customData.Add("Context", context);
            }

            // Send to error logger
            var raygunClient = _clientProvider.GetClient(_settings.Value, _httpContextAccessor.HttpContext);
            await raygunClient.SendInBackground(ex, null, customData);

            return shortId;
        }
    }

    public interface IErrorLogger
    {
        Task<string> LogErrorAsync(Exception exception, string context = "", Dictionary<string, object> customData = null);
    }
}

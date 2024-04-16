using ComplaintTracking.App;
using Mindscape.Raygun4Net.AspNetCore;

namespace ComplaintTracking.Services
{
    public class ErrorLogger : IErrorLogger
    {
        public async Task<string> LogErrorAsync(
            Exception exception,
            string context = "",
            Dictionary<string, object> customData = null)
        {
            var shortId = ShortID.GetShortID();

            // Custom data
            customData ??= [];
            customData.Add("CTS Error ID", shortId);
            if (!string.IsNullOrEmpty(context)) customData.Add("Context", context);

            // Send to error logger
            RaygunSettings raygunSettings = new()
            {
                ApiKey = ApplicationSettings.RaygunSettings.ApiKey,
                ExcludedStatusCodes = ApplicationSettings.RaygunSettings.ExcludedStatusCodes,
                ExcludeErrorsFromLocal = ApplicationSettings.RaygunSettings.ExcludeErrorsFromLocal,
                IgnoreFormFieldNames = ["*Password"]
            };

            var raygunClient = new RaygunClient(raygunSettings);
            await raygunClient.SendInBackground(exception, [CTS.CurrentEnvironment.ToString()], customData);

            return shortId;
        }
    }

    public interface IErrorLogger
    {
        Task<string> LogErrorAsync(Exception exception, string context = "",
            Dictionary<string, object> customData = null);
    }
}

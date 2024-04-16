using Mindscape.Raygun4Net.AspNetCore;

namespace ComplaintTracking.Services
{
    public class ErrorLogger(IServiceProvider serviceProvider) : IErrorLogger
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
            await serviceProvider.GetService<RaygunClient>().SendInBackground(exception, null, customData);

            return shortId;
        }
    }

    public interface IErrorLogger
    {
        Task<string> LogErrorAsync(Exception exception, string context = "",
            Dictionary<string, object> customData = null);
    }
}

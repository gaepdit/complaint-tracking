using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ComplaintTracking.AlertMessages
{
    public static class TempDataDictionaryExtensions
    {
        public static void SaveAlertForSession(this ITempDataDictionary TempData, string message, AlertStatus status = AlertStatus.Information, string title = null)
        {
            TempData["alertMessage"] = message;
            TempData["alertTitle"] = title;
            TempData["alertStatus"] = status.ToString();
        }

        public static AlertViewModel GetAlertFromSession(this ITempDataDictionary TempData)
        {
            if (TempData["alertMessage"] != null)
            {
                return new AlertViewModel(
                    TempData["alertMessage"].ToString(),
                    TempData["alertStatus"]?.ToString() ?? null,
                    TempData["alertTitle"]?.ToString() ?? null);
            }
            else
            {
                return null;
            }
        }
    }
}

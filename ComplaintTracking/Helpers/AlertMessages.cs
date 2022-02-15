using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ComplaintTracking.AlertMessages
{
    public static class TempDataDictionaryExtensions
    {
        public static void SaveAlertForSession(this ITempDataDictionary tempData, string message, AlertStatus status, string title = null)
        {
            tempData["alertMessage"] = message;
            tempData["alertTitle"] = title;
            tempData["alertStatus"] = status.ToString();
        }

        public static AlertViewModel GetAlertFromSession(this ITempDataDictionary tempData)
        {
            if (tempData["alertMessage"] != null)
            {
                return new AlertViewModel(
                    tempData["alertMessage"].ToString(),
                    tempData["alertStatus"]?.ToString(),
                    tempData["alertTitle"]?.ToString());
            }

            return null;
        }
    }
}

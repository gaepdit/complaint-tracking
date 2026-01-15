using Cts.WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace Cts.WebApp.Platform.PageModelHelpers;

public static class TempDataDictionaryExtensions
{
    extension(ITempDataDictionary tempData)
    {
        private void Set<T>(string key, T value) where T : class => tempData[key] = JsonSerializer.Serialize(value);

        private T? Get<T>(string key) where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public void SetDisplayMessage(DisplayMessage.AlertContext context, string message, string detail) =>
            tempData.SetDisplayMessage(context, message, [detail]);

        public void SetDisplayMessage(DisplayMessage.AlertContext context, string message,
            List<string>? details = null) =>
            tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message, details ?? []));

        public DisplayMessage? GetDisplayMessage() => tempData.Get<DisplayMessage>(nameof(DisplayMessage));
    }
}

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MyAppRoot.WebApp.Models;
using System.Text.Json;

namespace MyAppRoot.WebApp.Platform.RazorHelpers;

public static class TempDataExtensions
{
    private static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    private static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out var o);
        return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
    }

    public static void SetDisplayMessage(this ITempDataDictionary tempData, DisplayMessage.AlertContext context,
        string message)
    {
        tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message));
    }

    public static DisplayMessage? GetDisplayMessage(this ITempDataDictionary tempData) =>
        tempData.Get<DisplayMessage>(nameof(DisplayMessage));
}

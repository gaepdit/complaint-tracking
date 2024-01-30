namespace Cts.WebApp.Models;

public class DisplayMessage(DisplayMessage.AlertContext context, string message)
{
    // Context must be public so it works with deserialization in TempDataExtensions class
    // ReSharper disable once MemberCanBePrivate.Global
    public AlertContext Context { get; } = context;
    public string Message { get; } = message;

    public string AlertClass => Context switch
    {
        AlertContext.Primary => "alert-primary",
        AlertContext.Secondary => "alert-secondary",
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Warning => "alert-warning",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };

    public enum AlertContext
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
    }
}

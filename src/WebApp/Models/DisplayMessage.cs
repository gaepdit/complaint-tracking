namespace MyAppRoot.WebApp.Models;

public class DisplayMessage
{
    // ReSharper disable once MemberCanBePrivate.Global
    // Context must be public so it works with deserialization in TempDataExtensions class
    public AlertContext Context { get; }
    public string Message { get; }

    public DisplayMessage(AlertContext context, string message)
    {
        Context = context;
        Message = message;
    }

    public string AlertClass => Context switch
    {
        AlertContext.Primary => "alert-primary",
        AlertContext.Secondary => "alert-secondary",
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };

    public enum AlertContext
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Info,
    }
}

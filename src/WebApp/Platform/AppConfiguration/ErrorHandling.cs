namespace Cts.WebApp.Platform.AppConfiguration;

internal static class ErrorHandling
{
    public static WebApplication UseErrorHandling(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
        else app.UseExceptionHandler("/Error"); // Production or Staging
        return app;
    }
}

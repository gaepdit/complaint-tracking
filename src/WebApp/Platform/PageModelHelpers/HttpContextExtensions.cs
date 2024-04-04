namespace Cts.WebApp.Platform.PageModelHelpers;

public static class HttpContextExtensions
{
    public static string? GetBaseUrl(this PageModel page) =>
        page.Url.Page("/Index", null, null, protocol: "https",
            host: page.HttpContext.Request.Host.ToString());
}

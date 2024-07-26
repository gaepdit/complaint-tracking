using Microsoft.AspNetCore.Rewrite;

namespace Cts.WebApp.Platform.AppConfiguration;

// URL Rewriting Middleware in ASP.NET Core
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-8.0#performance-tips-for-url-rewrite-and-redirect
public static class UrlRedirection
{
    // language=regex
    private const string IntRegex = @"(\d+)";

    // language=regex
    private const string GuidRegex = "([0-9a-fA-F]{8}-(?:[0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12})";

    public static IApplicationBuilder UseUrlRedirection(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Order rewrite rules from the most frequently matched rule to the least frequently matched rule.
        var options = new RewriteOptions()
                // Public pages
                .AddRedirect(regex: "^(?:Public|Complaint)(?:/(?:Attachment(?:/)?)?)?$", replacement: "/",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^Public/ComplaintDetails/{IntRegex}$", replacement: "Complaint/$1",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^Public/Attachment/{GuidRegex}(?:/.*)?$", replacement: "Complaint/Attachment/$1",
                    statusCode: StatusCodes.Status302Found)

                // Staff complaint pages
                .AddRedirect(regex: "^Complaints(?:/)?$", replacement: "Staff/Complaints",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: "^Complaints/Create(?:/)?$", replacement: "Staff/Complaints/Add",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^Complaints/(?:Details|Actions)/{IntRegex}$",
                    replacement: "Staff/Complaints/Details/$1",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^Complaints/Attachment/{GuidRegex}(?:/.*)?$", 
                    replacement: "Staff/Complaints/Attachment/$1",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: "^Staff/Complaints/(?:Details|Attachment)(?:/)?$", replacement: "Staff/Complaints",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: "^Admin(?:/)?$", replacement: "Staff",
                    statusCode: StatusCodes.Status302Found)

                // Complaint Actions pages
                .AddRedirect(regex: "^ComplaintActions(?:/)?$", replacement: "Staff/ComplaintActions",
                    statusCode: StatusCodes.Status302Found)

                // Reporting
                .AddRedirect(regex: "^Reports(?:/.*)?$", replacement: "Admin/Reporting",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: "^Export(?:/.*)?$", replacement: "Admin/Reporting/Export",
                    statusCode: StatusCodes.Status302Found)
            ;

        return app.UseRewriter(options);
    }
}

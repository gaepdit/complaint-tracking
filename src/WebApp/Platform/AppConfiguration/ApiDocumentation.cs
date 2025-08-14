using Cts.WebApp.Platform.Settings;
using Microsoft.OpenApi.Models;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class ApiDocumentation
{
    private const string ApiVersion = "v1";
    private const string ApiTitle = "Complaint Tracking System API";

    public static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddMvcCore().AddApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(ApiVersion, new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiTitle,
                Contact = new OpenApiContact
                {
                    Name = $"{ApiTitle} Support",
                    Email = AppSettings.Support.TechnicalSupportEmail,
                    Url = AppSettings.Support.TechnicalSupportSiteUrl,
                },
            });
        });
    }

    public static void UseApiDocumentation(this IApplicationBuilder app) => app
        .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"{ApiVersion}/openapi.json", $"{ApiTitle} {ApiVersion}");
            options.RoutePrefix = "api-docs";
            options.DocumentTitle = ApiTitle;
        });
}

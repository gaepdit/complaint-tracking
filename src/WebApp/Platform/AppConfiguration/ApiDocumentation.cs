using Cts.WebApp.Platform.Settings;
using Microsoft.OpenApi.Models;

namespace Cts.WebApp.Platform.AppConfiguration;

public static class ApiDocumentation
{
    public static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddMvcCore().AddApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Complaint Tracking System API",
                Contact = new OpenApiContact
                {
                    Name = "Complaint Tracking System Technical Support",
                    Email = AppSettings.SupportSettings.TechnicalSupportEmail,
                },
            });
        });
    }

    public static void UseApiDocumentation(this IApplicationBuilder app) => app
        .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/openapi.json", "Complaint Tracking System API v1");
            options.RoutePrefix = "api-docs";
            options.DocumentTitle = "Complaint Tracking System API";
        });
}

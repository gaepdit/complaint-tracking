using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace ComplaintTracking;

public class ComplaintTrackingWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<ComplaintTrackingWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}

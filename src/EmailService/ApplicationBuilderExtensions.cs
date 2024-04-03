using Microsoft.Extensions.DependencyInjection;

namespace GaEpd.EmailService;

public static class ApplicationBuilderExtensions
{
    public static void AddEmailServices(this IServiceCollection services) =>
        services.AddTransient<IEmailService, EmailService>();
}

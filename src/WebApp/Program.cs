using Cts.AppServices.AutoMapper;
using Cts.AppServices.ServiceRegistration;
using Cts.WebApp.Platform.AppConfiguration;
using Cts.WebApp.Platform.OrgNotifications;
using Cts.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Set the default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure Identity stores.
builder.Services.AddIdentityStores();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add app services.
builder.Services.AddAppServices().AddAutoMapperProfiles().AddEmailService();

// Configure UI services.
builder.Services.AddRazorPages();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure file storage.
await builder.ConfigureFileStorage();

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Add API documentation.
builder.Services.AddApiDocumentation();

// Configure bundling and minification.
builder.AddWebOptimizer();

// Configure Aspire.
builder.AddServiceDefaults();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app
    .UseSecurityHeaders()
    .UseErrorHandling()
    .UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseUrlRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseApiDocumentation();

// Map endpoints.
app.MapDefaultEndpoints();
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();

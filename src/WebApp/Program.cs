using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.AutoMapper;
using Cts.AppServices.IdentityServices;
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

// Persist data protection keys.
builder.Services.AddDataProtection();

builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.AddAuthenticationServices();

// Configure authorization and identity services.
builder.Services.AddAuthorizationPolicies().AddIdentityServices();

// Add app services.
builder.Services.AddAppServices().AddAutoMapperProfiles().AddEmailService();

// Configure UI services.
builder.Services.AddRazorPages();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure file storage
await builder.ConfigureFileStorage();

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Add API documentation.
builder.Services.AddApiDocumentation();

// Configure bundling and minification.
builder.Services.AddWebOptimizer(
    minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizer,
    minifyCss: AppSettings.DevSettings.EnableWebOptimizer);

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

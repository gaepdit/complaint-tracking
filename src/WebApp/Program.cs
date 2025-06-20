using Cts.AppServices.AutoMapper;
using Cts.AppServices.ServiceRegistration;
using Cts.WebApp.Platform.AppConfiguration;
using Cts.WebApp.Platform.Logging;
using Cts.WebApp.Platform.OrgNotifications;
using Cts.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using Microsoft.AspNetCore.DataProtection;
using Mindscape.Raygun4Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Set the default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Bind application settings.
BindingsConfiguration.BindSettings(builder);

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "", "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

// Configure Identity stores.
builder.Services.AddIdentityStores();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add app entity services.
builder.Services.AddAutoMapperProfiles().AddAppServices();

// Configure UI services.
builder.Services.AddRazorPages();

if (!builder.Environment.IsDevelopment())
{
    builder.Services
        .AddHsts(options => options.MaxAge = TimeSpan.FromDays(360))
        .AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect);
}

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure file storage.
await builder.ConfigureFileStorage();

// Add email services.
builder.Services.AddEmailService();

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Add API documentation.
builder.Services.AddApiDocumentation();

// Configure bundling and minification.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(
        minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizerInDev,
        minifyCss: AppSettings.DevSettings.EnableWebOptimizerInDev);
}
else
{
    builder.Services.AddWebOptimizer();
}

// Configure application crash monitoring.
builder.Services.ConfigureErrorLogging(builder.Environment.EnvironmentName);

// Build the application.
var app = builder.Build();

// Configure error handling.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
else app.UseExceptionHandler("/Error"); // Production or Staging

if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) app.UseRaygun();

// Configure security HTTP headers
if (!app.Environment.IsDevelopment() || AppSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

// Configure the application pipeline.
app
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
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();

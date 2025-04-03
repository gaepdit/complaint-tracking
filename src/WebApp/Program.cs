using Cts.AppServices.ErrorLogging;
using Cts.AppServices.RegisterServices;
using Cts.WebApp.Platform.AppConfiguration;
using Cts.WebApp.Platform.Logging;
using Cts.WebApp.Platform.OrgNotifications;
using Cts.WebApp.Platform.Settings;
using GaEpd.EmailService.Utilities;
using GaEpd.FileService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
// ReSharper disable once HeapView.BoxingAllocation
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Bind application settings.
BindingsConfiguration.BindSettings(builder);

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration);

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "", "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

// Configure authorization policies.
builder.Services.AddAuthorizationPolicies();

// Configure UI services.
builder.Services.AddRazorPages();

if (!builder.Environment.IsDevelopment())
{
    builder.Services
        .AddHsts(options => options.MaxAge = TimeSpan.FromDays(360))
        .AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect);
}

// Configure application monitoring.
builder.Services
    .AddTransient<IErrorLogger, ErrorLogger>()
    .AddSingleton(provider =>
    {
        var client = new RaygunClient(provider.GetService<RaygunSettings>()!,
            provider.GetService<IRaygunUserProvider>()!);
        client.SendingMessage += (_, eventArgs) =>
            eventArgs.Message.Details.Tags.Add(builder.Environment.EnvironmentName);
        return client;
    })
    .AddRaygun(opts =>
    {
        opts.ApiKey = AppSettings.RaygunSettings.ApiKey;
        opts.ApplicationVersion = AppSettings.SupportSettings.InformationalVersion;
        opts.ExcludeErrorsFromLocal = AppSettings.RaygunSettings.ExcludeErrorsFromLocal;
        opts.IgnoreFormFieldNames = ["*Password"];
        opts.EnvironmentVariables.Add("ASPNETCORE_*");
    })
    .AddRaygunUserProvider()
    .AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Add app services.
builder.Services
    .AddAutoMapperProfiles()
    .AddAppServices()
    .AddEmailService()
    .AddValidators();

// Add data stores.
builder.Services
    .AddDataPersistence(builder.Configuration)
    .AddFileServices(builder.Configuration);

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Add API documentation.
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

// Configure bundling and minification.
builder.Services.AddWebOptimizer(
    minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizer,
    minifyCss: AppSettings.DevSettings.EnableWebOptimizer);

// Build the application.
var app = builder.Build();

// Configure error handling.
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
else app.UseExceptionHandler("/Error"); // Production or Staging

// Configure security HTTP headers
if (!app.Environment.IsDevelopment() || AppSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) app.UseRaygun();

// Configure the application pipeline.
app
    .UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseUrlRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Configure API documentation.
app
    .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/openapi.json", "Complaint Tracking System API v1");
        options.RoutePrefix = "api-docs";
        options.DocumentTitle = "Complaint Tracking System API";
    });

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();

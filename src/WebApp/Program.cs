using Cts.AppServices.ErrorLogging;
using Cts.AppServices.RegisterServices;
using Cts.WebApp.Platform.AppConfiguration;
using Cts.WebApp.Platform.ErrorLogging;
using Cts.WebApp.Platform.Settings;
using GaEpd.FileService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
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
builder.Services.AddTransient<IClaimsTransformation, AddActiveUserClaimToPrincipal>();

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "", "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

// Configure authorization policies.
builder.Services.AddAuthorizationPolicies();

// Configure UI services.
builder.Services.AddRazorPages();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!builder.Environment.IsDevelopment())
    builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromMinutes(300));

// Configure application monitoring.
builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
builder.Services.AddRaygun(builder.Configuration,
    new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Add app services.
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAppServices();
builder.Services.AddValidators();

// Add data stores.
builder.Services.AddDataPersistence(builder.Configuration);
builder.Services.AddFileServices(builder.Configuration);

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
            Name = "Complaint Tracking System Support",
            Email = AppSettings.SupportSettings.TechnicalSupportEmail,
        },
    });
});

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

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
app.UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Configure API documentation.
app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
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
app.Run();

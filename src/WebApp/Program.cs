using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net.AspNetCore;
using MyAppRoot.AppServices.ServiceCollectionExtensions;
using MyAppRoot.WebApp.Platform.Local;
using MyAppRoot.WebApp.Platform.Raygun;
using MyAppRoot.WebApp.Platform.Services;
using MyAppRoot.WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);
var isLocal = builder.Environment.IsLocalEnv();

// Bind application settings.
builder.Configuration.GetSection(nameof(ApplicationSettings.LocalDevSettings))
    .Bind(ApplicationSettings.LocalDevSettings);
builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings))
    .Bind(ApplicationSettings.RaygunSettings);
var raygunApiKeySet = !string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey);

// Configure Identity.
builder.Services.AddIdentityStores(isLocal);

// Configure cookies (SameSiteMode.None is needed to get single sign-out to work).
builder.Services.Configure<CookiePolicyOptions>(opts => opts.MinimumSameSitePolicy = SameSiteMode.None);

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration, isLocal);

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));
builder.Services.AddAuthorization();

// Configure UI services.
builder.Services.AddRazorPages();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!isLocal) builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromMinutes(300));

// Configure application monitoring.
if (raygunApiKeySet)
{
    builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
    builder.Services.AddRaygun(builder.Configuration,
        new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
    builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial
}

// Add App and data services.
builder.Services.AddAppServices();
builder.Services.AddDataServices(builder.Configuration, isLocal);

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Add API documentation.
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MY_APP_NAME API",
        Contact = new OpenApiContact
        {
            Name = "MY_APP_NAME Support",
            Email = builder.Configuration["SupportEmail"],
        },
    });
});

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

// Build the application.
var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsProduction() && env.IsStaging())
{
    // Production or Staging
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    if (raygunApiKeySet) app.UseRaygun();
}
else
{
    // Development or Local
    app.UseDeveloperExceptionPage();
}

// Configure the application pipeline.
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure API documentation.
app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/openapi.json"; });
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v1/openapi.json", "MY_APP_NAME API v1");
    c.RoutePrefix = "api-docs";
    c.DocumentTitle = "MY_APP_NAME API";
});

// Map endpoints.
app.MapRazorPages();
app.MapControllers(); // This is only needed if an API is implemented. Delete if unused.

// Make it so.
app.Run();

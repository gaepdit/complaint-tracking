using ComplaintTracking.App;
using ComplaintTracking.Data;
using ComplaintTracking.Helpers;
using ComplaintTracking.Models;
using ComplaintTracking.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Mindscape.Raygun4Net.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ComplaintTracking
{
    public class Startup
    {
        internal static bool IsLocal { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Setup();
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Persist data protection keys
            var directory = Directory.CreateDirectory(Configuration["DataProtectionKeysPath"]!);
            var dataProtectionBuilder = services.AddDataProtection().PersistKeysToFileSystem(directory);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                dataProtectionBuilder.ProtectKeysWithDpapi(protectToLocalMachine: true);

            // Bind Application Settings
            Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings)).Bind(ApplicationSettings.RaygunSettings);
            Configuration.GetSection(nameof(ApplicationSettings.ContactEmails)).Bind(ApplicationSettings.ContactEmails);
            Configuration.GetSection(nameof(ApplicationSettings.EmailOptions)).Bind(ApplicationSettings.EmailOptions);

            // Add database context
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    //opts.UseSqlite($"Data Source='{Path.Combine(FilePaths.BasePath, "cts-local.db")}'");
                    opts.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=cts-temp");
                    IsLocal = true;
                }
                else
                {
                    opts.UseSqlServer(connectionString);
                }
            });

            // Add framework services.
            services.AddIdentity<ApplicationUser, IdentityRole>(config => config.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure authorization policy
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            services.AddControllersWithViews(options => { options.Filters.Add(new AuthorizeFilter(policy)); });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login/");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied/");
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            // Add error logging
            services.AddRaygun(Configuration, new RaygunMiddlewareSettings()
            {
                ClientProvider = new RaygunClientProvider()
            });

            // Add sessions
            services.AddSession();

            // Add data access layer service
            services.AddScoped<DAL>();

            // Add application services
            services.AddTransient<IErrorLogger, ErrorLogger>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICtsImageService, CtsImageService>();
            services.AddTransient<ICtsFileService, CtsFileService>();

            // URL/Http Request helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            // Set up database
            services.AddHostedService<MigratorHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("ReSharper", "S125")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // For normal development
                app.UseDeveloperExceptionPage();

                // For testing exception handler
                // app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Error");
            }

            // Configure security HTTP headers
            app.UseSecurityHeaders(policies => policies.AddSecurityHeaderPolicies());

            app.UseRaygun();

            app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");

            // Set current environment
            if (env.IsStaging())
            {
                CTS.CurrentEnvironment = ServerEnvironment.Staging;
            }
            else if (env.IsDevelopment())
            {
                CTS.CurrentEnvironment = ServerEnvironment.Development;
            }
            else
            {
                CTS.CurrentEnvironment = ServerEnvironment.Production;
            }

            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }

        private void Setup()
        {
            // Base path for all generated/uploaded files
            FilePaths.BasePath = Configuration["UserFilesBasePath"];

            // Set file paths
            FilePaths.AttachmentsFolder = Path.Combine(FilePaths.BasePath, "UserFiles", "attachments");
            FilePaths.ExportFolder = Path.Combine(FilePaths.BasePath, "DataExport");
            FilePaths.ThumbnailsFolder = Path.Combine(FilePaths.BasePath, "UserFiles", "thumbnails");
            FilePaths.UnsentEmailFolder = Path.Combine(FilePaths.BasePath, "UnsentEmail");

            // Create Directories
            Directory.CreateDirectory(FilePaths.AttachmentsFolder);
            Directory.CreateDirectory(FilePaths.ExportFolder);
            Directory.CreateDirectory(FilePaths.ThumbnailsFolder);
            Directory.CreateDirectory(FilePaths.UnsentEmailFolder);
        }
    }
}

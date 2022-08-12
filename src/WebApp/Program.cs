using Cts.AppServices.ActionTypes;
using Cts.AppServices.ServiceCollectionExtensions;
using Cts.AppServices.Users;
using Cts.Domain.ActionTypes;
using Cts.Domain.Offices;
using Cts.Domain.Users;
using Cts.LocalRepository;
using Cts.LocalRepository.Identity;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>();

// Configure local Identity
builder.Services.AddLocalIdentity();

// Configure UI
builder.Services.AddRazorPages();

// Uses static data when running locally
builder.Services.AddScoped<IUserService, LocalUserService>();
builder.Services.AddSingleton<IActionTypeRepository, LocalActionTypeRepository>();
builder.Services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();

// Add App Services
builder.Services.AddAppServices();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

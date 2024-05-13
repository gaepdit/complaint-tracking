using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using System.Reflection;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class SupportModel(IAuthorizationService authorization) : PageModel
{
    public bool ActiveUser { get; private set; }
    public string? CurrentVersion { get; private set; }

    public async Task OnGetAsync()
    {
        ActiveUser = await authorization.Succeeded(User, Policies.ActiveUser);
        CurrentVersion = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;
        if (string.IsNullOrEmpty(CurrentVersion))
            CurrentVersion = GetType().Assembly.GetName().Version?.ToString(fieldCount: 3);
    }
}

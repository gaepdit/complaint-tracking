﻿using Cts.AppServices.AuthorizationPolicies;

namespace Cts.WebApp.Pages.Admin.Maintenance;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class MaintenanceIndexModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}

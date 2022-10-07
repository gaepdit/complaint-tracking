namespace MyAppRoot.Domain.Identity;

/// <summary>
/// Authorization Roles for the application.
/// </summary>
public class AppRole
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }

    private AppRole(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
    }

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    /// <summary>
    /// Converts a list of role strings to a list of <see cref="AppRole"/> objects.
    /// </summary>
    /// <param name="roles">A list of role strings.</param>
    /// <returns>A list of AppRoles.</returns>
    public static IList<AppRole> RolesAsAppRoles(IEnumerable<string> roles)
    {
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }

    // Roles
    // These are the strings that are stored in the database. Avoid modifying these once set!

    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string UserAdmin = nameof(UserAdmin);

    // These static Role objects are used for displaying role information in the UI.

    public static AppRole SiteMaintenanceRole { get; } = new(
        SiteMaintenance, "Site Maintenance",
        "Can update values in lookup tables (drop-down lists)."
    );
    
    public static AppRole UserAdminRole { get; } = new(
        UserAdmin, "User Account Admin",
        "Can register and edit all users and roles."
    );
}

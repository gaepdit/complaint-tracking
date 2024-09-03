namespace Cts.Domain.Identity;

/// <summary>
/// User Roles available to the application for authorization.
/// </summary>
public static class RoleName
{
    // These are the strings that are stored in the database. Avoid modifying these once set!

    public const string AttachmentsEditor = nameof(AttachmentsEditor);
    public const string DataExport = nameof(DataExport);
    public const string DivisionManager = nameof(DivisionManager);
    public const string Manager = nameof(Manager);
    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string Staff = nameof(Staff);
    public const string SuperUserAdmin = nameof(SuperUserAdmin);
    public const string UserAdmin = nameof(UserAdmin);
}

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
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
    public static IEnumerable<AppRole> RolesAsAppRoles(IEnumerable<string> roles)
    {
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }

    // These static Role objects are used for displaying role information in the UI.

    [UsedImplicitly]
    public static AppRole StaffRole { get; } = new(
        RoleName.Staff, "Staff",
        "Can create new complaints and work with complaints assigned to them. Can transfer their " +
        "complaints to other users or offices."
    );

    [UsedImplicitly]
    public static AppRole ManagerRole { get; } = new(
        RoleName.Manager, "Manager",
        "Can manage complaints for all users assigned to the same office."
    );

    [UsedImplicitly]
    public static AppRole UserAdminRole { get; } = new(
        RoleName.UserAdmin, "User Account Admin",
        "Can edit all users and roles, excluding the Division Manager role."
    );

    [UsedImplicitly]
    public static AppRole AttachmentsEditorRole { get; } = new(
        RoleName.AttachmentsEditor, "Attachments Editor",
        "Can edit attachments for all complaints, including closed complaints."
    );

    [UsedImplicitly]
    public static AppRole SiteMaintenanceRole { get; } = new(
        RoleName.SiteMaintenance, "Site Maintenance",
        "Can edit offices, assignors, and lookup tables (drop-down lists)."
    );

    [UsedImplicitly]
    public static AppRole DataExportRole { get; } = new(
        RoleName.DataExport, "Data Exporter",
        "Can generate a ZIP archive of complaints."
    );

    [UsedImplicitly]
    public static AppRole DivisionManagerRole { get; } = new(
        RoleName.DivisionManager, "Division Manager",
        "Can manage all complaints. Can delete and restore complaints. " +
        "Can edit all users and roles. Can edit offices, assignors, and lookup tables."
    );

    [UsedImplicitly]
    public static AppRole SuperUserAdminRole { get; } = new(
        RoleName.SuperUserAdmin, "Super-User Account Admin",
        "Can edit all users and roles, including the Division Manager role."
    );
}

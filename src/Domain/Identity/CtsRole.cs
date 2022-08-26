using GaEpd.Library.ListItems;

namespace Cts.Domain.Identity;

/// <summary>
/// Authorization Roles for the application.
/// </summary>
public class CtsRole
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }

    private CtsRole(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name.ToUpperInvariant(), this);
        AllRolesList.Add(new ListItem<string>(name.ToUpperInvariant(), displayName));
    }

    // This declaration must appear before the list of static instance types.
    public static Dictionary<string, CtsRole> AllRoles { get; } = new();
    public static List<ListItem<string>> AllRolesList { get; } = new();

    // Roles
    // These are the strings that are stored in the database. Avoid modifying!
    public const string DivisionManager = nameof(DivisionManager);
    public const string Manager = nameof(Manager);
    public const string UserAdmin = nameof(UserAdmin);
    public const string DataExport = nameof(DataExport);
    public const string AttachmentsEditor = nameof(AttachmentsEditor);

    // These static UserRole objects are used for displaying role information in the UI.
    public static CtsRole DivisionManagerRole { get; } = new(
        DivisionManager, "Division Manager",
        "Can register and edit all users and roles. Can manage all complaints. Can delete and restore " +
        "complaints and complaint actions. Can edit offices, master users, and lookup tables."
    );

    public static CtsRole ManagerRole { get; } = new(
        Manager, "Manager",
        "Can manage complaints for all users assigned to the same office. Can delete and restore " +
        "complaint actions."
    );

    public static CtsRole CtsAdminRole { get; } = new(
        UserAdmin, "User Account Admin",
        "Can register and edit all users and roles, excluding the Division Manager role."
    );

    public static CtsRole DataExportRole { get; } = new(
        DataExport, "Data Export",
        "Can generate a ZIP archive of complaints."
    );

    public static CtsRole AttachmentsEditorRole { get; } = new(
        AttachmentsEditor, "Attachments Editor",
        "Can edit attachments for all complaints, including closed complaints."
    );
}

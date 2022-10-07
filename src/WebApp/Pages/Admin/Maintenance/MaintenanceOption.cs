namespace MyAppRoot.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;

    private MaintenanceOption() { }

    public static MaintenanceOption Office { get; } =
        new() { SingularName = "Office", PluralName = "Offices" };
}

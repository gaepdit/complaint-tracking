namespace Cts.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;

    private MaintenanceOption() { }

    public static MaintenanceOption ActionType { get; } =
        new() { SingularName = "Complaint Action Type", PluralName = "Complaint Action Types" };
}

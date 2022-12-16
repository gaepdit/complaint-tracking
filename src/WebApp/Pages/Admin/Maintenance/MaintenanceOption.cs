namespace Cts.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;

    private MaintenanceOption() { }

    public static MaintenanceOption ActionType { get; } =
        new() { SingularName = "Complaint Action Type", PluralName = "Complaint Action Types" };

    public static MaintenanceOption Concern { get; } =
        new() { SingularName = "Environmental Area of Concern", PluralName = "Environmental Areas of Concern" };

    public static MaintenanceOption Office { get; } =
        new() { SingularName = "Office", PluralName = "Offices" };
}

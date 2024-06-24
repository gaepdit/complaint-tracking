namespace Cts.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; }
    public string PluralName { get; private init; }
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption(string singularName, string pluralName, bool startsWithVowelSound = false)
    {
        SingularName = singularName;
        PluralName = pluralName;
        StartsWithVowelSound = startsWithVowelSound;
    }

    public static MaintenanceOption ActionType { get; } =
        new(singularName: "Complaint Action Type", pluralName: "Complaint Action Types");

    public static MaintenanceOption Concern { get; } =
        new(singularName: "Environmental Area of Concern", pluralName: "Environmental Areas of Concern",
            startsWithVowelSound: true);

    public static MaintenanceOption Office { get; } =
        new(singularName: "Office", pluralName: "Offices and Assignors", startsWithVowelSound: true);
}

namespace ComplaintTracking.App;

internal static class ApplicationSettings
{
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public static ContactEmails ContactEmails { get; } = new();

    public static EmailOptions EmailOptions { get; } = new();
}

internal class RaygunClientSettings
{
    public string ApiKey { get; init; }
}

internal class ContactEmails
{
    // Support contact and return address on system emails
    public string Admin { get; init; }

    // Developer receives error and test emails
    public string Developer { get; init; }

    // Support contact
    public string Support { get; init; }

    // Account administrator
    public string AccountAdmin { get; init; }
}

internal class EmailOptions
{
    public bool EnableEmail { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
}

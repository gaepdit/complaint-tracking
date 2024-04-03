namespace Cts.AppServices.Email;

public class EmailTemplate
{
    public required string Subject { get; init; }
    public required string TextBody { get; init; }
    public required string HtmlBody { get; init; }

    public static readonly EmailTemplate NewUnassignedComplaint = new()
    {
        // {0} = Complaint ID
        // {1} = Complaint URL
        // {2} = Assigned Office
        Subject = "New complaint opened: {0}",
        TextBody = """
                        Complaint ID {0} has been opened for {2}. Please review and assign it as appropriate.
                        {1}
                        """,
        HtmlBody =
            "<p><a href='{1}'>Complaint ID {0}</a> has been opened for {2}. Please review and assign it as appropriate.</p>",
    };

    public static readonly EmailTemplate ComplaintAssigned = new()
    {
        // {0} = Complaint ID
        // {1} = Complaint URL
        Subject = "Complaint opened: {0}",
        TextBody = """
                        You have been assigned Complaint ID {0}:
                        {1}

                        Please review it and take necessary actions.
                        """,
        HtmlBody =
            "<p>You have been assigned <a href='{1}'>Complaint ID {0}</a>.<br />Please review it and take necessary actions.</p> ",
    };
}

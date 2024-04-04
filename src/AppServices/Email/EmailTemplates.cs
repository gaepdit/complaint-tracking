namespace Cts.AppServices.Email;

public class EmailTemplate
{
    // String format arguments:
    // {0} = Complaint ID
    // {1} = Complaint URL
    // {2} = CTS app URL
    // {3} = Assigned Office
    // {4} = Comments

    // Email template properties
    public required string Subject { get; init; }
    public required string TextBody { get; init; }
    public required string HtmlBody { get; init; }

    // Email templates
    public static readonly EmailTemplate UnassignedComplaint = new()
    {
        Subject = "Complaint {0} needs to be assigned",
        TextBody =
            """
            Complaint ID {0} has been opened for {3}. Please review and assign it as appropriate.
            {1}
            """,
        HtmlBody = "<p><a href='{1}'>Complaint ID {0}</a> has been opened for {3}. " +
            "Please review and assign it as appropriate.</p>",
    };

    public static readonly EmailTemplate AssignedComplaint = new()
    {
        Subject = "Complaint {0} assigned to you",
        TextBody =
            """
            You have been assigned Complaint ID {0}. Please review it and take necessary actions.
            {1}
            """,
        HtmlBody = "<p>You have been assigned <a href='{1}'>Complaint ID {0}</a>. " +
            "Please review it and take necessary actions.</p> ",
    };

    public static readonly EmailTemplate ReviewRequested = new()
    {
        Subject = "Complaint {0} review requested",
        TextBody =
            """
            Complaint ID {0} has been submitted for your review and approval. Please review the Complaint and either approve or return it.
            {1}

            Requester’s comments:
            {4}
            """,
        HtmlBody =
            "<p><a href='{1}'>Complaint ID {0}</a> has been submitted for your review and approval. " +
            "Please review the Complaint and either approve or return it.</p>" +
            "<p>Requester’s comments:</p><blockquote style='white-space:pre-line'>{4}</blockquote>",
    };

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email sent from the Complaint Tracking System at
        {2}

        """;

    public const string HtmlSignature = "<p>-- <br>This is an automatically generated email sent from the Complaint " +
        "Tracking System at<br><a href='{2}'>{2}</a></p>";
}

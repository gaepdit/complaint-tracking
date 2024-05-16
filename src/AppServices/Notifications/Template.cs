namespace Cts.AppServices.Notifications;

public class Template
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
    public static readonly Template UnassignedComplaint = new()
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

    public static readonly Template AssignedComplaint = new()
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

    public static readonly Template ReviewRequested = new()
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

    public static readonly Template ReviewApproved = new()
    {
        Subject = "Complaint {0} approved/closed",
        TextBody =
            """
            Complaint ID {0} has been approved/closed.
            {1}

            Reviewer's comments:
            {4}
            """,
        HtmlBody = "<p><a href='{1}'>Complaint ID {0}</a> has been approved/closed.</p>" +
            "<p>Reviewer's comments:</p><blockquote style='white-space:pre-line'>{4}</blockquote>",
    };

    public static readonly Template ReviewReturned = new()
    {
        Subject = "Complaint {0} returned to you",
        TextBody =
            """
            Complaint ID {0} has been returned to you. Please review it and take necessary actions.
            {1}

            Reviewer's comments:
            {4}
            """,
        HtmlBody = "<p><a href='{1}'>Complaint ID {0}</a> has been returned to you. " +
            "Please review it and take necessary actions.</p>" +
            "<p>Reviewer's comments:</p><blockquote style='white-space:pre-line'>{4}</blockquote>",
    };

    public static readonly Template ComplaintReopened = new()
    {
        Subject = "Complaint {0} reopened",
        TextBody =
            """
            Complaint ID {0} has been reopened. Please review it and take necessary actions.
            {1}

            Reviewer's comments:
            {4}
            """,
        HtmlBody = "<p><a href='{1}'>Complaint ID {0}</a> has been reopened. " +
            "Please review it and take necessary actions.</p>" +
            "<p>Reviewer's comments:</p><blockquote style='white-space:pre-line'>{4}</blockquote>",
    };

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email sent from the Complaint Tracking System at
        {2}
        
        Please do not reply to this email. This mailbox is not monitored.
        """;

    public const string HtmlSignature = "<hr><p>This is an automatically generated email sent from the Complaint " +
        "Tracking System at<br><a href='{2}'>{2}</a></p>" +
        "<p>Please do not reply to this email. This mailbox is not monitored.</p>";
}

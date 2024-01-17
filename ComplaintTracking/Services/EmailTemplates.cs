using ComplaintTracking.App;

namespace ComplaintTracking
{
    internal static class EmailTemplates
    {
        internal class Template
        {
            public Template(string subject, string plainBody, string htmlBody = "")
            {
                Subject = subject;
                PlainBody = plainBody;
                HtmlBody = htmlBody;
            }
            public string Subject { get; set; }
            public string PlainBody { get; set; }
            public string HtmlBody { get; set; }
        }

        #region Signature

        // {0} = site URL
        // {1} = recipient email
        public static readonly string PlainSignature =
@"

-- 
This email was sent to {1}
From the Complaint Tracking System: {0}
For support contact: " + ApplicationSettings.ContactEmails.Admin;

        public static readonly string HtmlSignature =
            "<p>-- <br />"
            + "This email was sent to {1}<br/>"
            + "From the Complaint Tracking System: {0}<br />"
            + "For support contact: " + ApplicationSettings.ContactEmails.Admin + "</p>";

        #endregion

        #region Account notification emails

        public static readonly Template ConfirmNewAccount = new Template(
            subject: "Confirm new account",
            // {0} = username
            // {1} = callback URL
            plainBody:
@"An account has been created for you on the Complaint Tracking System.

User name: {0}

Please confirm your account and create a password using this link:
{1}",
            htmlBody: "<p>An account has been created for you on the Complaint Tracking System.<br /></p>"
            + "<p>User name: {0}<br /></p>"
            + "<p>Please confirm your account and create a password using this link:<br />"
            + "<a href='{1}' target='_blank'>Confirm Account</a></p>"
        );

        public static readonly Template ResetPassword = new Template(
            subject: "Reset password",
            // {0} = callback URL
            plainBody:
@"Please reset your password for the Complaint Tracking System using this link:
{0}",
            htmlBody:
            "<p>Please reset your password for the Complaint Tracking System using this link:<br />"
            + "<a href='{0}' target='_blank'>Reset Password</a></p>"
        );

        public static readonly Template NotifyEmailChange = new Template(
            subject: "Account changed",
            // {0} = old email
            // {1} = new email
            // {2} = admin email
            plainBody:
@"The email address for your account on the Complaint Tracking System has changed.

Old email: {0}
New email: {1}

If this is an error, please contact CTS support: {2}",
            htmlBody:
                "<p>The email address for your account on the Complaint Tracking System has changed.<br /></p>"
                + "<p>Old email: {0}<br />"
                + "New email: {1}<br /></p>"
                + "<p>If this is an error, please <a href='mailto:{2}'>contact CTS support</a></p>"
        );

        #endregion

        #region Complaint transition emails

        public static readonly Template ComplaintAssigned = new Template(
            // {0} = Complaint ID
            subject: "Complaint assigned: {0}",

            // {0} = Complaint ID
            // {1} = Complaint URL
            plainBody:
@"You have been assigned Complaint ID {0}:
{1}

Please review it and take necessary actions.",

            htmlBody:
            "<p>You have been assigned <a href='{1}'>Complaint ID {0}</a>.<br />"
            + "Please review it and take necessary actions.</p> "
        );

        public static readonly Template ComplaintReturned = new Template(
            // {0} = Complaint ID
            subject: "Complaint returned: {0}",

            // {0} = Complaint ID
            // {1} = Complaint URL
            // {2} = Comments
            plainBody:
@"Complaint ID {0} has been returned to you. Please review it and take necessary actions.
{1}

Reviewer's comments:
{2}
",
            htmlBody:
            "<p><a href='{1}'>Complaint ID {0}</a> has been returned to you. "
            + "Please review it and take necessary actions.</p>"
            + "<p>Reviewer's comments:</p>"
            + "<blockquote>{2}</blockquote>"
        );

        public static readonly Template ComplaintApproved = new Template(
            // {0} = Complaint ID
            subject: "Complaint approved: {0}",

            // {0} = Complaint ID
            // {1} = Complaint URL
            // {2} = Comments
            plainBody:
@"Complaint ID {0} has been approved/closed.
{1}

Reviewer's comments:
{2}
",
            htmlBody:
            "<p><a href='{1}'>Complaint ID {0}</a> has been approved/closed.</p>"
            + "<p>Reviewer's comments:</p>"
            + "<blockquote>{2}</blockquote>"
        );

        public static readonly Template ComplaintReopened = new Template(
            // {0} = Complaint ID
            subject: "Complaint reopened: {0}",

            // {0} = Complaint ID
            // {1} = Complaint URL
            // {2} = Comments
            plainBody:
@"Complaint ID {0} has been reopened. Please review it and take necessary actions.
{1}

Reviewer's comments:
{2}
",
            htmlBody:
            "<p><a href='{1}'>Complaint ID {0}</a> has been reopened. "
            + "Please review it and take necessary actions.</p>"
            + "<p>Reviewer's comments:</p>"
            + "<blockquote>{2}</blockquote>"
        );

        public static readonly Template ComplaintReviewRequested = new Template(
            // {0} = Complaint ID
            subject: "Complaint review requested: {0}",
            // {0} = Complaint ID
            // {1} = Complaint URL
            // {2} = Comments
            plainBody:
@"Complaint ID {0} has been submitted for approval. Please review the Complaint and either approve or return it.
{1}

Submitter's comments:
{2}
",
            htmlBody:
            "<p><a href='{1}'>Complaint ID {0}</a> has been submitted for approval. "
            + "Please review the actions taken and either approve or return it.</p>"
            + "<p>Submitter's comments:</p>"
            + "<blockquote>{2}</blockquote>"
        );

        public static readonly Template ComplaintOpenedToMaster = new Template(
            // {0} = Complaint ID
            subject: "New complaint opened: {0}",
            // {0} = Complaint ID
            // {1} = Complaint URL
            // {2} = Office assigned
            plainBody:
@"Complaint ID {0} has been opened for {2}. Please review and assign it as appropriate.
{1}",
            htmlBody:
            "<p><a href='{1}'>Complaint ID {0}</a> has been opened for {2}. "
            + "Please review and assign it as appropriate.</p>"
        );

        #endregion

    }
}

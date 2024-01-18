using ComplaintTracking.App;
using ComplaintTracking.Data;
using ComplaintTracking.ExtensionMethods;
using ComplaintTracking.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace ComplaintTracking.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ApplicationDbContext _context;

        public EmailSender(
            IUrlHelper urlHelper,
            ApplicationDbContext context)
        {
            _urlHelper = urlHelper;
            _context = context;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string plainMessage,
            string htmlMessage = "",
            bool saveLocallyOnly = false,
            string replyTo = "")
        {
            var subjectPrefix = CTS.CurrentEnvironment switch
            {
                ServerEnvironment.Development => "[CTS-DEV] ",
                ServerEnvironment.Staging => "[CTS-UAT] ",
                ServerEnvironment.Production => "[CTS] ",
                _ => ""
            };

            var disableEmail = saveLocallyOnly || !ApplicationSettings.EmailOptions.EnableEmail;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Complaint Tracking System", ApplicationSettings.ContactEmails.Admin));
            emailMessage.To.Add(new MailboxAddress("", email));

            if (!string.IsNullOrEmpty(replyTo) && replyTo != email)
            {
                emailMessage.ReplyTo.Add(new MailboxAddress("", replyTo));
            }

            emailMessage.Subject = subjectPrefix + subject;

            var builder = new BodyBuilder();
            var appUrl = _urlHelper.AbsoluteAction("Index", "Home");
            builder.TextBody = plainMessage + string.Format(EmailTemplates.PlainSignature, appUrl, email);

            if (!string.IsNullOrWhiteSpace(htmlMessage))
            {
                builder.HtmlBody = htmlMessage + string.Format(EmailTemplates.HtmlSignature, appUrl, email);
            }

            emailMessage.Body = builder.ToMessageBody();

            if (disableEmail)
            {
                var fileName = $"email_{DateTime.Now:yyyy-MM-dd-HH-mm-ss.FFF}.txt";
                await using var sw = File.CreateText(Path.Combine(FilePaths.UnsentEmailFolder, fileName));
                await emailMessage.WriteToAsync(sw.BaseStream);
            }
            else
            {
                if (CTS.CurrentEnvironment == ServerEnvironment.Development)
                {
                    emailMessage.To.Clear();
                    emailMessage.To.Add(new MailboxAddress("", ApplicationSettings.ContactEmails.Developer));
                }

                using var client = new SmtpClient();
                await client.ConnectAsync(ApplicationSettings.EmailOptions.SmtpHost, ApplicationSettings.EmailOptions.SmtpPort, SecureSocketOptions.None)
                    .ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

            _context.EmailLogs.Add(new EmailLog()
            {
                DateSent = DateTime.Now,
                To = emailMessage.To.ToString(),
                From = emailMessage.From.ToString(),
                Subject = subject,
                TextBody = plainMessage,
                HtmlBody = htmlMessage
            });
            await _context.SaveChangesAsync();
        }
    }

    public interface IEmailSender
    {
        Task SendEmailAsync(
            string email,
            string subject,
            string plainMessage,
            string htmlMessage = "",
            bool saveLocallyOnly = false,
            string replyTo = ""
        );
    }
}

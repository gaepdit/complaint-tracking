using ComplaintTracking.Data;
using ComplaintTracking.ExtensionMethods;
using ComplaintTracking.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ComplaintTracking.Services
{
    public class EmailOptions
    {
        public bool EnableEmail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }

    public class EmailSender : IEmailSender
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EmailSender(
            IUrlHelper urlHelper,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _urlHelper = urlHelper;
            _context = context;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string emailTo,
            string subject,
            string textBody,
            string htmlBody = "",
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

            var emailOptions = new EmailOptions();
            _configuration.GetSection("EmailOptions").Bind(emailOptions);

            var disableEmail = saveLocallyOnly || !emailOptions.EnableEmail;

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Complaint Tracking System", CTS.AdminEmail));
            emailMessage.To.Add(new MailboxAddress("", emailTo));

            if (!string.IsNullOrEmpty(replyTo) && replyTo != emailTo)
            {
                emailMessage.ReplyTo.Add(new MailboxAddress("", replyTo));
            }

            emailMessage.Subject = subjectPrefix + subject;

            var builder = new BodyBuilder();
            var appUrl = _urlHelper.AbsoluteAction("Index", "Home");
            builder.TextBody = textBody + string.Format(EmailTemplates.PlainSignature, appUrl, emailTo);

            if (!string.IsNullOrWhiteSpace(htmlBody))
            {
                builder.HtmlBody = htmlBody + string.Format(EmailTemplates.HtmlSignature, appUrl, emailTo);
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
                    emailMessage.To.Add(new MailboxAddress("", CTS.DevEmail));
                }

                using var client = new SmtpClient();
                await client.ConnectAsync(emailOptions.SmtpHost, emailOptions.SmtpPort, SecureSocketOptions.None)
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
                TextBody = textBody,
                HtmlBody = htmlBody
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

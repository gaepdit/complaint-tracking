using ComplaintTracking.Data;
using ComplaintTracking.ExtensionMethods;
using ComplaintTracking.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ComplaintTracking.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        private readonly ApplicationDbContext _context;

        public EmailSender(
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper,
            ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
            _context = context;
        }

        public async Task SendEmailAsync(
            string emailTo,
            string subject,
            string textBody,
            string htmlBody = "",
            bool saveLocallyOnly = false,
            string replyTo = "")
        {
            string subjectPrefix = "";

            switch (CTS.CurrentEnvironment)
            {
                case ServerEnvironment.Development:
                    subjectPrefix = "[CTS-DEV] ";
                    break;
                case ServerEnvironment.Staging:
                    subjectPrefix = "[CTS-UAT] ";
                    break;
                case ServerEnvironment.Production:
                    subjectPrefix = "[CTS] ";
                    break;
            }

            bool disableEmail = saveLocallyOnly || _httpContextAccessor.HttpContext.Request.IsLocal() || !CTS.EnableEmail;

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
                // ref: https://www.stevejgordon.co.uk/how-to-send-emails-in-asp-net-core-1-0
                // https://www.strathweb.com/2016/04/request-islocal-in-asp-net-core/#comment-3335240646
                var fileName = string.Format("email_{0:yyyy-MM-dd-HH-mm-ss.FFF}.txt", DateTime.Now);
                using StreamWriter sw = File.CreateText(Path.Combine(FilePaths.UnsentEmailFolder, fileName));
                await emailMessage.WriteToAsync(sw.BaseStream);
            }
            else
            {
                if (CTS.CurrentEnvironment == ServerEnvironment.Development)
                {
                    emailMessage.To.Clear();
                    emailMessage.To.Add(new MailboxAddress("", CTS.DevEmail));
                    emailMessage.To.Add(new MailboxAddress("", CTS.QAEmail));
                }

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gets.ga.gov", 25, SecureSocketOptions.None).ConfigureAwait(false);
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

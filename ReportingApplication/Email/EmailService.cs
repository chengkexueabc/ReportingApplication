using System.Net.Mail;
using System.Net;
using System;
using Microsoft.Extensions.Logging;
using ReportingApplication.Job;
using System.IO;
using System.Collections.Generic;
using ReportingApplication.Model;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

namespace ReportingApplication.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public bool SendEmail(Model.Email email, bool enableSsl = true)
        {
            try
            {
                var mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(email.AddFrom);
                mailMessage.To.Add(new MailAddress(email.AddTo));
                mailMessage.Subject = email.Subject;

                mailMessage.Body = email.Body;
                mailMessage.IsBodyHtml = true;

                string attachmentFile = email.AttachmentFile;
                if (attachmentFile != null)
                {
                    mailMessage.Attachments.Add(new Attachment(attachmentFile));
                }

                var client = new SmtpClient();

                client.Host = email.Host;
                client.Port = email.Port;


                if (enableSsl)
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(email.AddFrom, email.Password);
                }

                client.Send(mailMessage);

                _logger.LogInformation("Mail sent successfully at {0}!", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                foreach (Attachment item in mailMessage.Attachments)
                {
                    item.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send mail, error message: {0}!", ex.ToString());

                return false;
            }
        }

    }
}

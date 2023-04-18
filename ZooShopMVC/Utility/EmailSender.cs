using Microsoft.AspNetCore.Identity.UI.Services;
using FluentEmail.Core.Defaults;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Core.Interfaces;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace ZooShopMVC.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailSettings mailSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public Task Execute(string email, string subject, string body)
        {
            mailSettings = _configuration.GetSection("MailSettings").Get<MailSettings>();

            using (SmtpClient client = new SmtpClient()
            {
                Host = mailSettings.Host,
                Port = mailSettings.Port,
                UseDefaultCredentials = false, // This require to be before setting Credentials property
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(mailSettings.MailFrom, mailSettings.MailPass), // you must give a full email address for authentication 
                //TargetName = "STARTTLS/smtp.office365.com", // Set to avoid MustIssueStartTlsFirst exception
                EnableSsl = true // Set to avoid secure connection exception
            })
            {

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(mailSettings.MailFrom), // sender must be a full email address
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,

                };

                message.To.Add(email);
                client.Send(message);
            }

            return Task.CompletedTask;
        }

    }
}

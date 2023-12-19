using Hangfire;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.Email.Models;

namespace UserManagement.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailconfiguration;
        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailconfiguration = emailConfiguration;
        }

        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public void SendEmail(Message message)
        {
            var emailmessage = CreateEmailMessage(message);
            Send(emailmessage);
        }

        private void Send(MimeMessage message)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_emailconfiguration.SmtpServer, _emailconfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailconfiguration.username, _emailconfiguration.password);
                    client.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailmessage = new MimeMessage();
            emailmessage.From.Add(new MailboxAddress("email", _emailconfiguration.From));
            emailmessage.To.AddRange(message.To);
            emailmessage.Subject = message.Subject;
            emailmessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Body
            };
            return emailmessage;

        }
    }
}

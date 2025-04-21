using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using BAL.DTOs.Authentication;
using BAL.Services.Interface;

namespace BAL.Services.Implement
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        private SmtpClient Internal_GetSmtpClient()
        {
            var networkCredentials = new NetworkCredential(
                _emailSettings.Email,
                _emailSettings.Password);

            return new SmtpClient
            {
                Port = _emailSettings.Port,
                Host = _emailSettings.SmtpClient!,
                EnableSsl = _emailSettings.EnableSsl,
                UseDefaultCredentials = _emailSettings.UseDefaultCredentials,
                Credentials = networkCredentials,
            };
        }

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmailAsync(MailDto mail, bool isHtml = false)
        {
            var smtpClient = Internal_GetSmtpClient();

            var message = new MailMessage
            {
                Subject = mail.Subject,
                Body = mail.Body,
                From = new MailAddress(_emailSettings.Email),
                IsBodyHtml = isHtml,
            };

            foreach (var attachment in mail.Attachments)
            {
                message.Attachments.Add(new Attachment(attachment));
            }

            foreach (var email in mail.Receivers)
            {
                message.To.Add(new MailAddress(email));
            }

            await smtpClient.SendMailAsync(message);
        }
    }
}

using NotesApp.Domain.Entities;
using NotesApp.Services.Email;
using NotesApp.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace NotesApp.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var email = CreateEmail(message);
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, true);
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
        }

        private MimeMessage CreateEmail(EmailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.From));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return email;
        }
    }
}

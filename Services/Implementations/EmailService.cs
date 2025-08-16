using MimeKit;
using RakbnyMa_aak.Services.Interfaces;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;


namespace RakbnyMa_aak.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting emailSetting;

        public EmailService(IConfiguration config)
        {
            emailSetting = config.GetSection("EmailSettings").Get<EmailSetting>();
        }

        public async Task SendEmailAsync(string mailTo, string subject,string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(emailSetting.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));
            email.From.Add(new MailboxAddress(emailSetting.DisplayName, emailSetting.Email));

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSetting.Host, emailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailSetting.Email, emailSetting.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
 

    public class EmailSetting
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

using FitnessApp.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using FitnessApp.Domain.Entities;
namespace FitnessApp.DAL.Repositories
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
        {
            _emailSettings = emailSettings.Value;
            _configuration = configuration;
        }
        private readonly IConfiguration _configuration;
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            _emailSettings.SmtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _emailSettings.SmtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _emailSettings.SmtpPassword = _configuration["EmailSettings:SmtpPassword"];
            _emailSettings.SmtpServer = _configuration["EmailSettings:SmtpServer"];

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Limitless Fitness App", _emailSettings.SmtpUsername));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = message
            };

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the error securely (use ILogger)
                throw new ApplicationException("Failed to send email", ex);
            }

        }

        public static string GetMesasge(ApplicationUsers user)
        {
            string emailContent = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Welcome to Limitless Fitness</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            background: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #00274D;
            color: white;
            text-align: center;
            padding: 20px;
        }}
        .header img {{
            width: 50px;
        }}
        .header h1 {{
            margin: 10px 0 0;
            font-size: 22px;
        }}
        .content {{
            padding: 20px;
            text-align: center;
        }}
        .content h2 {{
            color: #00274D;
        }}
        .content p {{
            color: #666;
            font-size: 16px;
            line-height: 1.5;
        }}
        .footer {{
            background-color: #f4f4f4;
            padding: 10px;
            text-align: center;
            font-size: 14px;
            color: #555;
        }}
    </style>
</head>
<body>

<div class=""email-container"">
    <div class=""header"">
        <img src=""~/Photos/Fitness.png"" alt=""Limitless Fitness Logo"">
        <h1>Welcome to Limitless Fitness</h1>
    </div>
    <div class=""content"">
        <h2>Hey {user.Name},</h2>
        <p>Welcome to the Limitless Fitness family! We're excited to help you on your journey to a healthier lifestyle.</p>
        <p>Let's get started with your first workout session as soon as possible!</p>
    </div>
    <div class=""footer"">
        © 2025 Limitless Fitness. All rights reserved.
    </div>
</div>

</body>
</html>";
            return emailContent;
        }
    }
}

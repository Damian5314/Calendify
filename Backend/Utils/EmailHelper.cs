using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace StarterKit.Utils
{
    public static class EmailHelper
    {
        private static readonly string FromEmail;
        private static readonly string FromPassword;
        private static readonly string SmtpServer;
        private static readonly int SmtpPort;

        static EmailHelper()
        {
            try
            {
                // Load email settings from appsettings.json
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                FromEmail = configuration["EmailSettings:FromEmail"] ?? throw new Exception("FromEmail is missing in appsettings.json");
                FromPassword = configuration["EmailSettings:FromPassword"] ?? throw new Exception("FromPassword is missing in appsettings.json");
                SmtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new Exception("SmtpServer is missing in appsettings.json");

                if (!int.TryParse(configuration["EmailSettings:SmtpPort"], out SmtpPort))
                {
                    throw new Exception("SmtpPort is missing or invalid in appsettings.json");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailHelper] Configuration error: {ex.Message}");
                throw;
            }
        }

        public static void SendResetEmail(string toEmail, string token)
        {
            if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("[EmailHelper] Error: Invalid email or token.");
                return;
            }

            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            using var client = new SmtpClient(SmtpServer, SmtpPort)
            {
                Credentials = new NetworkCredential(FromEmail, FromPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(FromEmail, toEmail)
            {
                Subject = "Password Reset Request",
                Body = $"Klik op de volgende link om je wachtwoord te resetten: <a href='{resetLink}'>Reset je wachtwoord</a>",
                IsBodyHtml = true
            };

            try
            {
                client.Send(mail);
                Console.WriteLine($"[EmailHelper] Reset email sent to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailHelper] Error sending email: {ex.Message}");
            }
        }
    }
}

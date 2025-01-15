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
            // Load email settings from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            FromEmail = configuration["EmailSettings:FromEmail"];
            FromPassword = configuration["EmailSettings:FromPassword"];
            SmtpServer = configuration["EmailSettings:SmtpServer"];
            SmtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        }

        public static void SendResetEmail(string toEmail, string token)
        {
            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            using var client = new SmtpClient(SmtpServer, SmtpPort)
            {
                Credentials = new NetworkCredential(FromEmail, FromPassword),
                EnableSsl = true
            };

            var mail = new MailMessage(FromEmail, toEmail)
            {
                Subject = "Password Reset Request",
                Body = $"Klik op de volgende link om je wachtwoord te resetten: {resetLink}",
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
                throw;
            }
        }
    }
}

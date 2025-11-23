using Firmeza.API.Configs;
using Firmeza.API.Data.Entities;
using Firmeza.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace Firmeza.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = AppSettings.Smtp.Host;
        private readonly int _smtpPort = AppSettings.Smtp.Port;
        private readonly string _smtpUser = AppSettings.Smtp.User;
        private readonly string _smtpPassword = AppSettings.Smtp.Password;
        private readonly string _fromAddress = AppSettings.Smtp.From;

        private readonly UserManager<IdentityUser> _userManager;

        public EmailService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public bool SendAccountCreated(IdentityUser user)
        {
            if (user.Email == null) return false;

            string subject = "Your account has been created successfully";
            string body = $"Hello {user.NormalizedUserName},\n" +
                          $"Your account has been created successfully.\n" +
                          $"Thank you.";
            return SendEmail(user.Email, subject, body);
        }

        public bool SendPurcharseConfirmation(String email)
        {

            string subject = "Thanks for your purcharse";
            string body = $"Hello,\n" +
                          $"Your products will arrive soon.\n" +
                          $"\nThank you.";
            return SendEmail(email, subject, body);
        }

        private bool SendEmail(string email, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_fromAddress);
                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}

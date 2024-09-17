using System.Net.Mail;
using System.Net;

namespace Clinic_system.Helpers
{
    public class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = "dentaltest616@gmail.com"; // Sender's Gmail address
            var fromPassword = "zulu xdpq uhyf tjtn";// App password 

            // SMTP configuration
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            // Create the email message
            using (MailMessage message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Email could not be sent.", ex);
                }
            }
        }
    }
}

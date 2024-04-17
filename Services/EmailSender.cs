using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace GBC_TRAVEL_GROUP_88.Services
{
    public class EmailSender : IEmailSender

    {
        private readonly IConfiguration _configuration;


        public EmailSender(IConfiguration configuration) { 
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpClient = new SmtpClient {
                Host = emailSettings["SmtpServer"],
                Port = int.Parse(emailSettings["SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(emailSettings["SmtpUsername"], emailSettings["SmtpPassword"])

            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("101422089@georgebrown.ca"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,

            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);

            
        }
    }
}

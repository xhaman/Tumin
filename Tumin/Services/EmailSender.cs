using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Tumin.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        public EmailSender()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json");
            Configuration = builder.Build();
            
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {


            SmtpClient client = new SmtpClient(Configuration["EmailConfig:SmtpClient"], 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Configuration["EmailConfig:user"], Configuration["EmailConfig:password"]);
        

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Configuration["EmailConfig:user"]);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}

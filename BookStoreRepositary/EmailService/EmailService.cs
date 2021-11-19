using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace BookStoreRepositary.EmailService
{
    public class EmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMailSmtp(string emailTo, string body)
        {
            try
            {
                // Create new instance of mail message
                MailMessage mailMesage = new MailMessage();

                // Add details for the email
                mailMesage.To.Add(emailTo);
                mailMesage.From = new MailAddress(this.configuration["EmailService:Email"]);
                mailMesage.Subject = "Bookstore";
                mailMesage.Body = body;

                // Configure the mail settings
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential(this.configuration["EmailService:Email"], this.configuration["EmailService:Key"]);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mailMesage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

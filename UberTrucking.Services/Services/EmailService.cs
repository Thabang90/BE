using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Services.Models;
using UberTrucking.Services.Services.Interfaces;
using MailKit.Net.Smtp;

namespace UberTrucking.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        public EmailService(IConfiguration config)
        {
            this.config = config;
        }
        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("UB Tracking", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.Receiver, emailModel.Receiver));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = String.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(config["EmailSettings:From"], config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admincore.Common;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace admincore.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        SendgridConfigs _configs;
        public EmailSender(IOptions<SendgridConfigs> options)
        {
            _configs = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            
            var client = new SendGridClient(_configs.APIKey);
            var from = new EmailAddress(_configs.From);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            var response = await client.SendEmailAsync(msg);



        }
    }
}

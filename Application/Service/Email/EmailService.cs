using Application.Contracts;
using Application.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void SendEmail(EmailDTO model)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("lukajosip14@gmail.com"));
            email.To.Add(MailboxAddress.Parse(model.To));
            email.Subject = model.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = model.Body };

            _emailSender.SendEmail(email);
        }
    }
}

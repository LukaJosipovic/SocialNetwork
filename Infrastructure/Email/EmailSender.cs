using Application.Contracts;
using Application.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(MimeMessage email)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("lukajosip14@gmail.com", "SECRET_KEY");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}

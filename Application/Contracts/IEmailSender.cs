using Application.DTO;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IEmailSender
    {
        void SendEmail(MimeMessage email);
    }
}

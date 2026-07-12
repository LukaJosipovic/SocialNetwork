using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class LogoutRequest
    {
        public string? DeviceToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserId { get; set; }
    }
}

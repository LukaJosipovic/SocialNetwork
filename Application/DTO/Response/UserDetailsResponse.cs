using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class UserDetailsResponse : GeneralResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public bool GhostMode { get; set; }
    }
}

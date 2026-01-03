using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserBriefDetailsDTO : GeneralResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureString { get; set; }
    }
}

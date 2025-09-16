using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserBriefDetailsDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureString { get; set; }
    }
}

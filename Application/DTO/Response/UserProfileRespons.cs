using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class UserProfileRespons : GeneralResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string ProfilePictureString { get; set; } = null!;
        public List<PostDTO>? PostDTO { get; set; }
    }
}

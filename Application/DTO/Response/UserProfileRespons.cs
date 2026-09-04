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
        public int FriendCount { get; set; }
        public int MatchCount { get; set; }
        public bool IsFriend { get; set; }
        public List<string>? Activities { get; set; }
    }
}

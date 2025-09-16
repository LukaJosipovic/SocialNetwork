using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public List<string>? Activities { get; set; }
        public bool GhostMode { get; set; }
        public bool DoNotDisturb { get; set; }
        public bool IsBanned { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Match>? Matches { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Activity>? Activity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Post
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public PostType Type { get; set; }
        public string? Content { get; set; }
        public byte[]? PostImage { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Report> Reports { get; set; }
        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Like>? Likes { get; set; }
    }
}

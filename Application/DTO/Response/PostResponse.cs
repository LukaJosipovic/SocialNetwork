using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class PostResponse
    {
        public int Id { get; set; }
        public PostType Type { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}

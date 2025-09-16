using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int PostId { get; set; }

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public Post Post { get; set; } = null!;
    }
}

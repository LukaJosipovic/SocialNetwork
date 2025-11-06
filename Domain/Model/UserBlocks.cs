using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserBlocks
    {
        public int Id { get; set; }
        public string BlockerUserId { get; set; } = null!;
        public ApplicationUser BlockerUser { get; set; } = null!;
        public string BlockedUserId { get; set; } = null!;
        public ApplicationUser BlockedUser { get; set; } = null!;
    }
}

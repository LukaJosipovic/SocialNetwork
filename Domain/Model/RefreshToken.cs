using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; } = DateTime.Now;
        public bool IsRevoked { get; set; }
        //Foreign key
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}

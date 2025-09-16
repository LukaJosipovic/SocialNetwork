using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Match
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string AcceptorId { get; set; }
        public DateTime DateMatched { get; set; }

        // Navigation properties
        public ApplicationUser Creator { get; set; }
        public ApplicationUser Acceptor { get; set; }
        public Activity Activity { get; set; }
    }
}

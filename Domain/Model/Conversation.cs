using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Conversation
    {
        public int Id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public ApplicationUser User1 { get; set; }
        public ApplicationUser User2 { get; set; }
        public DateTime LastMessageAt { get; set; }
        [NotMapped]
        public bool HasUnreadMessages { get; set; }
        public bool IsBlocked { get; set; }
        public ICollection<ChatMessage>? Messages { get; set; }
    }
}

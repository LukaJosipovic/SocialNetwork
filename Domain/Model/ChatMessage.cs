using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        // Navigation properties
        public Conversation Conversation { get; set; }
    }
}

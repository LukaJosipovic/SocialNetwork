using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class MessageResponse : UserBriefDetailsDTO
    {
        public List<MessageDTO> MessagesDto { get; set; } = new();
        public bool ConversationBlocked { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class MessageResponse
    {
        public List<MessageDTO> MessagesDto { get; set; }
        public bool ConversationBlocked { get; set; }
    }
}

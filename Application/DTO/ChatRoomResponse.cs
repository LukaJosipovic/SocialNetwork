using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ChatRoomResponse : GeneralResponse
    {
        public string MyId { get; set; }
        public List<ConversationDTO> ConversationList { get; set; } = new List<ConversationDTO>();
        public int TotalCount { get; set; }
    }
}

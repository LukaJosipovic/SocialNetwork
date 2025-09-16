using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class ChatRoomResponse
    {
        public string MyId { get; set; }
        public List<UserBriefDetailsDTO> UsersChatList { get; set; }
        public int TotalCount { get; set; }
    }
}

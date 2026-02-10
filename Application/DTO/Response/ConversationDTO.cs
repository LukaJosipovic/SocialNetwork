using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class ConversationDTO
    {
        public int ConversationId { get; set; }
        public string UserId { get; set; }
        public string? Name { get; set; }
        public string? ProfilePictureString { get; set; }
        public bool HasUnreadMessages { get; set; }
        public bool IsBlocked { get; set; }
    }
}

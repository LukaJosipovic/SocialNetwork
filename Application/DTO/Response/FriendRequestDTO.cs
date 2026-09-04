using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class FriendRequestDTO : UserBriefDetailsDTO
    {
        public int FriendRequestId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}

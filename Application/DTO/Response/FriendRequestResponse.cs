using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class FriendRequestResponse : GeneralResponse
    {
        public List<FriendRequestDTO> FriendRequestDTOs { get; set; } = new List<FriendRequestDTO>();
    }
}

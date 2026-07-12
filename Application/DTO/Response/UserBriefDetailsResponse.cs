using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class UserBriefDetailsResponse : GeneralResponse
    {
        public List<UserBriefDetailsDTO> UserBriefDetails { get; set; } = new List<UserBriefDetailsDTO>();
    }
}

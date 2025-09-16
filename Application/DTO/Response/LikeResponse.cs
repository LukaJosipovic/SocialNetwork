using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class LikeResponse : GeneralResponse
    {
        public int LikeCount { get; set; }
        public int PostId { get; set; }
    }
}

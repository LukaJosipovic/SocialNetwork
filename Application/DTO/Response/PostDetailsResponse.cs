using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class PostDetailsResponse : GeneralResponse
    {
        public List<PostDetailsDTO> PostDetails { get; set; } = new List<PostDetailsDTO>();
    }
}

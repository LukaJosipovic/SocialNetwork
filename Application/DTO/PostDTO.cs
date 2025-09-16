using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PostDTO
    {
        public int PostID { get; set; }
        public string? ImageUrl { get; set; }
        public byte[]? PostImage { get; set; }
        public string? PostImageString { get; set; }
    }
}

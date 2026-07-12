using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class PostDetailsDTO : GeneralResponse
    {
        public int PostId { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public UserBriefDetailsDTO? User { get; set; }
        public int NumberOfReports { get; set; }
        public byte[]? PostImage { get; set; }
        public string? PostImageString { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
    }
}

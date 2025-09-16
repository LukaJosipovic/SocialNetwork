using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class ActivityResponse
    {
        public string CacheKey { get; set; }
        public string Description { get; set; }
        public string ActivityCategory { get; set; }
        public UserBriefDetailsDTO User { get; set; }
    }
}

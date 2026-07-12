using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class ActivityResponse : GeneralResponse
    {
        public List<ActivityDTO> Activities { get; set; } = new List<ActivityDTO>();
    }
}

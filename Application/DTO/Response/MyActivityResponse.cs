using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class MyActivityResponse : GeneralResponse
    {
        public List<MyActivityDTO> MyActivities { get; set; } = new List<MyActivityDTO>();
    }
}

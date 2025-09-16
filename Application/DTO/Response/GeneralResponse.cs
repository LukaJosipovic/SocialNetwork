using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class GeneralResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}

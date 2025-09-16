using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class RegisterResponse : GeneralResponse
    {
        //public string? Type { get; set; }
        //public string? Title { get; set; }
        //public int Status { get; set; }
        //public Dictionary<string, List<string>> Errors { get; set; } = new();
        public IEnumerable<string>? Errors { get; set; }
    }
}

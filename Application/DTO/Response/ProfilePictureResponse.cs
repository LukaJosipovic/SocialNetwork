using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response
{
    public class ProfilePictureResponse : GeneralResponse
    {
        public byte[]? ImageData { get; set; }
    }
}

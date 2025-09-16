using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class AddUserProfilePictureRequest
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public byte[] ImageData { get; set; }
    }
}

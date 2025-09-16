using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class CreateInformationRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        public string? Description { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

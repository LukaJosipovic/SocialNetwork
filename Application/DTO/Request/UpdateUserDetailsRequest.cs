using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class UpdateUserDetailsRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters")]
        public string Username { get; set; } = null!;
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string? Description { get; set; }
    }
}

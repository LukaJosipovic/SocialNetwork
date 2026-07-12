using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class CreateAccountRequest : LoginUserRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

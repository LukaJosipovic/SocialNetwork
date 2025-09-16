using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string JwtToken { get; set; } = null!;
        [Required]
        public string RefreshToken { get; set; } = null!;
        public string? UserId { get; set; }
    }
}

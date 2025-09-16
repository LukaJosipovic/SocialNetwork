using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Request
{
    public class CreateActivityRequest
    {
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Category { get; set; } = null!;
        public double MyProperty { get; set; }
    }
}

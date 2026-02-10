using Application.Validation;
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
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Category is required")]
        //[Range(1, 10, ErrorMessage = "Category is required")]
        [NotSelectCategory]
        public string Category { get; set; } = null!;
        public int Range { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

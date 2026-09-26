using Application.Enum;
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
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Category is required")]
        //[NotSelectCategory]
        public string Category { get; set; } = ActivityCategory.FoodAndDrink.ToString();
        public int Range { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

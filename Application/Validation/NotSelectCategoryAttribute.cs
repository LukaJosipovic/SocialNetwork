using Application.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    public class NotSelectCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //if (value is string s && s == ActivityCategory.SelectCategory.ToString())
            //{
            //    return new ValidationResult("Please select a valid category");
            //}

            return ValidationResult.Success;
        }
    }
}

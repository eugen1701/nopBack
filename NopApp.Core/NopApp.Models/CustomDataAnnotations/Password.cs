using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NopApp.Models.CustomDataAnnotations
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var password = (string) value;

            if (!Regex.Match(password, "[a-z]").Success) return new ValidationResult("Password must contain at least one lower-case letter");
            if (!Regex.Match(password, "[A-Z]").Success) return new ValidationResult("Password must contain at least one upper-case letter");
            if (!Regex.Match(password, "[0-9]").Success) return new ValidationResult("Password must contain at least one digit");
            if (!Regex.Match(password, "[^a-zA-Z0-9]").Success) return new ValidationResult("Password must contain at least one non-alphanumeric character");

            return ValidationResult.Success;
        }
    }
}

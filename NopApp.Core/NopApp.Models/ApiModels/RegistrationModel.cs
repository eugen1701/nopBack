using NopApp.Models.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class RegistrationModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed in username")]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [Password]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

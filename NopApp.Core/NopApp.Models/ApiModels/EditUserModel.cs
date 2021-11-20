using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class EditUserModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed in username")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string AddressNumber { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Role { get; set; }
    }
}

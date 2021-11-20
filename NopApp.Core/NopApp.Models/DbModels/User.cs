using Microsoft.AspNetCore.Identity;
using NopApp.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class User : IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? AddressNumber { get; set; }
        public Kitchen Kitchen { get; set; }
        public string Status { get; set; }
    }
}

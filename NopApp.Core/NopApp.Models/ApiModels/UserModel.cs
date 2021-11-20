using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string AddressNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    
        public static UserModel CreateFromUser(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Country = user.Country ?? "",
                City = user.City ?? "",
                Street = user.Street ?? "",
                AddressNumber = user.AddressNumber ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Role = "",
                Status = user.Status ?? ""
            };
        }
    }
}

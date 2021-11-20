using NopApp.Models.CustomDataAnnotations;
using NopApp.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace NopApp.Models.ApiModels
{
    public class ManagerRegistrationModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed in username")]
        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [Password]
        public string Password { get; set; }

        public string KitchenName { get; set; }

        public KitchenAddress Address { get; set; }

        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string ContactEmailAddress { get; set; }

        public string AdditionalInformation  { get; set; }

        public static ManagerRegistrationModel CreateFromUser(User user)
        {
            if (user == null) return null;
            KitchenAddress kitchenAddress = new KitchenAddress();
            kitchenAddress.City = user.City;
            kitchenAddress.Country = user.Country;
            kitchenAddress.Number = user.AddressNumber;
            kitchenAddress.Street = user.Street;
            return new ManagerRegistrationModel
            {
                UserName = user.UserName,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                Password = user.PasswordHash ?? "",
                KitchenName = user.Kitchen.Name,
                Address = kitchenAddress,
                PhoneNumber = user.PhoneNumber,
                ContactEmailAddress = user.Email,
                AdditionalInformation = user.Kitchen.AdditionalInformation ?? ""
            };
        }
    }
}

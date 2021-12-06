using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class PublicKitchenModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string AdditionalInformation { get; set; }
        public string DeliveryOpenHour { get; set; }
        public string DeliveryCloseHour { get; set; }

        public static PublicKitchenModel CreateFromKitchen(Kitchen kitchen)
        {
            if (kitchen == null) return null;

            return new PublicKitchenModel
            {
                Id = kitchen.Id,
                Email = kitchen.Email,
                Name = kitchen.Name,
                ContactPhoneNumber = kitchen.ContactPhoneNumber,
                AdditionalInformation = kitchen.AdditionalInformation,
                DeliveryOpenHour = kitchen.DeliveryOpenHour,
                DeliveryCloseHour = kitchen.DeliveryCloseHour
            };
        }
    }
}

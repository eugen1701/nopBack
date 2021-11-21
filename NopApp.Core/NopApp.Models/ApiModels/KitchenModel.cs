using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NopApp.Models.DbModels;

namespace NopApp.Models.ApiModels
{
    public class KitchenModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ManagerId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string AdditionalInformation { get; set; }

        public string DeliveryOpenHour { get; set; }

        public string DeliveryCloseHour { get; set; }

        public static KitchenModel CreateFromKitchen(Kitchen kitchen)
        {
            if (kitchen == null) return null;

            return new KitchenModel
            {
                Id = kitchen.Id,
                ManagerId = kitchen.ManagerId,
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

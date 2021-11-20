using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
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

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Kitchen
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string ContactInfo { get; set; }

        public string DeliveryOpenHour { get; set; }

        public string DeliveryCloseHour { get; set; }

        [ForeignKey("User")]
        public string ManagerId { get; set; }

        public User User { get; set; }
    }
}

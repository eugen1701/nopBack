using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Kitchen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OpeningHours { get; set; }
        public string ContactInfo { get; set; }
        public string DeliveryInterval { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Subscription
    {
        public string Id { get; set; }
        public User User { get; set; }
        public Offer Offer { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

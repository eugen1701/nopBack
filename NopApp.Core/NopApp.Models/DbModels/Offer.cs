using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Offer
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfDays { get; set; }
        public double DailyPrice { get; set; }
        public string KitchenId { get; set; }
        public Kitchen Kitchen { get; set; }
    }
}

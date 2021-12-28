using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Day
    {
        public string Id { get; set; }

        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public Offer Offer { get; set; }

        public int Number { get; set; }

        public List<Meal> Meals { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class Meal
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string KitchenId { get; set; }

        public string Description { get; set; }

        public int Kcal { get; set; }
        public List<Ingredient> Ingredients { get; set; }


    }
}

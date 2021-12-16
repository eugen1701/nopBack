using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{

   

    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Kitchen")]
        public string KitchenId { get; set; }

        public UnitOfMeasure Unit { get; set; }

        public List<MealIngredient> Meals { get; set; }

    }
}

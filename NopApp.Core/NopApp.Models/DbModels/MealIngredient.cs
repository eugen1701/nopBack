using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.DbModels
{
    public class MealIngredient
    {
        [ForeignKey("Meal")]
        public string MealId { get; set; }
        [ForeignKey("Ingredient")]
        public string IngredientId { get; set; }
        public double Quantity { get; set; }

        
    }
}

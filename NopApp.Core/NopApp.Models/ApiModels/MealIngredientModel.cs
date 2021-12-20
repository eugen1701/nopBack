using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class MealIngredientModel
    {
        public string MealId { get; set; }
        [Required]
        public string IngredientId { get; set; }
        public double Quantity { get; set; }

        public static MealIngredientModel CreateFromMealIngredient(MealIngredient ingredient)
        {

            if (ingredient == null) return null;

            return new MealIngredientModel
            {
                MealId = ingredient.MealId,
                IngredientId = ingredient.IngredientId,
                Quantity = ingredient.Quantity
            };
        }
    }
}

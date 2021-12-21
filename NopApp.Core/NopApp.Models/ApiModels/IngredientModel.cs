using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class IngredientModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }

        public string KitchenId { get; set; }

        public UnitOfMeasure Unit { get; set; }

        public List<MealIngredientModel>? Meals { get; set; }

        public static IngredientModel CreateFromIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return null;

            var mealModels = new List<MealIngredientModel>();
            if (ingredient.Meals != null)
            {
                foreach (var mealModel in ingredient.Meals)
                    mealModels.Add(MealIngredientModel.CreateFromMealIngredient(mealModel));
            }
            else
            {
                mealModels = null;
            }




            return new IngredientModel
            {
                Id = ingredient.Id,
                KitchenId = ingredient.KitchenId,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                Meals = mealModels
            };
        }
    }
}

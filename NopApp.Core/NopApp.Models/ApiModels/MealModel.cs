using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class MealModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }

        public string KitchenId { get; set; }

        public string Description { get; set; }

        public int Kcal { get; set; }

        //public List<MealIngredient> Ingredients { get; set; }


        public static MealModel CreateFromMeal(Meal meal)
        {

            if (meal == null) return null;

            return new MealModel
            {
                Id = meal.Id,
                KitchenId = meal.KitchenId,
                Description = meal.Description,
                Kcal = meal.Kcal,
                //Ingredients = meal.Ingredients
                //trebuie facut si pt ingredients 
            };
        }

    }
}

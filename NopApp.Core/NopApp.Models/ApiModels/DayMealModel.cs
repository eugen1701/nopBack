using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class DayMealModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Kcal { get; set; }

        public static DayMealModel CreateFromMeal(Meal meal)
        {
            return new DayMealModel 
            { 
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Kcal = meal.Kcal
            };
        }
    }
}

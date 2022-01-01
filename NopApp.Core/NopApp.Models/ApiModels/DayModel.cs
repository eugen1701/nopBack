using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class DayModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public List<DayMealModel> Meals { get; set; }

        public static DayModel CreateFromDay(Day day)
        {
            if (day == null) return null;

            var newDayModel = new DayModel 
            {
                Id = day.Id,
                Number = day.Number
            };

            if (day.Meals != null)
            {
                newDayModel.Meals = day.Meals.Select(meal => DayMealModel.CreateFromMeal(meal)).ToList();
            }

            return newDayModel;
        }
    }
}

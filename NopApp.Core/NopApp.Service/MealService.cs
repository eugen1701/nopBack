using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service
{  public class MealService
    {
        private MealRepository _mealRepository;

        public MealService(MealRepository mealRepository)
        {
            this._mealRepository = mealRepository;
        }

        public async Task AddMeal(Meal meal)
        {
            await this._mealRepository.AddMeal(meal);
        }

        public async Task<List<Meal>> GetMeals()
        {
            return await this._mealRepository.GetMeals();
        }

        public async Task<Response> EditMeal(MealModel meal)
        {
            if (meal == null) return null;

            var existingMeal = await _mealRepository.GetMealById(meal.Id);
            if (existingMeal == null) throw new Exception("Meal not found");

            if (existingMeal.KitchenId != meal.KitchenId) throw new Exception("Kitchen id not matching!");
          
            existingMeal.Name = meal.Name ?? existingMeal.Name;
            existingMeal.Description = meal.Description ?? existingMeal.Description;
            existingMeal.Kcal = meal.Kcal == 0 ? existingMeal.Kcal:  meal.Kcal;


            var returned = await _mealRepository.Update(existingMeal);
            if (returned == null) throw new Exception("Meal edit failed");

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Meal edited successfully" };
        }

      

        public async Task<List<MealModel>> GetMealsByKitchenId(string id)
        {
            var meals = await _mealRepository.GetMealsByKitchenId(id);

            var mealsmodels = new List<MealModel>();

            foreach( var meal in meals)
            {
                MealModel mealModel = MealModel.CreateFromMeal(meal);
                mealsmodels.Add(mealModel);
            }

            return mealsmodels;
        }

       
    }
}

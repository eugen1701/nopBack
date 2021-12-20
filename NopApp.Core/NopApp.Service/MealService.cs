using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service
{  public class MealService
    {
        private MealRepository _mealRepository;
        private UserRepository _userRepository;

        public MealService(UserRepository userRepository, MealRepository mealRepository)
        {
            this._userRepository = userRepository;
            this._mealRepository = mealRepository;
        }

        public async Task<Response> AddMeal(string managerId, MealModel mealModel)
        {
            User manager = await _userRepository.GetManagerWithKitchen(managerId);
            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen == null) throw new Exception("Manager does not have a kitchen");

            var meal = new Meal
            {
                Id = mealModel.Id,
                Name = mealModel.Name,
                KitchenId = mealModel.KitchenId,
                Description = mealModel.Description,
                Kcal = mealModel.Kcal,
                //Ingredients = mealModel.Ingredients
            };

            var addedMeal = await this._mealRepository.AddMeal(meal);

            if (addedMeal == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Could not add meal" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Meal added successfully" };

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
        public async Task<MealModel> GetMealById(string id)
        {
            var meal = await _mealRepository.GetMealById(id);

            var mealModel = new MealModel();
            mealModel = MealModel.CreateFromMeal(meal);

            return mealModel;
        }
        public async Task<Response> DeleteMeal(string managerId, string mealId)
        {
            var meal = await _mealRepository.GetMealById(mealId);

            if (meal == null) throw new Exception("Delete Meal: Meal not found");

            var manager = await _userRepository.GetManagerWithKitchen(managerId);

            if (manager == null) throw new Exception("Manager not found");

            if (manager.Kitchen.Id != meal.KitchenId) throw new NotAuthorizedException("Manager not authorized to delete this Meal");

            await _mealRepository.DeleteMeal(mealId);

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Meal deleted successfully" };
        }
    }
}

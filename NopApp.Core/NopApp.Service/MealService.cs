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
        private OfferRepository _offerRepository;

        public MealService(UserRepository userRepository, MealRepository mealRepository, OfferRepository offerRepository)
        {
            this._userRepository = userRepository;
            this._mealRepository = mealRepository;
            this._offerRepository = offerRepository;
        }

        public async Task<Response> AddMeal(string managerId, MealModel mealModel)
        {
            User manager = await _userRepository.GetManagerWithKitchen(managerId);
            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen == null) throw new Exception("Manager does not have a kitchen");

            //transformation from MealIngredientModel list to MealIngredient list to add it to database
            var mealIngredients = new List<MealIngredient>();
            foreach (var mealIngredientModel in mealModel.Ingredients)
            {
                MealIngredient ingredient = new MealIngredient
                {
                    MealId = mealIngredientModel.MealId,
                    IngredientId = mealIngredientModel.IngredientId,
                    Quantity = mealIngredientModel.Quantity
                };
                mealIngredients.Add(ingredient);
            }


            var meal = new Meal
            {
                Id = mealModel.Id,
                Name = mealModel.Name,
                KitchenId = mealModel.KitchenId,
                Description = mealModel.Description,
                Kcal = mealModel.Kcal,
                Ingredients = mealIngredients
            };

            var addedMeal = await this._mealRepository.AddMeal(meal);

            if (addedMeal == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Could not add meal" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Meal added successfully" };

        }

        public async Task<List<Meal>> GetMeals(string kitchenId)
        {
            return await this._mealRepository.GetMealsByKitchenId(kitchenId);
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


            var ingredients = new List<MealIngredient>();

            foreach (var ingredient in meal.Ingredients)
            {
                MealIngredient mealIngredient = new MealIngredient();
                mealIngredient.MealId = ingredient.MealId;
                mealIngredient.Quantity = ingredient.Quantity;
                mealIngredient.IngredientId = ingredient.IngredientId;

                ingredients.Add(mealIngredient);
            }

            existingMeal.Ingredients = ingredients;

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

            if (meal == null) throw new Exception("Get meal: Meal not found!");

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

        public async Task<List<DayMealModel>> GetDayMeals(string dayId)
        {
            var day = await _offerRepository.GetDayById(dayId);

            if (day == null) return null;

            var meals = (await _mealRepository.GetMealsByDayId(dayId)).Select(meal => DayMealModel.CreateFromMeal(meal)).ToList();

            return meals;
        }
    }
}

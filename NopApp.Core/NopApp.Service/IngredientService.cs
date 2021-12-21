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
{
    public class IngredientService
    {
        private IngredientRepository _ingredientRepository;
        private UserRepository _userRepository;

        public IngredientService(UserRepository userRepository, IngredientRepository ingredientRepository)
        {
            this._userRepository = userRepository;
            this._ingredientRepository = ingredientRepository;
        }

        public async Task<Response> AddIngredient(string managerId, IngredientModel ingredientModel)
        {
            User manager = await _userRepository.GetManagerWithKitchen(managerId);
            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen == null) throw new Exception("Manager does not have a kitchen");

            var mealIngredients = new List<MealIngredient>();
            foreach (var mealIngredientModel in ingredientModel.Meals)
            {
                MealIngredient meal = new MealIngredient
                {
                    MealId = mealIngredientModel.MealId,
                    IngredientId = mealIngredientModel.IngredientId,
                    Quantity = mealIngredientModel.Quantity
                };
                mealIngredients.Add(meal);
            }

            var ingredient = new Ingredient
            {
                Id = ingredientModel.Id,
                Name = ingredientModel.Name,
                KitchenId = ingredientModel.KitchenId,
                Unit = ingredientModel.Unit
            };

            var addedIngredient = await this._ingredientRepository.AddIngredient(ingredient);

            if (addedIngredient == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Could not add ingredient" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Ingredient added successfully" };

        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await this._ingredientRepository.GetIngredients();
        }

        public async Task<Response> EditIngredient(IngredientModel ingredient)
        {
            if (ingredient == null) return null;

            var existingIngredient = await _ingredientRepository.GetIngredientById(ingredient.Id);
            if (existingIngredient == null) throw new Exception("Ingredient not found");

            if (existingIngredient.KitchenId != ingredient.KitchenId) throw new Exception("Kitchen id not matching!");

            existingIngredient.Name = ingredient.Name ?? existingIngredient.Name;
            //existingIngredient.Kcal = ingredient.Kcal == 0 ? existingIngredient.Kcal : ingredient.Kcal;


            var returned = await _ingredientRepository.Update(existingIngredient);
            if (returned == null) throw new Exception("Ingredient edit failed");

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Ingredient edited successfully" };
        }

        public async Task<List<IngredientModel>> GetIngredientsByKitchenId(string id)
        {
            var ingredients = await _ingredientRepository.GetIngredientsByKitchenId(id);

            var ingredientsmodels = new List<IngredientModel>();

            foreach (var ingredient in ingredients)
            {
                IngredientModel ingredientModel = IngredientModel.CreateFromIngredient(ingredient);
                ingredientsmodels.Add(ingredientModel);
            }

            return ingredientsmodels;
        }
        public async Task<IngredientModel> GetIngredientById(string id)
        {
            var ingredient = await _ingredientRepository.GetIngredientById(id);

            if (ingredient == null) throw new Exception("Get ingredient: Ingredient not found");

            var ingredientModel = new IngredientModel();
            ingredientModel = IngredientModel.CreateFromIngredient(ingredient);

            return ingredientModel;
        }
        public async Task<Response> DeleteIngredient(string managerId, string ingredientId)
        {
            var ingredient = await _ingredientRepository.GetIngredientById(ingredientId);

            if (ingredient == null) throw new Exception("Delete Ingredient: Ingredient not found");

            var manager = await _userRepository.GetManagerWithKitchen(managerId);

            if (manager == null) throw new Exception("Manager not found");

            if (manager.Kitchen.Id != ingredient.KitchenId) throw new NotAuthorizedException("Manager not authorized to delete this Ingredient");

            await _ingredientRepository.DeleteIngredient(ingredientId);

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Ingredient deleted successfully" };
        }
    }
}

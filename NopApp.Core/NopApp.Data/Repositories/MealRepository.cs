using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NopApp.Data;
using NopApp.Models.DbModels;

namespace NopApp.DAL.Repositories
{
    public class MealRepository
    {
        private NopAppContext _dbContext;

        public MealRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Meal> GetMealById(string id)
        {
            return await _dbContext.Meals.FindAsync(id);

        }

        public async Task DeleteMeal(string id)
        {
            Meal meal = await this._dbContext.Meals.FindAsync(id);
            this._dbContext.Remove(meal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Meal>> GetMeals()
        {
            return await _dbContext.Meals.ToListAsync();
        }

        public async Task<List<Meal>> GetMealsByKitchenId(string id)
        {
            List<Meal> meals = await this._dbContext.Meals.Where(meal => meal.KitchenId == id).ToListAsync();
            return  meals;
        }

        public async Task<List<Meal>> GetMealsByDayId(string dayId)
        {
            return await _dbContext.Meals.Include(meal => meal.Days).Where(meal => meal.Days.Any(day => day.Id == dayId)).ToListAsync();
        }

        public async Task<Meal> AddMeal(Meal meal)
        {
            await _dbContext.AddAsync(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }

        public async Task<Meal> Update(Meal updateMeal)
        {
            var mealId = updateMeal.Id;
            var oldMeal = _dbContext.Meals.SingleOrDefaultAsync(meal => meal.Id == mealId);
           
            if (oldMeal == null)
            {
                _dbContext.Entry(oldMeal).CurrentValues.SetValues(updateMeal);
            }
            

            await _dbContext.SaveChangesAsync();
            return updateMeal;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NopApp.Data;
using System.Text;
using System.Threading.Tasks;
using NopApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace NopApp.DAL.Repositories
{
    class IngredientRepository
    {
        private NopAppContext _dbContext;

        public IngredientRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Ingredient> GetIngredientById(int id)
        {
            return await _dbContext.Ingredients.FindAsync(id);

        }

        public async Task DeleteIngredient(int id)
        {
            Ingredient meal = await this._dbContext.Ingredients.FindAsync(id);
            this._dbContext.Remove(meal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _dbContext.Ingredients.ToListAsync();
        }

        public async Task AddIngredients(Ingredient meal)
        {
            await _dbContext.AddAsync(meal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Ingredient updateIngredient)
        {
            var mealId = updateIngredient.Id;
            var oldIngredient = _dbContext.Ingredients.SingleOrDefaultAsync(meal => meal.Id == mealId);
           
            if (oldIngredient == null)
            {
                _dbContext.Entry(oldIngredient).CurrentValues.SetValues(updateIngredient);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}

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
    public class IngredientRepository
    {
        private NopAppContext _dbContext;

        public IngredientRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Ingredient> GetIngredientById(string id)
        {
            return await _dbContext.Ingredients.FindAsync(id);

        }

        public async Task DeleteIngredient(string id)
        {
            Ingredient ingredient = await this._dbContext.Ingredients.FindAsync(id);
            this._dbContext.Remove(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _dbContext.Ingredients.ToListAsync();
        }

        public async Task<Ingredient> AddIngredient(Ingredient ingredient)
        {
            await _dbContext.AddAsync(ingredient);
            await _dbContext.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> Update(Ingredient updateIngredient)
        {
            var ingredientId = updateIngredient.Id;
            var oldIngredient = _dbContext.Ingredients.SingleOrDefaultAsync(ingredient => ingredient.Id == ingredientId);
           
            if (oldIngredient == null)
            {
                _dbContext.Entry(oldIngredient).CurrentValues.SetValues(updateIngredient);
            }

            await _dbContext.SaveChangesAsync();
            return updateIngredient;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NopApp.Data;
using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class KitchenRepository
    {
        private NopAppContext _dbContext;

        public KitchenRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Kitchen> GetKitchenById(string id)
        {
            return await _dbContext.Kitchens.FindAsync(id);

        }

        public async Task DeleteKitchen(string id)
        {
            Kitchen kitchen = await this._dbContext.Kitchens.FindAsync(id);
            this._dbContext.Remove(kitchen);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Kitchen>> GetKitchens()
        {
            return await _dbContext.Kitchens.ToListAsync();
        }

        public async Task AddKitchen(Kitchen kitchen)
        {
            await _dbContext.AddAsync(kitchen);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Kitchen> Update(Kitchen updateKitchen)
        {
            var kitchenId = updateKitchen.Id;
            var oldKitchen = _dbContext.Kitchens.SingleOrDefaultAsync(kitchen => kitchen.Id == kitchenId);
            if (oldKitchen == null)
            {
                _dbContext.Entry(oldKitchen).CurrentValues.SetValues(updateKitchen);
            }
            await _dbContext.SaveChangesAsync();
            return updateKitchen;
        }
    }
}

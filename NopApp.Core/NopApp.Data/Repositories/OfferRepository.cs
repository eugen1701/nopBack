using Microsoft.EntityFrameworkCore;
using NopApp.Data;
using NopApp.Models.DbModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class OfferRepository
    {
        private NopAppContext _dbContext;

        public OfferRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Offer>> GetByKitchenId(string kitchenId)
        {
            return await _dbContext.Offers.Where(offer => offer.KitchenId == kitchenId).ToListAsync();
        }

        public async Task<Offer> GetById(string id)
        {
            return await _dbContext.Offers.FirstOrDefaultAsync(offer => offer.Id == id);
        }

        public async Task<Offer> Insert(Offer offer)
        {
            _dbContext.Update(offer);
            var saveResult = await _dbContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> Delete(Offer offer)
        {
            if (offer == null) return null;

            _dbContext.Remove(offer);
            var saveResult = await _dbContext.SaveChangesAsync();

            return offer;
        }
    }
}

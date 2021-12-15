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

        public async Task<Offer> Add(Offer offer)
        {
            await _dbContext.AddAsync(offer);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult > 0) return offer;

            return null;
        }
    }
}

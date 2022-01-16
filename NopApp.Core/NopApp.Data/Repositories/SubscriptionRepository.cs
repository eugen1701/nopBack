using Microsoft.EntityFrameworkCore;
using NopApp.Data;
using NopApp.Models.DbModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class SubscriptionRepository
    {
        private NopAppContext _dbContext;

        public SubscriptionRepository(NopAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Subscription> AddSubscription(Subscription subscription)
        {
            await _dbContext.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription> GetById(string id)
        {
            return await this._dbContext.Subscriptions.Include(subscription => subscription.Offer).FirstOrDefaultAsync(subscription => subscription.Id == id);
        }

        public async Task<Subscription> GetByIdAndKitchenId(string id, string kitchenId)
        {
            return await this._dbContext.Subscriptions.Include(subscription => subscription.Offer).ThenInclude(offer => offer.Kitchen)
                .Where(subscription => subscription.Offer.KitchenId == kitchenId)
                .Where(subscription => subscription.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Subscription> GetByIdAndUserId(string id, string userId)
        {
            return await this._dbContext.Subscriptions.Include(subscription => subscription.Offer)
                .Where(subscription => subscription.User.Id == userId)
                .Where(subscription => subscription.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Subscription>> GetByUserId(string userId, int? quantity, int? page)
        {
            IQueryable<Subscription> query = _dbContext.Subscriptions.Include(subscription => subscription.Offer).ThenInclude(offer => offer.Kitchen)
                .Where(subscription => subscription.User.Id == userId);
            if (page != null && quantity != null) query = query.Skip((int)quantity * ((int)page - 1));
            if (quantity != null) query = query.Take((int)quantity);

            return await query.ToListAsync();
        }

        public async Task<List<Subscription>> GetByKitchenId(string kitchenId, int? quantity, int? page)
        {
            IQueryable<Subscription> query = this._dbContext.Subscriptions.Include(subscription => subscription.Offer).ThenInclude(offer => offer.Kitchen)
                .Where(subscription => subscription.Offer.KitchenId == kitchenId);
            if (page != null && quantity != null) query = query.Skip((int)quantity * ((int)page - 1));
            if (quantity != null) query = query.Take((int)quantity);

            return await query.ToListAsync();
        }
    }
}

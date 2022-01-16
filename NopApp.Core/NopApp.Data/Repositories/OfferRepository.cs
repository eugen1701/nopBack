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
            return await _dbContext.Offers.Include(offer => offer.Days).Where(offer => offer.KitchenId == kitchenId).ToListAsync();
        }

        public async Task<Offer> GetById(string id)
        {
            return await _dbContext.Offers.Include(offer => offer.Days).FirstOrDefaultAsync(offer => offer.Id == id);
        }

        public async Task<Offer> Insert(Offer offer)
        {
            if (offer.Id == null)
            {
                offer.Days = new List<Day>();
                for (int i = 1; i <= offer.NumberOfDays; i++)
                {
                    offer.Days.Add(new Day { Number = i });
                }
            }

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

        public async Task<Day> GetDayById(string id)
        {
            return await _dbContext.Days.Include(day => day.Offer).SingleOrDefaultAsync(day => day.Id == id);
        }

        public async Task<List<Day>> GetDaysForOffer(string offerId)
        {
            return await _dbContext.Days.Where(day => day.OfferId == offerId).ToListAsync();
        }

        public async Task<Day> ConfigureDay(string dayId, List<string> mealIds)
        {
            var day = await _dbContext.Days.Include(day => day.Meals).SingleOrDefaultAsync(day => day.Id == dayId);

            day.Meals.RemoveAll(meal => !mealIds.Contains(meal.Id));

            var dayMealIds = day.Meals.Select(meal => meal.Id).ToList();

            foreach (string mealId in mealIds)
            {
                if (!dayMealIds.Contains(mealId))
                {
                    var meal = await _dbContext.Meals.SingleOrDefaultAsync(meal => meal.Id == mealId);
                    day.Meals.Add(meal);
                }
            }

            var saveResult = await _dbContext.SaveChangesAsync();

            return day;
        }
    }
}

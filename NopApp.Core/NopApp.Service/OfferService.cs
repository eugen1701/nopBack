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
    public class OfferService
    {
        private UserRepository _userRepository;
        private OfferRepository _offerRepository;
        private MealRepository _mealRepository;

        public OfferService(UserRepository userRepository, OfferRepository offerRepository, MealRepository mealRepository)
        {
            this._offerRepository = offerRepository;
            this._userRepository = userRepository;
            this._mealRepository = mealRepository;
        }

        public async Task<Response> AddOffer(string managerId, OfferModel offerModel)
        {
            User manager = await _userRepository.GetManagerWithKitchen(managerId);
            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen == null) throw new OfferException("Manager does not have a kitchen");

            var offer = new Offer
            {
                Title = offerModel.Title,
                Description = offerModel.Description,
                NumberOfDays = offerModel.NumberOfDays,
                DailyPrice = offerModel.DailyPrice,
                Kitchen = manager.Kitchen
            };

            var addedOffer = await _offerRepository.Insert(offer);

            if (addedOffer == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Could not add offer" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Offer added successfully"};
        }

        public async Task<Response> EditOffer(string managerId, OfferModel offerModel)
        {
            if (offerModel.Id == null) throw new OfferException("Edit offer: id not specified");

            var offer = await _offerRepository.GetById(offerModel.Id);

            if (offer == null) throw new OfferException("Edit offer: offer not found");

            var manager = await _userRepository.GetManagerWithKitchen(managerId);

            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen.Id != offer.KitchenId) throw new NotAuthorizedException("Manager not authorized to edit this offer");

            offer.Id = offerModel.Id;
            offer.Title = offerModel.Title;
            offer.Description = offerModel.Description;
            offer.NumberOfDays = offerModel.NumberOfDays;
            offer.DailyPrice = offerModel.DailyPrice;

            var updatedOffer = await _offerRepository.Insert(offer);

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Offer edited successfully" };
        }

        public async Task<Response> DeleteOffer(string managerId, string offerId)
        {
            var offer = await _offerRepository.GetById(offerId);

            if (offer == null) throw new OfferException("Delete offer: offer not found");

            var manager = await _userRepository.GetManagerWithKitchen(managerId);

            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen.Id != offer.KitchenId) throw new NotAuthorizedException("Manager not authorized to delete this offer");

            var deletedOffer = await _offerRepository.Delete(offer);

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Offer deleted successfully" };
        }

        public async Task<List<OfferModel>> GetOffers(string kitchenId)
        {
            return (await _offerRepository.GetByKitchenId(kitchenId)).Select(offer => OfferModel.CreateFromOffer(offer)).ToList();
        }

        public async Task<OfferModel> GetOffer(string offerId)
        {
            var offer = await _offerRepository.GetById(offerId);

            return OfferModel.CreateFromOffer(offer);
        }

        public async Task<Response> ConfigureDayMeals(string managerId, string offerId, string dayId, DayConfigurationModel dayConfiguration)
        {
            if (dayConfiguration == null) throw new OfferException("Configure day meals: day configuration missing");

            var day = await _offerRepository.GetDayById(dayId);

            if (day == null) throw new OfferException("Configure day meals: day not found");

            var manager = await _userRepository.GetManagerWithKitchen(managerId);

            if (manager == null) throw new UserNotFoundException("Manager not found");

            if (manager.Kitchen.Id != day.Offer.KitchenId || day.OfferId != offerId) throw new NotAuthorizedException("Manager not authorized to edit this day");

            var kitchenMeals = (await _mealRepository.GetMealsByKitchenId(manager.Kitchen.Id)).Select(meal => meal.Id);
            foreach (string mealId in dayConfiguration.Meals)
            {
                if (!kitchenMeals.Contains(mealId)) throw new OfferException("Configure day meals: meal id does not belong to this kitchen");
            }

            await _offerRepository.ConfigureDay(dayId, new List<string>(dayConfiguration.Meals));

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Day configured successfully" };
        }
    }
}

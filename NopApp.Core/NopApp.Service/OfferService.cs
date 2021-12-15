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

        public OfferService(UserRepository userRepository, OfferRepository offerRepository)
        {
            this._offerRepository = offerRepository;
            this._userRepository = userRepository;
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

            var addedOffer = await _offerRepository.Add(offer);

            if (addedOffer == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Could not add offer" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Offer added successfully"};
        }
    }
}

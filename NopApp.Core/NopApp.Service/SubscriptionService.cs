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
    public class SubscriptionService
    {
        private UserRepository _userRepository;
        private OfferRepository _offerRepository;
        private SubscriptionRepository _subscriptionRepository;

        public SubscriptionService(UserRepository userRepository, OfferRepository offerRepository, SubscriptionRepository subscriptionRepository)
        {
            this._offerRepository = offerRepository;
            this._userRepository = userRepository;
            this._subscriptionRepository = subscriptionRepository;
        }

        public async Task<SubscriptionModel> AddSubscription(string userId, SubscriptionModel offerModel)
        {
            User user = await _userRepository.GetUserById(userId);
            if (user == null) throw new UserNotFoundException("User not found");

            Offer offer = await _offerRepository.GetById(offerModel.OfferId);

            if (offer == null) throw new OfferException($"Offer with id {offerModel.OfferId} not found");

            var totalDays = (offerModel.EndDate - offerModel.StarDate).TotalDays;
            var dailyPrice = offer.DailyPrice;
            var totalPrice = totalDays * dailyPrice;

            var subscription = new Subscription
            {
                User = user,
                Offer = offer,
                StarDate = offerModel.StarDate,
                EndDate = offerModel.EndDate,
                Price = totalPrice
            };

            var addedSubscription = await _subscriptionRepository.AddSubscription(subscription);

            if (addedSubscription == null) throw new SubscriptionException("Could not add subscription");

            return SubscriptionModel.CreateFromSubscription(addedSubscription);
        }

        public async Task<SubscriptionModel> GetSubscription(string userId, string id)
        {
            User user = await _userRepository.GetUserByIdWithKitchen(userId);
            if (user == null) throw new UserNotFoundException("User not found");

            var role = await _userRepository.GetUserRoleByUserName(user.UserName);

            if (role == null) throw new Exception($"User {user.Id}, {user.UserName} has no associated role");

            if (RoleEnum.Manager.ToString().Equals(role))
            {
                Subscription subscription = await this._subscriptionRepository.GetByIdAndKitchenId(id, user.Kitchen.Id);
                return SubscriptionModel.CreateFromSubscription(subscription);
            }

            if (RoleEnum.User.ToString().Equals(role))
            {
                Subscription subscription = await this._subscriptionRepository.GetByIdAndUserId(id, userId);
                return SubscriptionModel.CreateFromSubscription(subscription);
            }

            throw new SubscriptionException($"Role {role} not applicable for get subscription by id");
        }

        public async Task<List<SubscriptionModel>> GetSubscriptions(string userId, int? quantity, int? page)
        {
            User user = await _userRepository.GetUserByIdWithKitchen(userId);
            if (user == null) throw new UserNotFoundException("User not found");

            var role = await _userRepository.GetUserRoleByUserName(user.UserName);

            if (role == null) throw new Exception($"User {user.Id}, {user.UserName} has no associated role");

            if (RoleEnum.Manager.ToString().Equals(role))
            {
                List<Subscription> userSubscriptions = await this._subscriptionRepository.GetByKitchenId(user.Kitchen.Id, quantity, page);
                return userSubscriptions.Select(subscription => SubscriptionModel.CreateFromSubscription(subscription)).ToList();
            }
            if (RoleEnum.User.ToString().Equals(role))
            {
                List<Subscription> userSubscriptions = await this._subscriptionRepository.GetByUserId(userId, quantity, page);
                return userSubscriptions.Select(subscription => SubscriptionModel.CreateFromSubscription(subscription)).ToList();
            }
            throw new SubscriptionException($"Role {role} not applicable for listing subscriptions");
        }
    }
}

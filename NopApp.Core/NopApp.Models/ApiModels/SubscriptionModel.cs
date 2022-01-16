using NopApp.Models.DbModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace NopApp.Models.ApiModels
{
    public class SubscriptionModel
    {
        public string Id { get; set; }

        [Required]
        public string OfferId { get; set; }
        [Required]
        public DateTime StarDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public string KitchenName { get; set; }
        public string OfferTitle { get; set; }
        public string OfferDescription { get; set; }

        public static SubscriptionModel CreateFromSubscription(Subscription subscription, string kitchenName)
        {
            if (subscription == null) return null;

            var newSubscriptionModel = new SubscriptionModel
            {
                Id = subscription.Id,
                OfferId = subscription.Offer.Id,
                OfferTitle = subscription.Offer.Title,
                OfferDescription = subscription.Offer.Description,
                StarDate = subscription.StarDate,
                EndDate = subscription.EndDate,
                KitchenName = kitchenName
            };

            return newSubscriptionModel;
        }
    }
}

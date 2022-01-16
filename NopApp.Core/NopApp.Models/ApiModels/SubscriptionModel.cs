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
        public double Price { get; set; }

        public static SubscriptionModel CreateFromSubscription(Subscription subscription)
        {
            if (subscription == null) return null;

            var newSubscriptionModel = new SubscriptionModel
            {
                Id = subscription.Id,
                OfferId = subscription.Offer.Id,
                StarDate = subscription.StarDate,
                EndDate = subscription.EndDate,
                Price = subscription.Price
            };

            return newSubscriptionModel;
        }
    }
}

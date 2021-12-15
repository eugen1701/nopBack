using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class OfferModel
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int NumberOfDays { get; set; }

        [Required]
        public double DailyPrice { get; set; }

        public static OfferModel CreateFromOffer(Offer offer)
        {
            if (offer == null) return null;

            return new OfferModel
            {
                Id = offer.Id,
                Title = offer.Title,
                Description = offer.Description,
                NumberOfDays = offer.NumberOfDays,
                DailyPrice = offer.DailyPrice
            };
        }
    }
}

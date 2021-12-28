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

        public List<string> DayIds { get; set; }

        public static OfferModel CreateFromOffer(Offer offer)
        {
            if (offer == null) return null;

            var newOfferModel = new OfferModel
            {
                Id = offer.Id,
                Title = offer.Title,
                Description = offer.Description,
                NumberOfDays = offer.NumberOfDays,
                DailyPrice = offer.DailyPrice
            };

            if (offer.Days != null)
            {
                newOfferModel.DayIds = offer.Days.OrderBy(day => day.Number).Select(day => day.Id).ToList();
            }

            return newOfferModel;
        }
    }
}

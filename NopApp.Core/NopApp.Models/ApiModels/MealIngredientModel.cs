using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class MealIngredientModel
    {

        [Required]
        public string MealId { get; set; }
        [Required]
        public string IngredientId { get; set; }
        public double Quantity { get; set; }

        
    }
}

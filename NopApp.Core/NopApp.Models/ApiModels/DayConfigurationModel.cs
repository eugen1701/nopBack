using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class DayConfigurationModel
    {
        [Required]
        public List<string> Meals { get; set; }
    }
}

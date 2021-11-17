using NopApp.Models.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Models.ApiModels
{
    public class KitchenAddress
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

    }
}

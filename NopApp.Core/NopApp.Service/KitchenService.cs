using NopApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service
{
    

    public class KitchenService
    {
        private KitchenRepository _kitchenRepository;

        public KitchenService(KitchenRepository kitchenRepository)
        {
            this._kitchenRepository = kitchenRepository;
        }
}
}

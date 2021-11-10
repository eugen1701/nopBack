using NopApp.DAL.Repositories;
using NopApp.Models.DbModels;
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

        public async Task AddKitchen(Kitchen kitchen)
        {
            await this._kitchenRepository.AddKitchen(kitchen);
        }

        public async Task<List<Kitchen>> getKitchens()
        {
            return await this._kitchenRepository.GetKitchens();
        }
}
}

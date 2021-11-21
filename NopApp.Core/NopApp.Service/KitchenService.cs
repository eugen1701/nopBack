using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
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

        public async Task<Response> EditKitchen(KitchenModel kitchen)
        {
            if (kitchen == null) return null;

            var existingKitchen = await _kitchenRepository.GetKitchenById(kitchen.Id);
            if (existingKitchen == null) throw new Exception("Kitchen not found");

            if (existingKitchen.ManagerId != kitchen.ManagerId) throw new Exception("Manager id not matching!");
          
            existingKitchen.Name = kitchen.Name ?? existingKitchen.Name;
            existingKitchen.Email = kitchen.Email ?? existingKitchen.Email;
            existingKitchen.ContactPhoneNumber = kitchen.ContactPhoneNumber ?? existingKitchen.ContactPhoneNumber;
            existingKitchen.AdditionalInformation = kitchen.AdditionalInformation ?? existingKitchen.AdditionalInformation;
            existingKitchen.DeliveryCloseHour = kitchen.DeliveryCloseHour ?? existingKitchen.DeliveryCloseHour;
            existingKitchen.DeliveryOpenHour = kitchen.DeliveryOpenHour ?? existingKitchen.DeliveryOpenHour;
          
            var returned =  await _kitchenRepository.Update(existingKitchen);
            if (returned == null) throw new Exception("Kitchen edit failed");

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Kitchen edited successfully" };
        }

        public async Task<bool> KitchenOwnership(string kitchenId, string managerId)
        {
            var kitchen = await _kitchenRepository.GetKitchenByManagerId(managerId);

            return kitchenId == kitchen.Id;
        }

        public async Task<KitchenModel> GetModelById(string id)
        {
            Kitchen kitchen = await _kitchenRepository.GetKitchenById(id);

            if (kitchen == null) return null;

            KitchenModel kitchenModel = KitchenModel.CreateFromKitchen(kitchen);

            return kitchenModel;
        }

        public async Task<KitchenModel> GetModelByManagerId(string id)
        {
            Kitchen kitchen = await _kitchenRepository.GetKitchenByManagerId(id);

            if (kitchen == null) return null;

            KitchenModel kitchenModel = KitchenModel.CreateFromKitchen(kitchen);

            return kitchenModel;
        }
    }
}


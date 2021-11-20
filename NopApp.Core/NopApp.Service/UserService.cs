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
    public class UserService
    {
        private UserRepository _userRepository;
        private KitchenRepository _kitchenRepository;
        public UserService(UserRepository userRepository, KitchenRepository kitchenRepository)
        {
            this._userRepository = userRepository;
            this._kitchenRepository = kitchenRepository;
        }

        public async Task<UserModel> GetModelById(string id)
        {
            User user = await _userRepository.GetUserById(id);

            if (user == null) return null;

            UserModel userModel = UserModel.CreateFromUser(user);
            userModel.Role = await _userRepository.GetUserRoleByUserName(user.UserName);

            return userModel;
        }

        public async Task<List<ManagerRegistrationModel>> GetPendingRegistrations()
        {
            List<User> allUsers = await _userRepository.GetUsers();
            List<ManagerRegistrationModel> pendingUsers = new List<ManagerRegistrationModel>();
            foreach(User user in allUsers)
            {
                string role = await _userRepository.GetUserRoleByUserName(user.UserName);
                if (user.Status == "Pending" && role == "Manager")
                {
                    Kitchen kitchen = await _kitchenRepository.GetKitchenByManagerId(user.Id);
                    user.Kitchen = kitchen;
                    ManagerRegistrationModel pendingUserRegistration = ManagerRegistrationModel.CreateFromUser(user);
                    pendingUsers.Add(pendingUserRegistration);
                }
            }

            return pendingUsers;
        }
    }
}

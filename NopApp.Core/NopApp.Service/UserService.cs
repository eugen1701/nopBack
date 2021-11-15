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

        public UserService(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UserModel> GetModelById(string id)
        {
            User user = await _userRepository.GetUserById(id);

            if (user == null) return null;

            return UserModel.CreateFromUser(user);
        }
    }
}

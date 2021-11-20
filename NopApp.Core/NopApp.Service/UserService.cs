using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service.CustomExceptions;
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

            UserModel userModel = UserModel.CreateFromUser(user);
            userModel.Role = await _userRepository.GetUserRoleByUserName(user.UserName);

            return userModel;
        }

        public async Task<Response> EditUser(EditUserModel editUserModel)
        {
            if (editUserModel == null) return null;

            var existingUser = await _userRepository.GetUserById(editUserModel.Id);
            if (existingUser == null) throw new UserNotFoundException("User not found");

            var byUserNameResult = await _userRepository.GetUserByUserName(editUserModel.UserName);
            if (byUserNameResult != null && byUserNameResult.Id != existingUser.Id) throw new EditUserException("Username already in use");

            var byEmailResult = await _userRepository.GetUserByEmail(editUserModel.Email);
            if (byEmailResult != null && byEmailResult.Id != existingUser.Id) throw new EditUserException("Email already in use");

            existingUser.UserName = editUserModel.UserName ?? existingUser.UserName;
            existingUser.Email = editUserModel.Email ?? existingUser.Email;
            existingUser.PhoneNumber = editUserModel.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.FirstName = editUserModel.FirstName ?? existingUser.FirstName;
            existingUser.LastName = editUserModel.LastName ?? existingUser.LastName;
            existingUser.Country = editUserModel.Country ?? existingUser.Country;
            existingUser.City = editUserModel.City ?? existingUser.City;
            existingUser.Street = editUserModel.Street ?? existingUser.Street;
            existingUser.AddressNumber = editUserModel.AddressNumber ?? existingUser.AddressNumber;

            var editUserResult = await _userRepository.EditUser(existingUser);
            if (editUserResult == null) throw new EditUserException("User edit failed");

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "User edited successfully" };
        }
    }
}

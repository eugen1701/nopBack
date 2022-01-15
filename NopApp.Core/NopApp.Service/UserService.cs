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
        private KitchenRepository _kitchenRepository;
        private EmailNotificationService _emailNotificationService;

        public UserService(UserRepository userRepository, KitchenRepository kitchenRepository, EmailNotificationService emailNotificationService)
        {
            this._userRepository = userRepository;
            this._kitchenRepository = kitchenRepository;
            this._emailNotificationService = emailNotificationService;
        }

        public async Task<UserModel> GetModelById(string id)
        {
            User user = await _userRepository.GetUserById(id);

            if (user == null) return null;

            UserModel userModel = UserModel.CreateFromUser(user);
            userModel.Role = await _userRepository.GetUserRoleByUserName(user.UserName);

            if (userModel.Role == RoleEnum.Manager.ToString()) userModel.KitchenId = (await _kitchenRepository.GetKitchenByManagerId(user.Id)).Id;

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

        public async Task<User> AcceptRejectManager(string requestId, string acceptance)
        {
            User user = await _userRepository.GetUserById(requestId);
            string role = await _userRepository.GetUserRoleByUserName(user.UserName);
            if (user.Status == "Pending" && role == "Manager" && acceptance == "true")
            {
                User updatedUser = await _userRepository.UpdateUserStatus(user, UserStatusEnum.Accepted);
                _emailNotificationService.SendUserConfirmationEmail(updatedUser);
                return updatedUser;
            }
            else if (user.Status == "Pending" && role == "Manager" && acceptance == "false")
            {
                User updatedUser = await _userRepository.UpdateUserStatus(user, UserStatusEnum.Rejected);
                _emailNotificationService.SendManagerRejectedEmail(updatedUser);
                return updatedUser;
            }
            return null;
        }
    }
}

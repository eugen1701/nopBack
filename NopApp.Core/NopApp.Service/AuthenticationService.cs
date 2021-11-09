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
    public class AuthenticationService
    {
        private UserRepository _userRepository;

        public AuthenticationService(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<Response> RegisterUser(RegistrationModel registrationModel)
        {
            if (await _userRepository.GetUserByUserName(registrationModel.UserName) != null) throw new RegistrationException("Username already in use");
            if (await _userRepository.GetUserByEmail(registrationModel.Email) != null) throw new RegistrationException("Email already in use");

            var newUser = new User{
                UserName = registrationModel.UserName,
                Email = registrationModel.Email
            };

            if (await _userRepository.AddUser(newUser, registrationModel.Password, RoleEnum.User) == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Registration failed" };

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Registration successful" };
        }
    }
}

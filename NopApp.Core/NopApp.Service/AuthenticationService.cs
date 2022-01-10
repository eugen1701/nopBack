﻿using Microsoft.IdentityModel.Tokens;
using NopApp.DAL.Repositories;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service.CustomExceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service
{
    public class AuthenticationService
    {
        private UserRepository _userRepository;
        private KitchenRepository _kitchenRepository;
        private EmailNotificationService _emailNotificationService;

        public AuthenticationService(UserRepository userRepository, KitchenRepository kitchenRepository, EmailNotificationService emailNotificationService)
        {
            this._userRepository = userRepository;
            this._kitchenRepository = kitchenRepository;
            this._emailNotificationService = emailNotificationService;
        }

        public async Task<Response> RegisterUser(RegistrationModel registrationModel)
        {
            if (await _userRepository.GetUserByUserName(registrationModel.UserName) != null) throw new RegistrationException("Username already in use");
            if (await _userRepository.GetUserByEmail(registrationModel.Email) != null) throw new RegistrationException("Email already in use");
            string ConfirmationCode = BuildConfirmationCode();
            var newUser = new User
            {
                UserName = registrationModel.UserName,
                Email = registrationModel.Email,
                EmailConfirmed = false,
                ConfirmationCode = ConfirmationCode
            };

            if (await _userRepository.AddUser(newUser, registrationModel.Password, RoleEnum.User) == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Registration failed" };

            _emailNotificationService.SendUserConfirmationEmail(newUser);

            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Registration successful" };
        }

        public async Task<Response> RegisterManager(ManagerRegistrationModel registrationModel)
        {
            if (await _userRepository.GetUserByUserName(registrationModel.UserName) != null) throw new RegistrationException("Username already in use");
            if (await _userRepository.GetUserByEmail(registrationModel.Email) != null) throw new RegistrationException("Email already in use");

            string ConfirmationCode = BuildConfirmationCode();
            var newUser = new User
            {
                UserName = registrationModel.UserName,
                Email = registrationModel.Email,
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                Status = UserStatusEnum.Pending.ToString(),
                EmailConfirmed = false,
                ConfirmationCode = ConfirmationCode
            };
            KitchenAddress address = registrationModel.Address;
            if (address != null)
            {
                newUser.Country = address.Country;
                newUser.City = address.City;
                newUser.Street = address.Street;
                newUser.AddressNumber = address.Number;
            }

            var kitchen = new Kitchen
            {
                Name = registrationModel.KitchenName,
                Email = registrationModel.ContactEmailAddress,
                ContactPhoneNumber = registrationModel.PhoneNumber,
                AdditionalInformation = registrationModel.AdditionalInformation,
                User = newUser,
                ManagerId = newUser.Id
            };
            newUser.Kitchen = kitchen;
            
            if (await _userRepository.AddUser(newUser, registrationModel.Password, RoleEnum.Manager) == null) return new Response { Status = StatusEnum.Error.ToString(), Message = "Registration failed" };
            
            return new Response { Status = StatusEnum.Ok.ToString(), Message = "Registration successful" };
        }

        public async Task<LoginResponse> Authenticate(string email, string password, string jwtSecret)
        {
            var user = await _userRepository.AuthenticateUserByEmail(email, password);

            if (user == null) return null;

            var role = await _userRepository.GetUserRoleByUserName(user.UserName);

            if (role == null) throw new Exception($"User {user.Id}, {user.UserName} has no associated role");

            if (RoleEnum.Manager.ToString().Equals(role))
            {
                if (UserStatusEnum.Pending.ToString().Equals(user.Status))
                {
                    throw new RegistrationNotAcceptedException($"User {user.UserName} registration request hasn't been accepted yet.");
                }
                if (UserStatusEnum.Rejected.ToString().Equals(user.Status))
                {
                    throw new RegistrationNotAcceptedException($"User {user.UserName} registration request has been rejected.");
                }
            }

            if (!user.EmailConfirmed) throw new RegistrationNotAcceptedException($"Email {user.Email} for user {user.UserName} has not been confirmed yet.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var loginResponse = new LoginResponse()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = role,
                Token = tokenHandler.WriteToken(token)
            };

            return loginResponse;
        }

        public async Task<Response> ConfirmUserEmail(string confirmationCode)
        {
            User user = await _userRepository.GetUserByConfirmationCode(confirmationCode);
            if (user == null) throw new RegistrationException($"Confirmation code {confirmationCode} does not exist.");
            if (user.EmailConfirmed) throw new RegistrationException($"Email {user.Email} has already been confirmed.");
            user.EmailConfirmed = true;
            if (await _userRepository.EditUser(user) == null) throw new RegistrationException($"Failed to confirm email address {user.Email} for user {user.UserName}.");
            return new Response { Status = StatusEnum.Ok.ToString(), Message = $"Successfully confirmed email address {user.Email} for user {user.UserName}." };
        }

        private string BuildConfirmationCode()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}

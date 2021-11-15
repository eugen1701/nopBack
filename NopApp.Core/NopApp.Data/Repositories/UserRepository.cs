using Microsoft.AspNetCore.Identity;
using NopApp.Data;
using NopApp.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.DAL.Repositories
{
    public class UserRepository
    {
        private NopAppContext _dbContext;
        private UserManager<User> _userManager;

        public UserRepository(NopAppContext dbContext, UserManager<User> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await this._userManager.FindByNameAsync(userName);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this._userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserById(string id)
        {
            return await this._userManager.FindByIdAsync(id);
        }

        public async Task<string> GetUserRoleByUserName(string userName)
        {
            var user = await this._userManager.FindByNameAsync(userName);

            return (await this._userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task<User> AuthenticateUserByEmail(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (await _userManager.CheckPasswordAsync(user, password)) return user;

            return null;
        }

        public async Task<User> AddUser(User user, string password, RoleEnum role)
        {
            var addUserResult = await _userManager.CreateAsync(user, password);

            if (!addUserResult.Succeeded) return null;

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role.ToString());

            if (!addToRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return null;
            }

            return user;
        }
    }
}

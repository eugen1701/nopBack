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
    }
}

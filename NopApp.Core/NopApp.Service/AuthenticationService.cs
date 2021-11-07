using NopApp.DAL.Repositories;
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
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NopApp.Models.DbModels;
using NopApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private AuthenticationService _authenticationService;
        private SignInManager<User> _signInManager;

        public AuthenticationController(AuthenticationService authenticationService, SignInManager<User> signInManager)
        {
            this._authenticationService = authenticationService;
            this._signInManager = signInManager;
        }
    }
}

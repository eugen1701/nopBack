﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NopApp.Models.ApiModels;
using NopApp.Service;
using NopApp.WebApi.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private AuthenticationService _authenticationService;
        private UserService _userService;
        private JwtOptions _jwtOptions;

        public UserController(AuthenticationService authenticationService, UserService userService, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._jwtOptions = jwtOptions.Value;
            this._userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(string id)
        {
            var currentUserId = User.Identity.Name;

            if (currentUserId != id && !User.IsInRole("Admin")) return Forbid();

            UserModel userModel = await _userService.GetModelById(id);

            if (userModel == null)
                return Forbid();

            return Ok(userModel);
        }

<<<<<<< HEAD
        [HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PendingRegistrations(string id)
        {
            var currentUserId = User.Identity.Name;
            //if (currentUserId != id && !User.IsInRole("Admin"))
                //return StatusCode(403);

            List<ManagerRegistrationModel> pendingManagers = await _userService.GetPendingRegistrations();
            return Ok(pendingManagers);
        }
    }
=======

       
    

}
>>>>>>> 35cbc4d4791e7d63bf5db77d891a2812fb2e897f
}

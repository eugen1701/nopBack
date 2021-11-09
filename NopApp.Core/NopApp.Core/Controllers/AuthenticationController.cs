using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service;
using NopApp.Service.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
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

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel registrationModel)
        {
            try
            {
                var response = await _authenticationService.RegisterUser(registrationModel);
                return Ok(response);
            }
            catch (RegistrationException ex){
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message});
            }
        }
    }
}

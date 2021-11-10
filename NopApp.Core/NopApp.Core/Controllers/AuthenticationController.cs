using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NopApp.Models.ApiModels;
using NopApp.Models.DbModels;
using NopApp.Service;
using NopApp.Service.CustomExceptions;
using NopApp.WebApi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private AuthenticationService _authenticationService;
        private SignInManager<User> _signInManager;
        private JwtOptions _jwtOptions;

        public AuthenticationController(AuthenticationService authenticationService, SignInManager<User> signInManager, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._signInManager = signInManager;
            this._jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        [Route("/api/Authentication/Register/User")]
        public async Task<IActionResult> RegisterUser(RegistrationModel registrationModel)
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var loginResponse = await _authenticationService.Authenticate(loginModel.Email, loginModel.Password, _jwtOptions.Secret);

            if (loginResponse == null)
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = "Username or password is incorrect" });

            return Ok(loginResponse);
        }
    }
}

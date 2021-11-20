using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NopApp.Models.ApiModels;
using NopApp.Service;
using NopApp.Service.CustomExceptions;
using NopApp.WebApi.Options;
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
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            if (currentUserId != id && !User.IsInRole("Admin")) return StatusCode(403);

            UserModel userModel = await _userService.GetModelById(id);

            if (userModel == null)
                return StatusCode(403);

            return Ok(userModel);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(EditUserModel editUserModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            if (currentUserId != editUserModel.Id) return StatusCode(403);

            try
            {
                var response = await _userService.EditUser(editUserModel);

                return Ok(response);
            }
            catch(EditUserException ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
            catch(UserNotFoundException ex)
            {
                return StatusCode(403);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NopApp.Models.ApiModels;

using NopApp.Service;
using NopApp.Service.CustomExceptions;
using NopApp.WebApi.Options;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KitchenController : Controller
    {

        private KitchenService _kitchenService;
        private AuthenticationService _authenticationService;
        private JwtOptions _jwtOptions;
        public KitchenController(AuthenticationService authenticationService, KitchenService userService, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._jwtOptions = jwtOptions.Value;
            this._kitchenService = userService;
        }



        [HttpPut]
        public async Task<IActionResult> Edit(KitchenModel editKitchenModel)
        {
            /*var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            if (currentUserId != editKitchenModel.ManagerId)
                return Forbid();*/
            try
            {
                var response = await _kitchenService.EditKitchen(editKitchenModel);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }
    }
}

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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Edit(KitchenModel editKitchenModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            if (currentUserId != editKitchenModel.ManagerId)
                return StatusCode(403);

            if (!await _kitchenService.KitchenOwnership(editKitchenModel.Id, currentUserId))
                return StatusCode(403);

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

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(string id)
        {
            var kitchenModel = await _kitchenService.GetModelById(id);

            if (kitchenModel == null) return StatusCode(404);

            return Ok(kitchenModel);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? quantity, int? page)
        {
            var kitchens = await _kitchenService.GetKitchens(quantity, page);

            return Ok(kitchens);
        }
    }
}

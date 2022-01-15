using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NopApp.Models.ApiModels;
using NopApp.Service;
using NopApp.Service.CustomExceptions;
using NopApp.WebApi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IngredientController : Controller
    {
        private IngredientService _ingredientService;
        private KitchenService _kitchenService;
        private AuthenticationService _authenticationService;
        private JwtOptions _jwtOptions;

        public IngredientController(AuthenticationService authenticationService, IngredientService ingredientService, KitchenService kitchenService, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._jwtOptions = jwtOptions.Value;
            this._ingredientService = ingredientService;
            this._kitchenService = kitchenService;
        }

       

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get()
        {

            var currentUserId = User.Identity.Name;


            var ingredients = await _ingredientService.GetIngredientsByUserId(currentUserId);

            return Ok(ingredients);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetIngredient(string id)
        {
            try
            {
                var ingredient = await _ingredientService.GetIngredientById(id);
                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = User.Identity.Name;

            try
            {
                var response = await _ingredientService.DeleteIngredient(currentUserId, id);

                return response.Status == StatusEnum.Ok.ToString() ? Ok(response) : BadRequest(response);
            }
            catch (NotAuthorizedException ex)
            {
                return StatusCode(403);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Create(IngredientModel ingredientModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            try
            {
                var response = await _ingredientService.AddIngredient(currentUserId, ingredientModel);

                return response.Status == StatusEnum.Ok.ToString() ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Edit(IngredientModel editIngredientModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user
            var kitchenModel = await _kitchenService.GetModelById(editIngredientModel.KitchenId);

            if (currentUserId != kitchenModel.ManagerId)
                return StatusCode(403);

            if (!await _kitchenService.KitchenOwnership(editIngredientModel.KitchenId, currentUserId))
                return StatusCode(403);

            try
            {
                var response = await _ingredientService.EditIngredient(editIngredientModel);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }
    }
}

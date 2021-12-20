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
    public class MealController : ControllerBase
    {
        private MealService _mealService;
        private KitchenService _kitchenService;
        private AuthenticationService _authenticationService;
        private JwtOptions _jwtOptions;
        public MealController(AuthenticationService authenticationService, MealService mealService, KitchenService kitchenService, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._jwtOptions = jwtOptions.Value;
            this._mealService = mealService;
            this._kitchenService = kitchenService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int? quantity, int? page)
        {
            var meals = await _mealService.GetMeals();

            return Ok(meals);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMeal(string id)
        {
            var meal = await _mealService.GetMealById(id);

            return Ok(meal);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = User.Identity.Name;

            try
            {
                var response = await _mealService.DeleteMeal(currentUserId, id);

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
        public async Task<IActionResult> Create(MealModel mealModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            try
            {
                var response = await _mealService.AddMeal(currentUserId, mealModel);

                return response.Status == StatusEnum.Ok.ToString() ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Edit(MealModel editMealModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user
            var kitchenModel = await _kitchenService.GetModelById(editMealModel.KitchenId);

            if (currentUserId != kitchenModel.ManagerId)
                return StatusCode(403);

            if (!await _kitchenService.KitchenOwnership(editMealModel.KitchenId, currentUserId))
                return StatusCode(403);

            try
            {
                var response = await _mealService.EditMeal(editMealModel);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NopApp.Service;
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
        private AuthenticationService _authenticationService;
        private JwtOptions _jwtOptions;
        public MealController(AuthenticationService authenticationService, MealService mealService, IOptions<JwtOptions> jwtOptions)
        {
            this._authenticationService = authenticationService;
            this._jwtOptions = jwtOptions.Value;
            this._mealService = mealService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int? quantity, int? page)
        {
            var meals = await _mealService.GetMeals();

            return Ok(meals);
        }
    }
}

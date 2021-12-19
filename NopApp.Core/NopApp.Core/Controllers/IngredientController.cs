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
    public class IngredientController : ControllerBase
    {
        private IngredientService ingredientService;
    }
}

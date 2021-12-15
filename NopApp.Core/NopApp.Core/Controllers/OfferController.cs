using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using NopApp.Service;
using System.Threading.Tasks;
using NopApp.Models.ApiModels;
using Microsoft.AspNetCore.Authorization;
using NopApp.Service.CustomExceptions;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : Controller
    {
        private OfferService _offerService;

        public OfferController(OfferService offerService)
        {
            this._offerService = offerService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Create(OfferModel offerModel)
        {
            var currentUserId = User.Identity.Name; // User.Identity.Name is actually the id of the user

            try
            {
                var response = await _offerService.AddOffer(currentUserId, offerModel);

                return response.Status == StatusEnum.Ok.ToString() ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response { Status = StatusEnum.Error.ToString(), Message = ex.Message});
            }
            
        }
    }
}

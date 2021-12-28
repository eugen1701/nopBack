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

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> Edit(string id, OfferModel offerModel)
        {
            var currentUserId = User.Identity.Name;

            offerModel.Id ??= id;

            try
            {
                var response = await _offerService.EditOffer(currentUserId, offerModel);

                return response.Status == StatusEnum.Ok.ToString() ? Ok(response) : BadRequest(response);
            }
            catch (NotAuthorizedException ex) {
                return StatusCode(403);
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
                var response = await _offerService.DeleteOffer(currentUserId, id);

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


        [HttpGet]
        [Route("Kitchen/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOffers(string id)
        {
            var offers = await _offerService.GetOffers(id);

            return Ok(offers);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOffer(string id)
        {
            var offer = await _offerService.GetOffer(id);

            return offer != null ? Ok(offer) : StatusCode(404);
        }

        [HttpPut]
        [Route("{offerId}/day/{dayId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> ConfigureDayMeals(string offerId, string dayId, DayConfigurationModel dayConfiguration)
        {
            var currentUserId = User.Identity.Name;

            try
            {
                var response = await _offerService.ConfigureDayMeals(currentUserId, offerId, dayId, dayConfiguration);

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
    }
}

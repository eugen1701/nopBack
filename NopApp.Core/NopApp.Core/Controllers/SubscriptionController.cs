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
    public class SubscriptionController : Controller
    {
        private SubscriptionService _subscriptionService;

        public SubscriptionController(SubscriptionService subscriptionService)
        {
            this._subscriptionService = subscriptionService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> CreateSubscription(SubscriptionModel subscriptionModel)
        {
            var currentUserId = User.Identity.Name;

            try
            {
                return Ok(await _subscriptionService.AddSubscription(currentUserId, subscriptionModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetSubscription(string id)
        {
            var currentUserId = User.Identity.Name;
            try
            {
                var subscription = await _subscriptionService.GetSubscription(currentUserId, id);

                return subscription != null ? Ok(subscription) : StatusCode(404);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetSubscriptions(int? quantity, int? page)
        {
            var currentUserId = User.Identity.Name;
            try
            {
                var subscription = await _subscriptionService.GetSubscriptions(currentUserId, quantity, page);

                return subscription != null ? Ok(subscription) : StatusCode(404);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

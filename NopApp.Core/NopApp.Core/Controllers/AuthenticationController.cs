using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NopApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }
    }
}

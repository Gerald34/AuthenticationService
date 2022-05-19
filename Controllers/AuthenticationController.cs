using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Requests;
using AuthenticationService.Services;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private UserService _userService;
        public AuthenticationController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public IActionResult AuthenticationProcess(AuthenticateRequest authenticateRequest)
        {
            dynamic response = _userService.Authenticate(authenticateRequest);
            if (response.error) return BadRequest(response);

            if (response.data.active == 0)
                return BadRequest(new { error = true, message = "Account not activated, please verify account to continue." });

            return Ok(response);
        }

    }
}


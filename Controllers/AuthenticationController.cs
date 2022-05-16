using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Requests;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using AuthenticationService.Entities;

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
        public IActionResult post(AuthenticateRequest authenticateRequest)
        {
            var response = _userService.Authenticate(authenticateRequest);
            if (response.error)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Username or password is incorrect."
                });
            }

            if (response.data.active == 0)
            {
                return NotFound(new { error = true, message = "Account not activated, please verify account to continue." });
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public dynamic CreateAccount(UserEntity user)
        {
            dynamic process = _userService.CreateAccount(user);

            return (process.error) ? BadRequest(process) : Ok(process);
        }

        /// <summary>
        /// Get all admin users
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllAdminUsers()
        {
            var users = _userService.GetAll();
            if (users == null)
            {
                var errorResponse = new
                {
                    error = true,
                    message = "No users found"
                };

                return BadRequest(errorResponse);
            }

            dynamic response = new
            {
                error = false,
                message = "User collection found.",
                totalCount = users.Count(),
                data = users
            };

            return await Ok(response);
        }

        /// <summary>
        /// Verify account
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("verification")]
        public dynamic VerifyAccount(Guid userID, string username)
        {
            return _userService.ActivateAccount(userID, username);
        }

    }
}


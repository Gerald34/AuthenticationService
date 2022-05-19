using AuthenticationService.DTO;
using AuthenticationService.Entities;
using AuthenticationService.Requests;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("operations")]
    public class OperationsController : ControllerBase
    {
        private UserService _userService;
        public OperationsController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public dynamic HiThere()
        {
            return "Gerald";
        }

        /// <summary>
        /// Create new user account
        /// Manager role only
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public dynamic CreateAccount(UserEntity user)
        {
            dynamic process = _userService.CreateAccount(user);
            return (process.error) ? BadRequest(process) : Ok(process);
        }

        /// <summary>
        /// Get all admin users
        /// </summary>
        /// <returns></returns>\
        [HttpGet("fetch-all-users")]
        public dynamic GetAllUsers([FromQuery] UserListQueryRequest queryRequest)
        {
            IEnumerable<AuthenticateDTO> collection = _userService.GetAll();

            if (collection.Count() <= 0) return NoContent();

            return Ok(new
            {
                error = false,
                message = "User collection found.",
                totalCount = collection.Count(),
                data = collection
            });
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

        /// <summary>
        /// Deactivate account
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPost("deactivate"), Authorize]
        public async Task<IActionResult> DeactivateAccount(Guid userID, string reason)
        {
            dynamic process = await _userService.Deactivate(userID, reason);
            return (process.error) ? BadRequest(process) : Ok(process);
        }

        /// <summary>
        /// Suspend account
        /// </summary>
        /// <param name="statusRequest"></param>
        /// <returns></returns>
        [HttpPost("suspend"), Authorize]
        public async Task<IActionResult> SuspendAccount(StatusRequest statusRequest)
        {
            dynamic process = await _userService.Deactivate(statusRequest.userID, statusRequest.reason);
            return (process.error) ? BadRequest(process) : Ok(process);
        }
    }
}
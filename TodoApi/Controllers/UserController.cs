using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Services.UserServices;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all user data related requests.
    /// </summary>
    [Route(Routes.UserRoute)]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userService">TODO</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            return Ok();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserById(string userId)
        {
            try
            {
                await _userService.RemoveUserByIdAsync(userId);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (RemoveUserFailedException)
            {
                return StatusCode(520);
            }
        }
    }
}
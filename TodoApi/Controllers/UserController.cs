using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Services.Interfaces;

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
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="userService">A service that implements IUserService</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Get a specific user given his id.
        /// GET api/{version}/user/{id}
        /// </summary>
        /// <param name="userId">The id of the requested user</param>
        /// <returns>200 and user dto if successful, 404 otherwise</returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                return Ok(await _userService.GetUserByIdAsync(userId));
            }
            catch(UserNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all users.
        /// GET api/{version}/user
        /// </summary>
        /// <returns>200 and a list of user dto</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersOrderedByNameAsync());
        }

        /// <summary>
        /// Delete a specific user.
        /// DELETE api/{version}/user/{id}
        /// </summary>
        /// <param name="userId">The id of the requested user</param>
        /// <returns>204 if successful, 404 if user is not found, 520 otherwise</returns>
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
        }
    }
}
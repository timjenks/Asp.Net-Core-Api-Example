#region Imports

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Utils.Constants;
using TodoApi.Exceptions;
using TodoApi.Services.Interfaces;
using TodoApi.Models.DtoModels;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

#endregion

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all user data related requests.
    /// </summary>
    [Produces("application/json")]
    [Route(Routes.UserRoute)]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="userService">A service that implements IUserService</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region GetUser

        /// <summary>
        /// Get a specific user given his id.
        /// </summary>
        /// <remarks>GET api/{version}/user/{id}</remarks>
        /// <param name="userId">The id of the requested user</param>
        /// <response code="200">User received</response>
        /// <response code="401">Token required</response>
        /// <response code="403">Token owner does not have required access to this resource</response>
        /// <response code="404">User requested does not exist</response>
        /// <returns>200 and user dto if successful, 404 otherwise</returns>
        [SwaggerResponse(200, typeof(ApplicationUserDto))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(403, typeof(void))]
        [SwaggerResponse(404, typeof(void))]
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

        #endregion

        #region GetAllUsers

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <remarks>GET api/{version}/user</remarks>
        /// <response code="200">Users received</response>
        /// <response code="401">Token required</response>
        /// <response code="403">Token owner does not have required access to this resource</response>
        /// <returns>200 and a list of user dto</returns>
        [SwaggerResponse(200, typeof(IEnumerable<ApplicationUserDto>))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(403, typeof(void))]
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersOrderedByNameAsync());
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete a specific user.
        /// </summary>
        /// <remarks>DELETE api/{version}/user/{id}</remarks>
        /// <param name="userId">The id of the requested user</param>
        /// <response code="204">User deleted</response>
        /// <response code="401">Token required</response>
        /// <response code="403">Token owner does not have required access to this resource</response>
        /// <response code="404">User requested does not exist</response>
        /// <returns>204 if successful, 404 if user is not found, 520 otherwise</returns>
        [SwaggerResponse(204, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(403, typeof(void))]
        [SwaggerResponse(404, typeof(void))]
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

        #endregion
    }
}
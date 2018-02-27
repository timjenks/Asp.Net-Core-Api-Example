using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all user data related requests.
    /// </summary>
    [Route(Routes.UserRoute)]
    public class UserController : Controller
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        [HttpGet("{userId:string}")]
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
        [HttpDelete("{userId:string}")]
        public async Task<IActionResult> RemoveUserById(string userId)
        {
            return Ok();
        }
    }
}
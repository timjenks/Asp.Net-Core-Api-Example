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
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:string}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId:string}")]
        public async Task<IActionResult> RemoveUserById(string userId)
        {
            return Ok();
        }
    }
}
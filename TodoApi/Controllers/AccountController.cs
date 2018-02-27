using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Models.ViewModels;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all account related requests.
    /// </summary>
    [Route(Routes.AccountRoute)]
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            return Ok();
        }
    }
}
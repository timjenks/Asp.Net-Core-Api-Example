using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
using TodoApi.Services.AccountServices;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all account related requests.
    /// </summary>
    [Route(Routes.AccountRoute)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="accountService">TODO</param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="model">TODO</param>
        /// <returns>TODO</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _accountService.Login(model);
                return Ok(token);
            }
            catch (LoginFailException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="model">TODO</param>
        /// <returns>TODO</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _accountService.Register(model);
                return Ok(token);
            }
            catch (RegisterFailException)
            {
                return Unauthorized();
            }
        }
    }
}
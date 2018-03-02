using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;

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
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="accountService">A service that implements IAccountService</param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Request a token for an existing user.
        /// POST api/{version}/account/login
        /// </summary>
        /// <param name="model">A model with login information</param>
        /// <returns>200 and a bearer token if successful, 
        /// 400 if missing data, 401 otherwise</returns>
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
        /// Request a token for a new user.
        /// POST api/{version}/account/register
        /// </summary>
        /// <param name="model">A model with register information</param>
        /// <returns>201 and a bearer token if successful, 
        /// 400 if missing data, 401 otherwise</returns>
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
                return StatusCode(201, token);
            }
            catch (RegisterFailException)
            {
                return Unauthorized();
            }
        }
    }
}
#region Imports

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApi.Utils.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all account related requests.
    /// </summary>
    [Produces("application/json")]
    [Route(Routes.AccountRoute)]
    public class AccountController : Controller
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="accountService">A service that implements IAccountService</param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Login

        /// <summary>
        /// Request a token for an existing user.
        /// </summary>
        /// <remarks>POST api/{version}/account/login</remarks>
        /// <param name="model">A model with login information</param>
        /// <response code="200">Token received</response>
        /// <response code="400">Invalid data in body</response>
        /// <response code="401">Login failed</response>
        /// <returns>200 and a bearer token if successful, 
        /// 400 if missing data, 401 otherwise</returns>
        [SwaggerResponse(200, typeof(string))]
        [SwaggerResponse(400, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
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

        #endregion

        #region Register

        /// <summary>
        /// Request a token for a new user.
        /// </summary>
        /// <remarks>POST api/{version}/account/register</remarks>
        /// <param name="model">A model with register information</param>
        /// <response code="201">Token received</response>
        /// <response code="400">Invalid data in body</response>
        /// <response code="401">Register failed</response>
        /// <returns>201 and a bearer token if successful, 
        /// 400 if missing data, 401 otherwise</returns>
        [SwaggerResponse(201, typeof(string))]
        [SwaggerResponse(400, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
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
            catch (PasswordModelException)
            {
                return BadRequest(PasswordLimits.SettingsErrorMessages);
            }
            catch (RegisterFailException)
            {
                return Unauthorized();
            }
        }

        #endregion
    }
}
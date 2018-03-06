using Jwt;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.EntityModels;
using TodoApi.Models.ViewModels;
using TodoApi.Services;
using Xunit;

namespace Tests.ServicesTests
{
    /// <summary>
    /// Testing AccountService with in memory sqlite database,
    /// mock config, user manager and sign in manager.
    /// </summary>
    public class AccountServiceTest
    {
        private readonly AccountService _service;
        private readonly MockConfiguration _config;
        private readonly AppDataContext _ctx;

        /// <summary>
        /// Before each.
        /// </summary>
        public AccountServiceTest()
        {
            _ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(_ctx);
            var signInManager = new MockSignInManager(userManager);
            _config = new MockConfiguration();
            _service = new AccountService(userManager, signInManager, _config, _ctx);
        }

        #region Login

        [Fact]
        public async Task Login_NonExistingUser_LoginFailException()
        {
            // Arrange
            var credentials = new LoginViewModel
            {
                Email = "horse@name.com",
                Password = "A_h0rse_with_n0_name"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<LoginFailException>(() => _service.Login(credentials));
        }

        [Fact]
        public async Task Login_ExistingUserWrongPassword_LoginFailException()
        {
            // Arrange
            var credentials = new LoginViewModel
            {
                Email = MockApplicationUsers.Get(6).Email,
                Password = "A_h0rse_with_n0_name"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<LoginFailException>(() => _service.Login(credentials));
        }

        [Fact]
        public async Task Login_ExistingUserCorrectPassword_JwtToken()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var credentials = new LoginViewModel()
            {
                Email = user.Email,
                Password = MockApplicationUsers.UniversalPassword
            };

            // Act
            var token = await _service.Login(credentials);

            // Assert
            CheckToken(token, user);
        }

        #endregion

        #region Register

        [Fact]
        public async Task Register_ExistingUser_RegisterFailException()
        {
            // Arrange
            var oldUser = MockApplicationUsers.Get(3);
            var credentials = new RegisterViewModel
            {
                Name = oldUser.Name,
                Email = oldUser.Email,
                Password = MockApplicationUsers.UniversalPassword
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<RegisterFailException>(() => _service.Register(credentials)); 
        }

        [Fact]
        public async Task Register_NewUserInvalidPassword_PasswordModelException()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Name = "Gloria Estefan",
                Email = "beat@dr.com",
                Password = "only lower"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<PasswordModelException>(() => _service.Register(credentials));
        }

        [Fact]
        public async Task Register_NewUserValidPassword_JwtToken()
        {
            // Arrange
            var credentials = new RegisterViewModel
            {
                Name = "Laura Branigan",
                Email = "control@self.com",
                Password = MockApplicationUsers.UniversalPassword
            };

            // Act
            var token = await _service.Register(credentials);
            var user = _ctx.Users.SingleOrDefault(w => w.UserName == credentials.Email);

            // Assert
            Assert.NotNull(user);
            CheckToken(token, user);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helper for validating tokens for users with assertion.
        /// </summary>
        /// <param name="token">The token received from login or register</param>
        /// <param name="user">The owner of the token</param>
        private void CheckToken(string token, ApplicationUser user)
        {
            // In this scenario, role is 1-to-many, not many-to-many.
            // Need to alter for multiple user roles.
            var role = (from ur in _ctx.UserRoles
                        join r in _ctx.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select r.Name).SingleOrDefault();
            var data = JsonWebToken.Decode(token, _config["SecretKey"]);
            var json = JObject.Parse(data);
            Assert.Equal(user.Email, json.GetValue("sub"));
            Assert.Equal(user.Id, json.GetValue(ClaimTypes.NameIdentifier));
            Assert.Equal(user.Name, json.GetValue(ClaimTypes.Name));
            Assert.Equal(role, json.GetValue(ClaimTypes.Role));
        }

        #endregion
    }
}

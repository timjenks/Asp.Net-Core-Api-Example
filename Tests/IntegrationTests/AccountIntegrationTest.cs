using System;
using Jwt;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.Helpers.Json;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Utils.Constants;
using TodoApi.Models.EntityModels;
using Xunit;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Start the server with a in memory version of 
    /// our startup using the mock data and test it
    /// with a local client making http requests to
    /// it and assert on responses received. 
    /// </summary>
    public class AccountIntegrationTest
    {
        private readonly MockServerAndClient _endSystems;

        /// <summary>
        /// Before each.
        /// </summary>
        public AccountIntegrationTest()
        {
            _endSystems = new MockServerAndClient();
        }

        #region Login

        [Fact]
        public async Task PostAccountLogin_InvalidContentType_UnsupportedMediaType()
        {
            // Arrange
            var content = new StringContent("Home is where I want to be Pick me up and turn me around I feel numb, born with a weak heart I guess I must be having fun");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content, "application/text");

            // Assert
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountLogin_NoContent_BadRequest()
        {
            // Arrange
            var content = new StringContent("");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountLogin_InvalidContent_BadRequest()
        {
            // Arrange
            var content = new StringContent("{}");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountLogin_NonExistingUser_Unauthorized()
        {
            // Arrange
            var body = StringJsonBuilder.LoginJsonBody("invalid@mail.com", "J0hn_Carmack");
            var content = new StringContent(body);

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountLogin_ExistingUserWrongPassword_Unauthorized()
        {
            // Arrange
            var user = MockApplicationUsers.Get(6);
            var body = StringJsonBuilder.LoginJsonBody(user.Email, "A-Am-N0t-C0rrect");
            var content = new StringContent(body);

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountLogin_ExistingUser_OkWithToken()
        {
            // Arrange
            var user = MockApplicationUsers.Get(6);
            var body = StringJsonBuilder.LoginJsonBody(user.Email, MockApplicationUsers.UniversalPassword);
            var content = new StringContent(body);
            var role = MockRoles.Admin.Id == MockUserRoles.GetUserRoleForUser(user.Id).RoleId 
                ? MockRoles.Admin : MockRoles.User;

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);
            var token = response.Body;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            CheckToken(role, user, token);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Register

        [Fact]
        public async Task PostAccountRegister_InvalidContentType_UnsupportedMediaType()
        {
            // Arrange
            var content = new StringContent("Home, is where I want to be But I guess I'm already there I come home, she lifted up her wings I guess that this must be the place");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/register", content, "application/text");

            // Assert
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountRegiser_NoContent_BadRequest()
        {
            // Arrange
            var content = new StringContent("");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountRegiser_InvalidContent_BadRequest()
        {
            // Arrange
            var content = new StringContent("{}");

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountRegiser_WeakPassword_BadRequest()
        {
            // Arrange
            var body = StringJsonBuilder.RegisterJsonBody("The Message", "grandmaster@flash.com", "onlylowercases");
            var content = new StringContent(body);

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountRegiser_EmailInUse_Unauthorized()
        {
            // Arrange
            var user = MockApplicationUsers.Get(8);
            var body = StringJsonBuilder.RegisterJsonBody(user.Name, user.Email, MockApplicationUsers.UniversalPassword);
            var content = new StringContent(body);

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/register", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task PostAccountRegiser_ValidNewUser_CreatedAndToken()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Name = "The Clash",
                Email = "police@thieves.com",
                UserName = "police@thieves.com"
            };
            var registerBody = StringJsonBuilder.RegisterJsonBody
            (
                user.Name, 
                user.Email,
                MockApplicationUsers.UniversalPassword
            );
            var loginBody = StringJsonBuilder.LoginJsonBody
            (
                user.Email,
                MockApplicationUsers.UniversalPassword
            );
            var registerContent = new StringContent(registerBody);
            var loginContent = new StringContent(loginBody);

            // Act
            var registerResponse = await _endSystems.Post(Routes.AccountRoute + "/register", registerContent);
            var loginResponse = await _endSystems.Post(Routes.AccountRoute + "/login", loginContent);
            var registerToken = registerResponse.Body;
            var loginToken = loginResponse.Body;

            // Assert
            Assert.Equal(HttpStatusCode.Created, registerResponse.Code);
            Assert.Equal(HttpStatusCode.OK, loginResponse.Code);
            CheckToken(MockRoles.User, user, registerToken, false);
            CheckToken(MockRoles.User, user, loginToken, false);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Assertion for token.
        /// </summary>
        /// <param name="role">The role of the user. Singular only!</param>
        /// <param name="user">The owner of the token</param>
        /// <param name="token">The token as string</param>
        /// <param name="checkId">Optional parameter on if the id should be checked</param>
        /// <exception cref="ArgumentNullException">If parameters are null</exception>
        private static void CheckToken(IdentityRole role, ApplicationUser user, 
            string token, bool checkId = true)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (token == null) throw new ArgumentNullException(nameof(token));
            var check = checkId;
            var data = JsonWebToken.Decode(token, new MockConfiguration()["SecretKey"]);
            var json = JObject.Parse(data);
            Assert.Equal(user.Email, json.GetValue("sub"));
            if (check)
            {
                Assert.Equal(user.Id, json.GetValue(ClaimTypes.NameIdentifier));
            }
            Assert.Equal(user.Name, json.GetValue(ClaimTypes.Name));
            Assert.Equal(role.Name, json.GetValue(ClaimTypes.Role));
        }

        #endregion
    }
}

using Jwt;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Constants;
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
            var body = ConstructLoginBody("invalid@mail.com", "J0hn_Carmack");
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
            var body = ConstructLoginBody(user.Email, "A-Am-N0t-C0rrect");
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
            var body = ConstructLoginBody(user.Email, MockApplicationUsers.UniversalPassword);
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
            var body = ConstructRegisterBody("The Message", "grandmaster@flash.com", "onlylowercases");
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
            var body = ConstructRegisterBody(user.Name, user.Email, MockApplicationUsers.UniversalPassword);
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
            var registerBody = ConstructRegisterBody
            (
                user.Name, 
                user.Email,
                MockApplicationUsers.UniversalPassword
            );
            var loginBody = ConstructLoginBody
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

        private void CheckToken(IdentityRole role, ApplicationUser user, 
            string token, bool checkId = true)
        {
            var data = JsonWebToken.Decode(token, new MockConfiguration()["SecretKey"]);
            var json = JObject.Parse(data);
            Assert.Equal(user.Email, json.GetValue("sub"));
            if (checkId)
            {
                Assert.Equal(user.Id, json.GetValue(ClaimTypes.NameIdentifier));
            }
            Assert.Equal(user.Name, json.GetValue(ClaimTypes.Name));
            Assert.Equal(role.Name, json.GetValue(ClaimTypes.Role));
        }

        /// <summary>
        /// Create a json string from given fields.
        /// </summary>
        private string ConstructRegisterBody(string name, string email, string password)
        {
            return new StringBuilder(36 + name.Length + email.Length + password.Length)
                .Append('{')
                .Append('"')
                .Append("Email")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(email)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Password")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(password)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Name")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(name)
                .Append('"')
                .Append('}')
                .ToString();
        }

        /// <summary>
        /// Create a json string from given fields.
        /// </summary>
        private string ConstructLoginBody(string email, string password)
        {
            return new StringBuilder(26 + email.Length + password.Length)
                .Append('{')
                .Append('"')
                .Append("Email")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(email)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Password")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(password)
                .Append('"')
                .Append('}')
                .ToString();
        }

        #endregion
    }
}

using Jwt;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Constants;
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
            var content = new StringContent
            (
                "{\"Email\": \"invalid@mail.com\", \"Password\": \"J0hn_Carmack\"}"
            );

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
            var body = "{\"Email\": \"" 
                + user.Email 
                + "\", \"Password\": \""
                + MockApplicationUsers.UniversalPassword 
                + "\" }";
            var content = new StringContent(body);
            var role = MockRoles.Admin.Id == MockUserRoles.GetUserRoleForUser(user.Id).RoleId 
                ? MockRoles.Admin : MockRoles.User;

            // Act
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);
            var token = response.Body;
            var data = JsonWebToken.Decode(token, new MockConfiguration()["SecretKey"]);
            var json = JObject.Parse(data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Equal(MockApplicationUsers.Get(6).Email, json.GetValue("sub"));
            Assert.Equal(MockApplicationUsers.Get(6).Id, json.GetValue(ClaimTypes.NameIdentifier));
            Assert.Equal(MockApplicationUsers.Get(6).Name, json.GetValue(ClaimTypes.Name));
            Assert.Equal(role.Name, json.GetValue(ClaimTypes.Role));

            // Tear down
            _endSystems.Dispose();
        }
    }
}

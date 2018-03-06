using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.Helpers.Json;
using Tests.MockData.EntityModels;
using TodoApi.Constants;
using TodoApi.Models.DtoModels;
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
    public class UserIntegrationTest
    {
        private readonly MockServerAndClient _endSystems;

        /// <summary>
        /// Before each.
        /// </summary>
        public UserIntegrationTest()
        {
            _endSystems = new MockServerAndClient();
        }

        #region GetUser


        [Fact]
        public async Task GetUserById_NoToken_Unauthorized()
        {
            // Arrange
            var userToFind = MockApplicationUsers.Get(4);
            var path = $"{Routes.UserRoute}/{userToFind.Id}";

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetUserById_NonAdmin_Forbidden()
        {
            // Arrange
            var userToRequest = MockApplicationUsers.Get(9);
            var userToFind = MockApplicationUsers.Get(4);
            var path = $"{Routes.UserRoute}/{userToFind.Id}";
            var token = await GetToken(userToRequest);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_NotFound()
        {
            // Arrange
            var adminToRequest = MockApplicationUsers.Get(0);
            const string userIdToFind = "7039d78b-a954-4552-af92-e7a1b90bbd9e";
            var path = $"{Routes.UserRoute}/{userIdToFind}";
            var token = await GetToken(adminToRequest);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetUserById_ExistingUser_Ok()
        {
            // Arrange
            var adminToRequest = MockApplicationUsers.Get(0);
            var userToFind = MockApplicationUsers.Get(1);
            var expectedDto = new ApplicationUserDto(userToFind);
            var path = $"{Routes.UserRoute}/{userToFind.Id}";
            var token = await GetToken(adminToRequest);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dto = JsonStringSerializer.GetApplicationUserDto(response.Body);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Equal(expectedDto.Id, dto.Id);
            Assert.Equal(expectedDto.Email, dto.Email);
            Assert.Equal(expectedDto.Name, dto.Name);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Helpers

        public async Task<string> GetToken(ApplicationUser user)
        {
            Assert.NotNull(user);
            Assert.NotNull(user.Email);
            var body = StringJsonBuilder.LoginJsonBody(user.Email, MockApplicationUsers.UniversalPassword);
            var content = new StringContent(body);
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);
            Assert.NotNull(response);
            var token = response.Body;
            Assert.NotNull(token);
            return token;
        }

        #endregion
    }
}

/*

            var user = MockApplicationUsers.Get(0);
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            var path = Routes.UserRoute + "/" + MockApplicationUsers.Get(4).Id;

            var response = await _endSystems.Get(path);
*/

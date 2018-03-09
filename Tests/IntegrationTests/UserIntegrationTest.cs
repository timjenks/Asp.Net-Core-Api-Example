using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.Helpers.Json;
using Tests.MockData.EntityModels;
using TodoApi.Utils.Constants;
using TodoApi.Models.DtoModels;
using TodoApi.Models.EntityModels;
using Xunit;
using System.Linq;
using System;

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

        #region GetAllUsers

        [Fact]
        public async Task GetAllUsers_NoToken_Unauthorized()
        {
            // Arrange
            const string path = Routes.UserRoute;

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllUsers_NonAdmin_Forbidden()
        {
            // Arrange
            var user = MockApplicationUsers.Get(5);
            const string path = Routes.UserRoute;
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllUsers_AdminRequesting_OkWithAllUsersInOrder()
        {
            // Arrange
            var requestingUser = MockApplicationUsers.Get(0);
            const string path = Routes.UserRoute;
            var token = await GetToken(requestingUser);
            _endSystems.SetBearerToken(token);
            var expectedDtos = 
                MockApplicationUsers
                .GetAll()
                .OrderBy(w => w.Name)
                .Select(z => new ApplicationUserDto(z))
                .ToArray();
            var first = true;
            ApplicationUserDto last = null;

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfApplicationUserDto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Equal(expectedDtos.Length, dtos.Length);
            for (var i = 0; i < dtos.Length; i++)
            {
                Assert.Equal(expectedDtos[i].Id, dtos[i].Id);
                Assert.Equal(expectedDtos[i].Email, dtos[i].Email);
                Assert.Equal(expectedDtos[i].Name, dtos[i].Name);
                if (first)
                {
                    first = false;
                }
                else
                {
                    Assert.True(string.Compare(dtos[i].Name, last.Name, StringComparison.Ordinal) >= 0);
                }
                last = dtos[i];
            }

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Delete

        [Fact]
        public async Task RemoveUserById_NoToken_Unauthorized()
        {
            // Arrange
            var userToRemove = MockApplicationUsers.Get(4);
            var path = $"{Routes.UserRoute}/{userToRemove.Id}";

            // Act
            var response = await _endSystems.Delete(path);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task RemoveUserById_NonAdmin_Forbidden()
        {
            // Arrange
            var userThatRomves = MockApplicationUsers.Get(1);
            var userToRemove = MockApplicationUsers.Get(4);
            var path = $"{Routes.UserRoute}/{userToRemove.Id}";
            var token = await GetToken(userThatRomves);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Delete(path);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task RemoveUserById_NonExistingUser_NotFound()
        {
            // Arrange
            var adminUser = MockApplicationUsers.Get(0);
            const string userIdToRemove = "c09fda23-61c4-49db-873d-eb86224befea";
            var path = $"{Routes.UserRoute}/{userIdToRemove}";
            var token = await GetToken(adminUser);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Delete(path);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task RemoveUserById_ExistingUser_NoContent()
        {
            // Arrange
            var adminUser = MockApplicationUsers.Get(0);
            var userToRemove = MockApplicationUsers.Get(7);
            var path = $"{Routes.UserRoute}/{userToRemove.Id}";
            var token = await GetToken(adminUser);
            _endSystems.SetBearerToken(token);

            // Act
            var deleteResponse = await _endSystems.Delete(path);
            var getResponse = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.Code);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.Code);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Login with a user and return the token as string.
        /// </summary>
        /// <param name="user">A user to login</param>
        /// <returns>A token as string</returns>
        private async Task<string> GetToken(ApplicationUser user)
        {
            Assert.NotNull(user);
            Assert.NotNull(user.Email);
            var body = JsonStringBuilder.LoginJsonBody(
                user.Email, MockApplicationUsers.UniversalPassword);
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
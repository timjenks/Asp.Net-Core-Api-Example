using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.Helpers.Json;
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

        [Fact]
        public async Task GetUserId_()
        {
            var user = MockApplicationUsers.Get(0);
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            var path = Routes.UserRoute + "/" + MockApplicationUsers.Get(4).Id;

            var response = await _endSystems.Get(path);

            var x = 55;
        }

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

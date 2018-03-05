using Jwt;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Models.ViewModels;
using TodoApi.Services;
using Xunit;

namespace Tests.ServicesTests
{
    public class AccountServiceTest
    {
        private readonly AccountService _service;
        private MockConfiguration _config;

        public AccountServiceTest()
        {
            var ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(ctx);
            var signInManager = new MockSignInManager(userManager);
            _config = new MockConfiguration();

            _service = new AccountService(userManager, signInManager, _config);
        }

        [Fact]
        public async Task TestingMockkkkkkkkkkkkkkkkkkkk()
        {
            var m = new LoginViewModel()
            {
                Email = MockApplicationUsers.Get(4).Email,
                Password = MockApplicationUsers.UniversalPassword
            };
            var token = await _service.Login(m);

            try
            {
                var data = JsonWebToken.Decode(token, _config["SecretKey"]);

                var o = JObject.Parse(data);
                var u = MockApplicationUsers.Get(4);
                Assert.Equal(u.Email, o.GetValue("sub"));
                Assert.Equal(u.Id, o.GetValue(ClaimTypes.NameIdentifier));
                Assert.Equal(u.Name, o.GetValue(ClaimTypes.Name));
            }
            catch (SignatureVerificationException)
            {
                // TODO: REMOVE
                Assert.False(true); // <--- skip try-catch in test unless you want to fail... just here for debugging...
            }
        }
    }
}

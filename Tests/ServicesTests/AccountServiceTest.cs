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

        public AccountServiceTest()
        {
            var ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(ctx);
            var signInManager = new MockSignInManager(userManager);
            var config = new MockConfiguration();

            _service = new AccountService(userManager, signInManager, config);
        }

        [Fact]
        public async Task TestingMockkkkkkkkkkkkkkkkkkkk()
        {
            var m = new LoginViewModel()
            {
                Email = MockApplicationUsers.Get(4).Email,
                Password = MockApplicationUsers.UniversalPassword
            };
            var x = await _service.Login(m);

            
            // change name and validate token?
        }
    }
}

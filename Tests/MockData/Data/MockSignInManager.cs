using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Tests.MockData.EntityModels;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.Data
{
    public class MockSignInManager : SignInManager<ApplicationUser>
    {
        public MockSignInManager(UserManager<ApplicationUser> userManager)
            : base(userManager,
                  new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object)
        { }

        public override async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            var userFound = await UserManager.FindByEmailAsync(user.Email) != null;
            var passwordCorrect = MockApplicationUsers.UniversalPassword == password;
            return userFound && passwordCorrect ? SignInResult.Success : SignInResult.Failed;

        }

        public override async Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            await Task.Run(() => { });
        }
    }
}

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
    /// <inheritdoc />
    /// <summary>
    /// Mock of a sign in manager. It only implements the 
    /// method that are used in this project but more can
    /// easily be added. The validation on sign in actually
    /// checks the in memory database if the user exists
    /// and if the password is the password every mock user
    /// is provided with. That is why a user manager is
    /// not mocked like the rest of the parameters past 
    /// to the super class.
    /// </summary>
    public class MockSignInManager : SignInManager<ApplicationUser>
    {
        /// <inheritdoc />
        /// <summary>
        /// Takes a usermanager but mocks all other object past to super class.
        /// </summary>
        /// <param name="userManager">A mocked user manager</param>
        public MockSignInManager(UserManager<ApplicationUser> userManager)
            : base(userManager,
                  new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object)
        { }

        /// <inheritdoc />
        /// <summary>
        /// When signed in, we look for a user with this email and
        /// check if the password matches the universal password
        /// used by all our mock users.
        /// </summary>
        /// <param name="user">The user trying to login</param>
        /// <param name="password">the password he entered</param>
        /// <param name="isPersistent">always false</param>
        /// <param name="lockoutOnFailure">always false</param>
        /// <returns>Success result if correct password and user exists, failure otherwise</returns>
        public override async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            var userFound = await UserManager.FindByEmailAsync(user.Email) != null;
            var passwordCorrect = MockApplicationUsers.UniversalPassword == password;
            return userFound && passwordCorrect ? SignInResult.Success : SignInResult.Failed;
        }

        /// <inheritdoc />
        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="user">The user to sign in</param>
        /// <param name="isPersistent">always set to false</param>
        /// <param name="authenticationMethod">always set to false</param>
        public override async Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            await Task.Run(() => { });
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Data;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.Data
{
    /// <summary>
    /// Mock for the user manager that actually uses the in memory database.
    /// </summary>
    public class MockUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// The in memory data context.
        /// </summary>
        private readonly AppDataContext _ctx;

        /// <summary>
        /// Calls super class with a store from the context and 
        /// various other properties. The properties are passed
        /// as null if they are not needed in the current services.
        /// Others might have to add more or possibly mock.
        /// </summary>
        /// <param name="ctx">In memory application db context</param>
        public MockUserManager(AppDataContext ctx) : base
        (
            new UserStore<ApplicationUser>(ctx),
            null,                                                       // IOptions <IdentityOptions>
            new PasswordHasher<ApplicationUser>(),                      // IPasswordHasher<ApplicationUser> 
            new[] { new MockUserValidator() },                          // IEnumerable<IUserValidator<ApplicationUser>> 
            new[] { new MockPasswordValidator() },                      // IEnumerable<IPasswordValidator<ApplicationUser>>
            new MockNormalizer(),                                       // ILookupNormalizer 
            new IdentityErrorDescriber(),                               // IdentityErrorDescriber 
            null,                                                       // IServiceProvider 
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object    // ILogger<UserManager<ApplicationUser>>
        )
        {
            _ctx = ctx;
        }

        /// <summary>
        /// When user manager is asked to add user to a role, this is run in stead.
        /// </summary>
        /// <param name="user">Instance of the user</param>
        /// <param name="role">Name of the role</param>
        /// <returns>Successs is always returned</returns>
        public override async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            var roleId = role.ToUpper() == "ADMIN" ? "314" : "2718";
            var ur = new IdentityUserRole<string> { UserId = user.Id, RoleId = roleId };
            _ctx.UserRoles.Add(ur);
            await _ctx.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}

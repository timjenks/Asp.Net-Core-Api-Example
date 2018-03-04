using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        /// Calls super class with a store from the context and 
        /// various other properties. The properties are passed
        /// as null if they are not needed in the current services.
        /// Others might have to add more or possibly mock.
        /// </summary>
        /// <param name="ctx">In memory application db context</param>
        public MockUserManager(AppDataContext ctx) : base
        (
            new UserStore<ApplicationUser, IdentityRole, AppDataContext, string>(ctx), 
            null,                                   // IOptions <IdentityOptions>
            new PasswordHasher<ApplicationUser>(),  // IPasswordHasher<ApplicationUser> 
            null,                                   // IEnumerable<IUserValidator<ApplicationUser>> 
            null,                                   // IEnumerable<IPasswordValidator<ApplicationUser>>
            new MockNormalizer(),                   // ILookupNormalizer 
            null,                                   // IdentityErrorDescriber 
            null,                                   // IServiceProvider 
            null                                    // ILogger<UserManager<ApplicationUser>>
        ) { }
    }
}

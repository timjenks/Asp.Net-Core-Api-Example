using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.Data
{
    public class MockUserManager : UserManager<ApplicationUser>
    {
        public MockUserManager(
            AppDataContext ctx
        ) : base
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
            )
        { }
    }
}

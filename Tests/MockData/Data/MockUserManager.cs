﻿#region Imports

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Models.EntityModels;

#endregion

namespace Tests.MockData.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Mock for the user manager that actually uses the in memory database.
    /// </summary>
    public class MockUserManager : UserManager<ApplicationUser>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Calls super class with a store from the context and 
        /// various other properties. The properties are passed
        /// as null if they are not needed in the current services.
        /// Others might have to add more or possibly mock.
        /// </summary>
        /// <param name="ctx">In memory application db context</param>
        public MockUserManager(DbContext ctx) : base
        (
            new UserStore<ApplicationUser>(ctx),                        // IUserManager<ApplicationUser>
            null,                                                       // IOptions<IdentityOptions>
            new PasswordHasher<ApplicationUser>(),                      // IPasswordHasher<ApplicationUser> 
            new[] { new MockUserValidator() },                          // IEnumerable<IUserValidator<ApplicationUser>> 
            new[] { new MockPasswordValidator() },                      // IEnumerable<IPasswordValidator<ApplicationUser>>
            new MockNormalizer(),                                       // ILookupNormalizer 
            new IdentityErrorDescriber(),                               // IdentityErrorDescriber 
            null,                                                       // IServiceProvider 
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object    // ILogger<UserManager<ApplicationUser>>
        )
        {
        }

        #endregion
    }
}

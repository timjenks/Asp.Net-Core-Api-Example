#region Imports

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TodoApi.Models.EntityModels;

#endregion

namespace Tests.MockData.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Validator for registering a user that already exists.
    /// </summary>
    public class MockUserValidator : UserValidator<ApplicationUser>
    {
        /// <inheritdoc />
        /// <summary>
        /// Check if user is in database and if so, return error.
        /// </summary>
        /// <param name="manager">The user manager used</param>
        /// <param name="user">An instance of the user</param>
        /// <returns>Failure if user can already be found in database, success otherwise</returns>
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            await Task.Run(() => { });
            return (await manager.FindByNameAsync(user.Email)) != null ? 
                IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "X" }) : 
                IdentityResult.Success;
        }
    }
}
#region Imports

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Utils.Constants;
using TodoApi.Models.EntityModels;

#endregion

namespace Tests.MockData.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Validator for password error based on our settings.
    /// </summary>
    public class MockPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        /// <inheritdoc />
        /// <summary>
        /// Validated our password and sets errors if any.
        /// </summary>
        /// <param name="manager">User manager used</param>
        /// <param name="user">Instance of the user</param>
        /// <param name="password">The password being validated</param>
        /// <returns>Success if valid password, failure otherwise</returns>
        public async Task<IdentityResult> ValidateAsync(
            UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            await Task.Run(() => { });

            var errors = new List<IdentityError>();

            if (!HasEnoughUniqueChars(password))
            {
                errors.Add(new IdentityError { Code = "PasswordRequiresUniqueChars", Description = "X" });
            }

            if (!HasDigit(password))
            {
                errors.Add(new IdentityError { Code = "PasswordRequiresDigit", Description = "X" });
            }

            if (!HasUpper(password))
            {
                errors.Add(new IdentityError { Code = "PasswordRequiresUpper", Description = "X" });
            }

            if (!HasLower(password))
            {
                errors.Add(new IdentityError { Code = "PasswordRequiresLower", Description = "X" });
            }

            if (!HasNonAlphanumeric(password))
            {
                errors.Add(new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = "X" });
            }

            return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        #region Helpers

        /// <summary>
        /// Check if password contains the minimum amount of unique characters.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if does contain, false otherwise</returns>
        private static bool HasEnoughUniqueChars(string pw)
        {
            return pw.ToCharArray().ToHashSet().Count() >= PasswordLimits.AccountMinUniqueChars;
        }

        /// <summary>
        /// Check if password contains a digit.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private static bool HasDigit(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireDigit || pw.Any(char.IsDigit);
        }

        /// <summary>
        /// Check if password contains an upper case character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private static bool HasUpper(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireUppercase || pw.Any(char.IsUpper);
        }

        /// <summary>
        /// Check if password contains an lower case character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private static bool HasLower(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireLowercase || pw.Any(char.IsLower);
        }

        /// <summary>
        /// Check if password contains a non-alphanumerical character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private static bool HasNonAlphanumeric(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireNonAlphanumeric || !pw.All(char.IsLetterOrDigit);
        }

        #endregion
    }
}

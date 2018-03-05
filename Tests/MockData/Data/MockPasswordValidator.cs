using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.Data
{
    /// <summary>
    /// Validator for password error based on our settings.
    /// </summary>
    public class MockPasswordValidator : IPasswordValidator<ApplicationUser>
    {
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

            return errors.Count() > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        /// <summary>
        /// Check if password contains the minimum amount of unique characters.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if does contain, false otherwise</returns>
        private bool HasEnoughUniqueChars(string pw)
        {
            return pw.ToCharArray().ToHashSet().Count() >= PasswordLimits.AccountMinUniqueChars;
        }

        /// <summary>
        /// Check if password contains a digit.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private bool HasDigit(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireDigit || pw.Any(c => char.IsDigit(c));
        }

        /// <summary>
        /// Check if password contains an upper case character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private bool HasUpper(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireUppercase || pw.Any(c => char.IsUpper(c));
        }

        /// <summary>
        /// Check if password contains an lower case character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private bool HasLower(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireLowercase || pw.Any(c => char.IsLower(c));
        }

        /// <summary>
        /// Check if password contains a non-alphanumerical character.
        /// </summary>
        /// <param name="pw">Password as a sting</param>
        /// <returns>True if not needed or does contain, false otherwise</returns>
        private bool HasNonAlphanumeric(string pw)
        {
            return !PasswordLimits.PasswordSettings.RequireNonAlphanumeric || !pw.All(c => char.IsLetterOrDigit(c));
        }
    }
}

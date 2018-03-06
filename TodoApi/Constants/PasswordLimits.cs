using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TodoApi.Constants
{
    /// <summary>
    /// A static collection of values that have to do with passwords.
    /// </summary>
    public static class PasswordLimits
    {
        /// <summary>
        /// Minimum password length as string.
        /// </summary>
        public const string AccountMinPasswordLengthString = "6";

        /// <summary>
        /// Minimum password length.
        /// </summary>
        public const int AccountMinPasswordLength = 6;

        /// <summary>
        /// Maximum password length as string.
        /// </summary>
        public const string AccountMaxPasswordLengthString = "25";

        /// <summary>
        /// Maximum password length.
        /// </summary>
        public const int AccountMaxPasswordLength = 25;

        /// <summary>
        /// The minimum of unique characters in the password.
        /// </summary>
        public const int AccountMinUniqueChars = 4;

        /// <summary>
        /// Password options to use in Asp.Net Identity.
        /// </summary>
        public static readonly PasswordOptions PasswordSettings = new PasswordOptions()
        {
            RequiredLength = AccountMinPasswordLength,
            RequiredUniqueChars = AccountMinUniqueChars,
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true
        };

        /// <summary>
        /// Set of all possible password error codes.
        /// </summary>
        public static readonly HashSet<string> SettingsErrorMessages = GetErrorCodes();

        /// <summary>
        /// Construct a set of possible error codes from password settings.
        /// </summary>
        /// <returns>A hashset of strings, containing error codes</returns>
        private static HashSet<string> GetErrorCodes()
        {
            var set = new HashSet<string>();
            if (PasswordSettings.RequiredUniqueChars > 0)
            {
                set.Add("PasswordRequiresUniqueChars");
            }
            if (PasswordSettings.RequireDigit)
            {
                set.Add("PasswordRequiresDigit");
            }
            if (PasswordSettings.RequireLowercase)
            {
                set.Add("PasswordRequiresLower");
            }
            if (PasswordSettings.RequireUppercase)
            {
                set.Add("PasswordRequiresUpper");
            }
            if (PasswordSettings.RequireNonAlphanumeric)
            {
                set.Add("PasswordRequiresNonAlphanumeric");
            }
            return set;
        }
    }
}

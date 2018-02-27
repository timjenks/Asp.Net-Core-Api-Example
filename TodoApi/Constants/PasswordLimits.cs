using Microsoft.AspNetCore.Identity;

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
        /// Password options to use in Asp.Net Identity.
        /// </summary>
        public static PasswordOptions PasswordSettings = new PasswordOptions()
        {
            RequiredLength = AccountMinPasswordLength,
            RequiredUniqueChars = 4,
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true
        };
    }
}

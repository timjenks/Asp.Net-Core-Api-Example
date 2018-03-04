using System.ComponentModel.DataAnnotations;
using TodoApi.Constants;

namespace TodoApi.Models.ViewModels
{
    /// <summary>
    /// The expected attributes when a user authenticates.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The email of a user.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.AccountEmailIsRequired)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The password of a user.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.AccountPasswordIsRequired)]
        [StringLength(
            PasswordLimits.AccountMaxPasswordLength,
            ErrorMessage = ErrorMessages.AccountPasswordInvalidLength,
            MinimumLength = PasswordLimits.AccountMinPasswordLength)]
        [DataType(DataType.Password)] //PasswordLimits.PasswordSettings
        public string Password { get; set; }
    }
}

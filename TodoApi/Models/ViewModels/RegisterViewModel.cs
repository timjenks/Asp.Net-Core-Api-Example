using System.ComponentModel.DataAnnotations;
using TodoApi.Constants;

namespace TodoApi.Models.ViewModels
{
    /// <summary>
    /// The expected attributes when creating a new user.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// The users full name.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.UserNameRequired)]
        [MaxLength(UserLimits.NameMaxLength, ErrorMessage = ErrorMessages.UserNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// The users email.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.AccountEmailIsRequired)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The users password.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.AccountPasswordIsRequired)]
        [StringLength(
            PasswordLimits.AccountMaxPasswordLength,
            ErrorMessage = ErrorMessages.AccountPasswordInvalidLength,
            MinimumLength = PasswordLimits.AccountMinPasswordLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

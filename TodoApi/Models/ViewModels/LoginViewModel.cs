﻿#region Imports

using System.ComponentModel.DataAnnotations;
using TodoApi.Utils.Constants;

#endregion

namespace TodoApi.Models.ViewModels
{
    /// <summary>
    /// The expected attributes when a user authenticates.
    /// </summary>
    public class LoginViewModel
    {
        #region Fields

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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}

namespace TodoApi.Utils.Constants
{
    /// <summary>
    /// A static wrapper for error messages used in view models.
    /// </summary>
    public static class ErrorMessages
    {
        #region Todo

        /// <summary>
        /// When todo due date is missing.
        /// </summary>
        public const string TodoDueRequired = "Todo due date is required";

        /// <summary>
        /// When todo description is missing.
        /// </summary>
        public const string TodoDescriptionRequired = "Todo description is required";

        /// <summary>
        /// When todo description is too long.
        /// </summary>
        public const string TodoDescriptionMaxLength = 
            "Todo description can be of length " 
            + TodoLimits.DescriptionMaxLengthString 
            + " at most";

        /// <summary>
        /// When todo id is missing.
        /// </summary>
        public const string TodoIdRequired = "Todo id is required";

        #endregion

        #region Account

        /// <summary>
        /// When email is missing in login or register.
        /// </summary>
        public const string AccountEmailIsRequired = "Email is required";

        /// <summary>
        /// When password is missing in login or register.
        /// </summary>
        public const string AccountPasswordIsRequired = "Password is required";

        /// <summary>
        /// When password has invalid length.
        /// </summary>
        public const string AccountPasswordInvalidLength =
            "Password must be of length between "
            + PasswordLimits.AccountMinPasswordLengthString
            + " and "
            + PasswordLimits.AccountMaxPasswordLengthString;

        #endregion

        #region User

        /// <summary>
        /// When name of user is missing.
        /// </summary>
        public const string UserNameRequired = "Name is required";

        /// <summary>
        /// When user name (not username) is too long.
        /// </summary>
        public const string UserNameMaxLength =
            "Names can be of length "
            + UserLimits.NameMaxLengthString
            + " at most";

        #endregion
    }
}

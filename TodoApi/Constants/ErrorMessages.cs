namespace TodoApi.Constants
{
    /// <summary>
    /// A static wrapper for error messages used in view models.
    /// </summary>
    public static class ErrorMessages
    {
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

        public const string TodoIdRequired = "Todo id is required";
    }
}

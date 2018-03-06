namespace TodoApi.Utils.Constants
{
    /// <summary>
    /// A static collection of names of roles.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Person who can view and remove other users.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Person who can view, create, edit and remove todos.
        /// </summary>
        public const string User = "User";
    }
}

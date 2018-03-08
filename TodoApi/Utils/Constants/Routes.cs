namespace TodoApi.Utils.Constants
{
    /// <summary>
    /// A static class containing routes prefixes. This is done to simplify
    /// support for multiple versions, so that each branch can change the
    /// version at one place.
    /// </summary>
    public static class Routes
    {
        /// <summary>
        /// The current version.
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// The route prefix for all controllers.
        /// </summary>
        private const string VersionPrefix = "api/" + Version + "/";

        /// <summary>
        /// The route prefix for TodoController.
        /// </summary>
        public const string TodoRoute = VersionPrefix + "todo";

        /// <summary>
        /// The route prefix for AccountController.
        /// </summary>
        public const string AccountRoute = VersionPrefix + "account";

        /// <summary>
        /// The route prefix for UserController.
        /// </summary>
        public const string UserRoute = VersionPrefix + "user";

        /// <summary>
        /// The route for Swagger documentation.
        /// </summary>
        public const string SwaggerRoute = "/swagger/" + Version + "/swagger.json";
    }
}
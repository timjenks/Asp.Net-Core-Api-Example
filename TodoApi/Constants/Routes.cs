namespace TodoApi.Constants
{
    /// <summary>
    /// A static class containing routes prefixes. This is done to simplify
    /// support for multiple versions, so that each branch can change the
    /// version at one place.
    /// </summary>
    public class Routes
    {
        /// <summary>
        /// The route prefix for all controllers.
        /// </summary>
        private const string VersionPrefix = "api/v1/";

        /// <summary>
        /// The route prefix for TodoController.
        /// </summary>
        public const string TodoRoute = VersionPrefix + "todo";
    }
}

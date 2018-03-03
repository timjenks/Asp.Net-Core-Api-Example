using Microsoft.AspNetCore.Identity;

namespace Tests.MockData.EntityModels
{
    /// <summary>
    /// Static collection of roles for mock database.
    /// </summary>
    public static class MockRoles
    {
        /// <summary>
        /// The admin role.
        /// </summary>
        public static readonly IdentityRole Admin = new IdentityRole
        {
            Id = "314",
            ConcurrencyStamp = null,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        /// <summary>
        /// The user role.
        /// </summary>
        public static readonly IdentityRole User = new IdentityRole
        {
            Id = "2718",
            ConcurrencyStamp = null,
            Name = "User",
            NormalizedName = "USER"
        };
    }
}

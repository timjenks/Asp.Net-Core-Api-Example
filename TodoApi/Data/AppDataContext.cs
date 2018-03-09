#region Imports

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.EntityModels;

#endregion

namespace TodoApi.Data
{
    /// <inheritdoc />
    /// <summary>
    /// The DbContext instance and database session.
    /// </summary>
    public class AppDataContext : IdentityDbContext<ApplicationUser>
    {
        #region Fields

        /// <summary>
        /// All todos in database.
        /// </summary>
        public DbSet<Todo> Todo { get; set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// A constructor that allows for injecting SQL Server or in-memory SQlite options.
        /// </summary>
        public AppDataContext(DbContextOptions options) : base(options) { }

        #endregion

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

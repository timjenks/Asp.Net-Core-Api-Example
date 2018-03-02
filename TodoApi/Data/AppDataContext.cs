using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.EntityModels;

namespace TodoApi.Data
{
    /// <inheritdoc />
    /// <summary>
    /// The DbContext instance and database session.
    /// </summary>
    public class AppDataContext : IdentityDbContext<ApplicationUser>
    {
        /// <inheritdoc />
        /// <summary>
        /// A constructor that allows for injecting SQL Server or in-memory SQlite options.
        /// </summary>
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        /// <summary>
        /// All todos in database.
        /// </summary>
        public DbSet<Todo> Todo { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

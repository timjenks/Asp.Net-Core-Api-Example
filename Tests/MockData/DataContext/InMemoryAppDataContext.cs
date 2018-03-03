using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoApi.Data;

namespace Tests.MockData.DataContext
{
    /// <inheritdoc />
    /// <summary>
    /// An DbContext for testing, using in-memory sqlite.
    /// </summary>
    public sealed class InMemoryAppDataContext : AppDataContext
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor for in memory database.
        /// </summary>
        public InMemoryAppDataContext() : base(GetInMemoryOptions())
        {
            Database.OpenConnection();
            Database.EnsureCreated();
        }

        // TODO:
        // Create methods to add collections of mock data

        /// <summary>
        /// A option creator, that sets our context to a in-memory sqlite database. 
        /// It is kept static to pass to the parent class in our constructor.
        /// </summary>
        /// <returns>DbContext options with in memory settings</returns>
        private static DbContextOptions<AppDataContext> GetInMemoryOptions()
        {
            var connectionString = new SqliteConnectionStringBuilder { DataSource = ":memory:" }.ToString();
            var connection = new SqliteConnection(connectionString);
            var builder = new DbContextOptionsBuilder<AppDataContext>();
            return builder.UseSqlite(connection).Options;
        }
    }
}





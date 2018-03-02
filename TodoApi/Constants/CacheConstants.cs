using Microsoft.Extensions.Caching.Memory;
using System;

namespace TodoApi.Constants
{
    /// <summary>
    /// A static container of keys for cache memory and other settings.
    /// </summary>
    public static class CacheConstants
    {
        /// <summary>
        /// The cache key prefix for all todos.
        /// </summary>
        private const string AllTodosCacheKey = "c_todos";

        /// <summary>
        /// The cache key prefix for a single todo.
        /// </summary>
        private const string SingleTodoCacheKey = "c_todo";

        /// <summary>
        /// The cache key for all users.
        /// </summary>
        public const string AllUsersCacheKey = "c_users";

        /// <summary>
        /// The cache key prefix for a single user.
        /// </summary>
        private const string SingleUserCacheKey = "c_user";

        /// <summary>
        /// The cache key of all todos owned by a user.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The cache memory key owned for all todos owned by a user</returns>
        public static string GetAllTodosCacheKey(string userId)
        {
            return string.Format("{0}_{1}", AllTodosCacheKey, userId);
        }

        /// <summary>
        /// The cache key of all todos owned by a user on a given day.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="date">The date of the todos</param>
        /// <returns>The cache memory key for owned todos on a date</returns>
        public static string GetAllTodosForDayCacheKey(string userId, DateTime date)
        {
            return string.Format("{0}_{1}_{2}_{3}", 
                GetAllTodosCacheKey(userId), date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// The cache key of a single todo.
        /// </summary>
        /// <param name="todoId">The id of an todo</param>
        /// <param name="userId">The id of the owner</param>
        /// <returns>The cache memory key for the todo with given id</returns>
        public static string GetSingleTodoCacheKey(int todoId, string userId)
        {
            return string.Format("{0}_{1}_{2}", SingleTodoCacheKey, todoId, userId);
        }

        /// <summary>
        /// The cache key of a single user.
        /// </summary>
        /// <param name="userId">The id of an user</param>
        /// <returns>the cache memory key for the user with given id</returns>
        public static string GetSingleUserCacheKey(string userId)
        {
            return string.Format("{0}_{1}", SingleUserCacheKey, userId);
        }

        /// <summary>
        /// Create and return cache options with the default life span.
        /// </summary>
        /// <returns>Options with default life span</returns>
        public static MemoryCacheEntryOptions GetDefaultCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));
        }
    }
}

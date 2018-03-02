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
        /// The cache key for all todos.
        /// </summary>
        public const string AllTodosCacheKey = "c_todos";

        /// <summary>
        /// The cache key prefix for a single todo.
        /// </summary>
        private const string SingleTodoCacheKey = "c_todo";

        /// <summary>
        /// TODO
        /// </summary>
        private const string AllUsersCacheKey = "c_users";

        /// <summary>
        /// TODO
        /// </summary>
        private const string SingleUserCacheKey = "c_user";

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        public static string GetAllTodosCacheKey(string userId)
        {
            return string.Format("{0}_{1}", AllTodosCacheKey, userId);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <param name="date">TODO</param>
        /// <returns>TODO</returns>
        public static string GetAllTodosForDayCacheKey(string userId, DateTime date)
        {
            return string.Format("{0}_{1}_{2}_{3}", 
                GetAllTodosCacheKey(userId), date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// The cache key of a single todo.
        /// </summary>
        /// <param name="id">The id of an todo</param>
        /// <param name="userId">The id of the owner</param>
        /// <returns>The cache memory key for the todo with given id</returns>
        public static string GetSingleTodoCacheKey(int id, string userId)
        {
            return string.Format("{0}_{1}_{2}", SingleTodoCacheKey, id, userId);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        public static string GetSingleUserCacheKey(string userId)
        {
            return string.Format("{0}_{1}", SingleUserCacheKey, userId);
        }

        /// <summary>
        /// The default timeout of a cache memory entry.
        /// </summary>
        public const int DefaultCacheTimeoutHours = 1;

        /// <summary>
        /// Create and return cache options with the default life span.
        /// </summary>
        /// <returns>Options with life span</returns>
        public static MemoryCacheEntryOptions GetDefaultCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(DefaultCacheTimeoutHours));
        }
    }
}

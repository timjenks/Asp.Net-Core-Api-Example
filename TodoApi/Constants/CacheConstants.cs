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
        /// The cache key of a single todo.
        /// </summary>
        /// <param name="id">The id of an todo</param>
        /// <returns>The cache memory key for the todo with given id</returns>
        public static string GetSingleTodoCacheKey(int id)
        {
            return SingleTodoCacheKey + id;
        }

        /// <summary>
        /// The default timeout of a cache memory entry.
        /// </summary>
        public static readonly TimeSpan DefaultCacheTimeout = TimeSpan.FromHours(2);

        /// <summary>
        /// Create and return cache options with the default life span.
        /// </summary>
        /// <returns>Options with life span</returns>
        public static MemoryCacheEntryOptions GetDefaultCacheOptions()
        {
            return new MemoryCacheEntryOptions().SetSlidingExpiration(DefaultCacheTimeout);
        }
    }
}

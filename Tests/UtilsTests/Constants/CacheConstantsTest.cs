#region Imports

using System;
using TodoApi.Utils.Constants;
using Xunit;

#endregion

namespace Tests.UtilsTests.Constants
{
    /// <summary>
    /// Tests for cache key getters.
    /// </summary>
    public class CacheConstantsTest
    {
        [Fact]
        public void GetAllTodosCacheKey_WithUuid_Matches()
        {
            // Arrange
            const string id = "79744ca4-c450-40ed-8ea4-daf0569b70e1";

            // Act
            var cacheKey = CacheConstants.GetAllTodosCacheKey(id);

            // Assert
            Assert.Equal("c_todos_79744ca4-c450-40ed-8ea4-daf0569b70e1", cacheKey);
        }

        [Fact]
        public void GetAllTodosForDayCacheKey_WithUuidAndTodoId_Matches()
        {
            // Arrange
            const string id = "7d02b709-d300-44e5-90a7-39fc233d0ca9";
            var day = new DateTime(1999, 2, 10, 4, 22, 1);

            // Act
            var cacheKey = CacheConstants.GetAllTodosForDayCacheKey(id, day);

            // Assert
            Assert.Equal("c_todos_7d02b709-d300-44e5-90a7-39fc233d0ca9_1999_2_10", cacheKey);
        }

        [Fact]
        public void GetSingleTodoCacheKey_WithUuidAndTodoId_Matches()
        {
            // Arrange
            const string userId = "f7b3d1d1-4c9c-43d3-a929-a6d06d49885c";
            const int todoId = 55;
            
            // Act
            var cacheKey = CacheConstants.GetSingleTodoCacheKey(todoId, userId);

            // Assert
            Assert.Equal("c_todo_55_f7b3d1d1-4c9c-43d3-a929-a6d06d49885c", cacheKey);
        }

        [Fact]
        public void GetSingleUserCacheKey_WithUuid_Matches()
        {
            // Arrange
            const string userId = "31f4a428-33b7-45c3-a886-a63d93543166";

            // Act
            var cacheKey = CacheConstants.GetSingleUserCacheKey(userId);

            // Assert
            Assert.Equal("c_user_31f4a428-33b7-45c3-a886-a63d93543166", cacheKey);
        }

        [Fact]
        public void GetDefaultCacheOptions_HasCorrectExpireTime()
        {
            // Arrange
            const int expecteSeconds = 3600;

            // Act
            var options = CacheConstants.GetDefaultCacheOptions();
            var lifeSpan = options?.SlidingExpiration?.TotalSeconds;

            // Assert
            Assert.NotNull(lifeSpan);
            Assert.Equal(expecteSeconds, lifeSpan);
        }
    }
}
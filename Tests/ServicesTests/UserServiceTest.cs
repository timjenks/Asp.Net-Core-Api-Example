#region Imports

using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using TodoApi.Services;
using Xunit;

#endregion

namespace Tests.ServicesTests
{
    /// <summary>
    /// Testing UserService with in memory sqlite database and mock user manager.
    /// </summary>
    public class UserServiceTest
    {
        #region Fields

        private readonly UserService _service;
        private readonly AppDataContext _ctx;

        #endregion

        #region Constructors

        /// <summary>
        /// Before each.
        /// </summary>
        public UserServiceTest()
        {
            _ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(_ctx);
            _service = new UserService(_ctx, userManager, new MemoryCache(new MemoryCacheOptions()));
        }

        #endregion

        #region GetUser

        [Fact]
        public async Task GetUserByIdAsync_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            const string unknownId = "45788fc6-03b9-4285-b3cf-47599921dcc4";

            // Act
            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.GetUserByIdAsync(unknownId));
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_MatchingUserDto()
        {
            // Arrange
            var userToFind = MockApplicationUsers.Get(9);
            var id = userToFind.Id;

            // Act
            var dto = await _service.GetUserByIdAsync(id);

            // Assert
            Assert.Equal(userToFind.Id, dto.Id);
            Assert.Equal(userToFind.Email, dto.Email);
            Assert.Equal(userToFind.Name, dto.Name);
        }

        #endregion

        #region GetAllUsers

        [Fact]
        public async Task GetAllUsersOrderedByNameAsync_AllInMockAndInOrder()
        {
            // Arrange
            var allIds = MockApplicationUsers.GetAll().Select(w => w.Id).ToHashSet();

            // Act
            var allUsers = (await _service.GetAllUsersOrderedByNameAsync()).ToArray();
            var dtoIds = allUsers.Select(w => w.Id).ToHashSet();

            // Assert
            Assert.Equal(allIds.Count, dtoIds.Count);
            foreach (var id in dtoIds)
            {
                Assert.Contains(id, allIds);
            }
            var first = true;
            ApplicationUserDto last = null;
            foreach (var dto in allUsers)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Assert.True(string.Compare(dto.Name, last.Name, StringComparison.Ordinal) >= 0);
                }
                last = dto;
            }
        }

        #endregion

        #region Remove

        [Fact]
        public async Task RemoveUserByIdAsync_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            const string unknownId = "45788fc6-03b9-4285-b3cf-47599921dcc4";

            // Act
            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.RemoveUserByIdAsync(unknownId));
        }

        [Fact]
        public async Task RemoveUserByIdAsync_ExistingUser_UserAndAllTodosRemoved()
        {
            // Arrange
            var userToRemove = MockApplicationUsers.Get(0);
            var userId = userToRemove.Id;
            var userFoundBefore = _ctx.Users.SingleOrDefault(w => w.Id == userId) != null;
            var todosToRemove = _ctx.Todo.Where(w => w.Owner.Id == userToRemove.Id).ToList();

            // Act
            await _service.RemoveUserByIdAsync(userId);

            // Assert
            Assert.True(userFoundBefore);
            Assert.NotEmpty(todosToRemove);
            Assert.Null(_ctx.Users.SingleOrDefault(w => w.Id == userId));
            foreach (var todo in todosToRemove)
            {
                Assert.Null(_ctx.Todo.SingleOrDefault(w => w.Id == todo.Id));
            }
        }

        #endregion
    }
}

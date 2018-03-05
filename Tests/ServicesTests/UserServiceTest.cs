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

namespace Tests.ServicesTests
{
    /// <summary>
    /// Testing UserService with in memory sqlite database,
    /// mock config, user manager and sign in manager.
    /// </summary>
    public class UserServiceTest
    {
        private readonly UserService _service;
        private readonly AppDataContext _ctx;

        public UserServiceTest()
        {
            _ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(_ctx);
            _service = new UserService(_ctx, userManager, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            var unknownId = "45788fc6-03b9-4285-b3cf-47599921dcc4";

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

        [Fact]
        public async Task GetAllUsersOrderedByNameAsync_AllInMockAndInOrder()
        {
            // Arrange
            var allIds = MockApplicationUsers.GetAll().Select(w => w.Id).ToHashSet();

            // Act
            var allUsers = await _service.GetAllUsersOrderedByNameAsync();
            var dtoIds = allUsers.Select(w => w.Id).ToHashSet();

            // Assert
            Assert.Equal(allIds.Count(), dtoIds.Count());
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
                    Assert.True(dto.Name.CompareTo(last.Name) >= 0);
                }
                last = dto;
            }
        }
    }
}

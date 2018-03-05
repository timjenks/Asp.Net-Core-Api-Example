using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Services;
using Xunit;

namespace Tests.ServicesTests
{
    /// <summary>
    /// Testing TodoService with in memory sqlite database and mock user manager.
    /// </summary>
    public class TodoServiceTest
    {
        private readonly TodoService _service;
        private readonly AppDataContext _ctx;

        /// <summary>
        /// Before each.
        /// </summary>
        public TodoServiceTest()
        {
            _ctx = new InMemoryAppDataContext();
            var userManager = new MockUserManager(_ctx);
            _service = new TodoService(_ctx, userManager, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async Task GetTodoByIdAsync_NonExistingTodo_TodoNotFoundException()
        {
            // Arrange
            var nonExistingId = 0;
            var userId = MockApplicationUsers.Get(4).Id;

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _service.GetTodoByIdAsync(nonExistingId, userId));
        }

        [Fact]
        public async Task GetTodoByIdAsync_NonExistingUser_TodoNotFoundException()
        {
            // Arrange
            var todoId = MockTodos.Get(7).Id;
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _service.GetTodoByIdAsync(todoId, userId));
        }

        [Fact]
        public async Task GetTodoByIdAsync_UserDoesNotOwnTodo_TodoNotFoundException()
        {
            // Arrange
            var todo = MockTodos.Get(2);
            var user = MockApplicationUsers.GetAll().Where(w => w.Id != todo.Owner.Id).FirstOrDefault();

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(
                () => _service.GetTodoByIdAsync(todo.Id, user.Id));
        }

        [Fact]
        public async Task GetTodoByIdAsync_UserOwnsTodo_TodoDto()
        {
            // Arrange
            var todo = MockTodos.Get(5);
            var user = MockApplicationUsers.GetAll().Where(w => w.Id == todo.Owner.Id).SingleOrDefault();

            // Act
            var dto = await _service.GetTodoByIdAsync(todo.Id, user.Id);

            // Assert
            Assert.Equal(todo.Id, dto.Id);
            Assert.Equal(todo.Description, dto.Description);
            Assert.Equal(todo.Due, dto.Due);
        }
    }
}

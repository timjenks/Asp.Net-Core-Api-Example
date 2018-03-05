using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
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

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_MissingYearInFilter_AllDtosForOwner()
        {
            // Arrange
            var owner = MockApplicationUsers.Get(9);
            var todosOwnedByOwner = _ctx.Todo.Where(w => w.Owner.Id == owner.Id).Select(z => z.Id).ToHashSet();

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync(null, "5", "1", owner.Id);

            // Assert
            Assert.Equal(todosOwnedByOwner.Count(), all.Count());
            foreach (var dto in all)
            {
                Assert.Contains(dto.Id, todosOwnedByOwner);
            }
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_MissingMonthInFilter_AllDtosForOwner()
        {
            // Arrange
            var owner = MockApplicationUsers.Get(9);
            var todosOwnedByOwner = _ctx.Todo.Where(w => w.Owner.Id == owner.Id).Select(z => z.Id).ToHashSet();

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync("1999", null, "1", owner.Id);

            // Assert
            Assert.Equal(todosOwnedByOwner.Count(), all.Count());
            foreach (var dto in all)
            {
                Assert.Contains(dto.Id, todosOwnedByOwner);
            }
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_MissingDayInFilter_AllDtosForOwner()
        {
            // Arrange
            var owner = MockApplicationUsers.Get(9);
            var todosOwnedByOwner = _ctx.Todo.Where(w => w.Owner.Id == owner.Id).Select(z => z.Id).ToHashSet();

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync("1999", "11", null, owner.Id);

            // Assert
            Assert.Equal(todosOwnedByOwner.Count(), all.Count());
            foreach (var dto in all)
            {
                Assert.Contains(dto.Id, todosOwnedByOwner);
            }
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_InvalidFilter_EmptyList()
        {
            // Arrange
            var owner = MockApplicationUsers.Get(9);

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync("2002", "2", "29", owner.Id);

            // Assert
            Assert.Empty(all);
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_ValidFilter_OnlyWithGivenDates()
        {
            // Arrange
            var owner = MockApplicationUsers.Get(9);

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync("2019", "11", "5", owner.Id);

            // Assert
            foreach (var dto in all)
            {
                Assert.Equal(2019, dto.Due.Year);
                Assert.Equal(11, dto.Due.Month);
                Assert.Equal(5, dto.Due.Day);
            }
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_NoFilterNonExistingUser_EmptyList()
        {
            // Arrange
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync(null, null, null, userId);

            // Assert
            Assert.Empty(all);
        }

        [Fact]
        public async Task GetAllTodosOrderedByDueAsync_ValidFilterNonExistingUser_EmptyList()
        {
            // Arrange
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";

            // Act
            var all = await _service.GetAllTodosOrderedByDueAsync("2000", "5", "20", userId);

            // Assert
            Assert.Empty(all);
        }

        [Fact]
        public async Task CreateTodoAsync_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";
            var model = new CreateTodoViewModel
            {
                Description = "Buy a pie from Frank Pepe's",
                Due = DateTime.Now
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.CreateTodoAsync(model, userId));
        }

        [Fact]
        public async Task CreateTodoAsync_ExistingUser_IdOfNewlyCreatedTodo()
        {
            // Arrange
            var userId = MockApplicationUsers.Get(4).Id;
            var model = new CreateTodoViewModel
            {
                Description = "Get a cone from Milkcraft",
                Due = new DateTime(2019, 5, 1, 12, 0, 0)
            };
            var expectedId = MockTodos.FirstId + MockTodos.GetAll().Count();

            // Act
            var id = await _service.CreateTodoAsync(model, userId);
            var todo = _ctx.Todo.Where(z => z.Id == expectedId && z.Owner.Id == userId).SingleOrDefault();

            // Assert
            Assert.Equal(expectedId, id);
            Assert.NotNull(todo);
            Assert.Equal("Get a cone from Milkcraft", todo.Description);
            Assert.Equal(new DateTime(2019, 5, 1, 12, 0, 0), todo.Due);
        }

        [Fact]
        public async Task RemoveTodoByIdAsync_NonExistingTodo_TodoNotFoundException()
        {
            // Arrange
            var userId = MockApplicationUsers.Get(0).Id;
            var todoId = 0;

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(() => _service.RemoveTodoByIdAsync(todoId, userId));
        }

        [Fact]
        public async Task RemoveTodoByIdAsync_NonExistingUser_TodoNotFoundException()
        {
            // Arrange
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";
            var todoId = MockTodos.Get(3).Id;

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(() => _service.RemoveTodoByIdAsync(todoId, userId));
        }

        [Fact]
        public async Task RemoveTodoByIdAsync_ValidUserAndTodo_Removed()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var todo = _ctx.Todo.Where(z => z.Owner.Id == user.Id).FirstOrDefault();
            var ownedBefore = _ctx.Todo.Where(z => z.Owner.Id == user.Id).Count();
            var foundBefore = _ctx.Todo.Where(z => z.Id == todo.Id).SingleOrDefault() != null;

            // Act
            await _service.RemoveTodoByIdAsync(todo.Id, user.Id);

            // Assert
            Assert.True(foundBefore);
            Assert.Null(_ctx.Todo.Where(z => z.Id == todo.Id).SingleOrDefault());
            Assert.Equal(ownedBefore - 1, _ctx.Todo.Where(z => z.Owner.Id == user.Id).Count());
        }

        [Fact]
        public async Task EditTodoAsync_NonExistingTodo_TodoNotFoundException()
        {
            // Arrange
            var userId = MockApplicationUsers.Get(0).Id;
            var model = new EditTodoViewModel
            {
                Id = 0,
                Due = DateTime.Now,
                Description = "Take to the sewers, in a clown costume"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(() => _service.EditTodoAsync(model, userId));
        }

        [Fact]
        public async Task EditTodoAsync_NonExistingUser_TodoNotFoundException()
        {
            // Arrange
            var userId = "c54a85fa-ca7c-49d7-b830-6b07ea49cfa8";
            var todo = MockTodos.Get(3);
            var model = new EditTodoViewModel
            {
                Id = todo.Id,
                Due = todo.Due,
                Description = todo.Description
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(() => _service.EditTodoAsync(model, userId));
        }

        [Fact]
        public async Task EditTodoAsync_ValidUserAndTodo_Edited()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var todo = _ctx.Todo.Where(z => z.Owner.Id == user.Id).FirstOrDefault();
            var model = new EditTodoViewModel
            {
                Id = todo.Id,
                Description = "Catch Mr. X",
                Due = new DateTime(2020, 1, 1, 0, 0, 0)
            };

            // Act
            await _service.EditTodoAsync(model, user.Id);
            var changedTodo = _ctx.Todo.Where(z => z.Id == todo.Id).SingleOrDefault();

            // Assert
            Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 0), changedTodo.Due);
            Assert.Equal("Catch Mr. X", changedTodo.Description);
        }
    }
}

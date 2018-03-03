using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models.DtoModels;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;

namespace Tests.MockData.Services
{
    /// <inheritdoc />
    /// <summary>
    /// A mock of todo service.
    /// </summary>
    public class MockTodoService : ITodoService
    {
        /// <summary>
        /// A method to control what CreateTodoAsync does. Needs to be set in test.
        /// </summary>
        public Func<CreateTodoViewModel, string, int> MCreateTodoAsync { get; set; }

        /// <summary>
        /// A method to control what EditTodoAsync does. Needs to be set in test.
        /// </summary>
        public Action<EditTodoViewModel, string> MEditTodoAsync { get; set; }

        /// <summary>
        /// A method to control what GetAllTodosOrderedByDueAsync does. Needs to be set in test.
        /// </summary>
        public Func<string, string, string, string, IEnumerable<TodoDto>> MGetAllTodosOrderedByDueAsync { get; set; }

        /// <summary>
        /// A method to control what GetTodoByIdAsync does. Needs to be set in test.
        /// </summary>
        public Func<int, string, TodoDto> MGetTodoByIdAsync { get; set; }

        /// <summary>
        /// A method to control what RemoveTodoByIdAsync does. Needs to be set in test.
        /// </summary>
        public Action<int, string> MRemoveTodoByIdAsync { get; set; }

        /// <inheritdoc />
        public async Task<int> CreateTodoAsync(CreateTodoViewModel todo, string userId)
        {
            await Task.Run(() => { });
            return MCreateTodoAsync(todo, userId);
        }

        /// <inheritdoc />
        public async Task EditTodoAsync(EditTodoViewModel model, string userId)
        {
            await Task.Run(() => { });
            MEditTodoAsync(model, userId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueAsync(string year, string month, string day, string userId)
        {
            await Task.Run(() => { });
            return MGetAllTodosOrderedByDueAsync(year, month, day, userId);
        }

        /// <inheritdoc />
        public async Task<TodoDto> GetTodoByIdAsync(int todoId, string userId)
        {
            await Task.Run(() => { });
            return MGetTodoByIdAsync(todoId, userId);
        }

        /// <inheritdoc />
        public async Task RemoveTodoByIdAsync(int todoId, string userId)
        {
            await Task.Run(() => { });
            MRemoveTodoByIdAsync(todoId, userId);
        }
    }
}

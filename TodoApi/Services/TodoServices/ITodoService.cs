using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models.DtoModels;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.TodoServices
{
    /// <summary>
    /// An interface for a todo service that defines 
    /// what methods such a service must implement.
    /// </summary>
    public interface ITodoService
    {
        /// <summary>
        /// Get a single todo.
        /// </summary>
        /// <param name="id">The id of said todo</param>
        /// <returns>The todo dto corresponding to todo with given id</returns>
        Task<TodoDto> GetTodoByIdAsync(int id, string userId);

        /// <summary>
        /// Get all todos, possibly filtered by date, ordered by start time.
        /// </summary>
        /// <param name="year">Filter year as string</param>
        /// <param name="month">Filter month as string (1-12)</param>
        /// <param name="day">Filter day as string (1-31)</param>
        /// <returns>A list of all todos as dto, possibly filtered by date and ordered by due time</returns>
        Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueAsync(
            string year, string month, string day, string userId);

        /// <summary>
        /// Create a todo.
        /// </summary>
        /// <param name="todo">A model with attributes needed to create new todo</param>
        /// <returns>The id of the newly created todo</returns>
        Task<int> CreateTodoAsync(CreateTodoViewModel todo, string userId);

        /// <summary>
        /// Remmve a specific todo.
        /// </summary>
        /// <param name="id">The id of the todo to remove</param>
        Task RemoveTodoByIdAsync(int id, string userId);

        /// <summary>
        /// Edit a specific todo.
        /// </summary>
        /// <param name="model">A view model containing the info needed for editing an todo</param>
        Task EditTodoAsync(EditTodoViewModel model, string userId);

    }
}

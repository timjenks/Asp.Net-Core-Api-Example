using System.Threading.Tasks;
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
        /// Create a todo.
        /// </summary>
        /// <param name="todo">A model with attributes needed to create new todo</param>
        /// <returns>The id of the newly created todo</returns>
        Task<int> CreateTodoAsync(CreateTodoViewModel todo);

    }
}

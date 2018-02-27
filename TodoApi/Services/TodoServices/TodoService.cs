using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models.EntityModels;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.TodoServices
{
    /// <inheritdoc />
    /// <summary>
    /// The todo service that the production API uses.
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly AppDataContext _db;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// A constructor that injects AppDataContext and MemoryCache.
        /// </summary>
        /// <param name="db">A DbContext to access a database</param>
        /// <param name="cache">A cache memory to utilize RAM to save db queries</param>
        public TodoService(AppDataContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<int> CreateTodoAsync(CreateTodoViewModel todo)
        {
            var newTodo = new Todo
            {
                Due = todo.Due.Value,
                Description = todo.Description
            };
            await _db.AddAsync(newTodo);
            await _db.SaveChangesAsync();
            return newTodo.Id;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using TodoApi.Models.EntityModels;
using TodoApi.Models.ViewModels;
using TodoApi.Utils.TimeUtils;

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
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            var todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                throw new TodoNotFoundException();
            }
            return new TodoDto(todo);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueAsync(string year, string month, string day)
        {
            // If any of the parameters is null, they are ignored.
            if (year == null || month == null || day == null)
            {
                return await GetAllTodosOrderedByDueWithNoFilterAsync();
            }
            // if date creation was unsuccesful, we return an empty list
            var date = QueryDateBuilder.CreateDate(year, month, day);
            return date == null ? new List<TodoDto>() : await GetAllTodosForDayOrderedByDueAsync(date.Value);
        }

        /// <inheritdoc />
        public async Task<int> CreateTodoAsync(CreateTodoViewModel todo)
        {
            var newTodo = new Todo(todo);
            await _db.AddAsync(newTodo);
            await _db.SaveChangesAsync();
            return newTodo.Id;
        }

        /// <inheritdoc />
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task RemoveTodoByIdAsync(int id)
        {
            var todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                throw new TodoNotFoundException();
            }
            _db.Remove(todo);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc />
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task EditTodoAsync(EditTodoViewModel model)
        {
            var todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (todo == null)
            {
                throw new TodoNotFoundException();
            }
            todo.Edit(model);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Returns all todos with no filtering.
        /// </summary>
        /// <returns>List of todos</returns>
        private async Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueWithNoFilterAsync()
        {
            return await (from t in _db.Todo
                          orderby t.Due
                          select new TodoDto(t)).ToListAsync();
        }

        /// <summary>
        /// Filters the list by a date (ignoring time).
        /// </summary>
        /// <param name="date">Valid date to filter by</param>
        /// <returns>List of todos</returns>
        private async Task<IEnumerable<TodoDto>> GetAllTodosForDayOrderedByDueAsync(DateTime date)
        {
            return await (from t in _db.Todo
                          where t.Due.Date == date
                          orderby t.Due
                          select new TodoDto(t)).ToListAsync();
        }

    }
}

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
using Microsoft.AspNetCore.Identity;
using TodoApi.Services.Interfaces;
using TodoApi.Constants;

namespace TodoApi.Services
{

    /// <inheritdoc />
    /// <summary>
    /// The todo service that the production API uses.
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly AppDataContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// A constructor that injects AppDataContext and MemoryCache.
        /// </summary>
        /// <param name="db">A DbContext to access a database</param>
        /// <param name="userManager">TODO</param>
        /// <param name="cache">A cache memory to utilize RAM to save db queries</param>
        public TodoService
        (
            AppDataContext db,
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache
        )
        {
            _db = db;
            _userManager = userManager;
            _cache = cache;
        }

        /// <inheritdoc />
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task<TodoDto> GetTodoByIdAsync(int id, string userId)
        {
            var cacheKey = CacheConstants.GetSingleTodoCacheKey(id, userId);
            if (!_cache.TryGetValue(cacheKey, out Todo todo))
            {
                todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == id && t.Owner.Id == userId);
                if (todo == null)
                {
                    throw new TodoNotFoundException();
                }
                _cache.Set(cacheKey, todo, CacheConstants.GetDefaultCacheOptions());
            }
            return new TodoDto(todo);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueAsync(
            string year, string month, string day, string userId)
        {
            // If any of the parameters is null, they are ignored.
            if (year == null || month == null || day == null)
            {
                return await GetAllTodosOrderedByDueWithNoFilterAsync(userId);
            }
            // if date creation was unsuccesful, we return an empty list
            var date = QueryDateBuilder.CreateDate(year, month, day);
            return date == null ?
                new List<TodoDto>() :
                await GetAllTodosForDayOrderedByDueAsync(date.Value, userId);
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">When todo is not found</exception>
        public async Task<int> CreateTodoAsync(CreateTodoViewModel todo, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            var newTodo = new Todo(todo, user);
            await _db.AddAsync(newTodo);
            await _db.SaveChangesAsync();
            _cache.Set(CacheConstants.GetSingleTodoCacheKey(newTodo.Id, userId),
                newTodo, CacheConstants.GetDefaultCacheOptions());
            _cache.Remove(CacheConstants.GetAllTodosCacheKey(userId));
            _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(userId, newTodo.Due));
            return newTodo.Id;
        }

        /// <inheritdoc />
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task RemoveTodoByIdAsync(int id, string userId)
        {
            var todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == id && t.Owner.Id == userId);
            if (todo == null)
            {
                throw new TodoNotFoundException();
            }
            _db.Remove(todo);
            await _db.SaveChangesAsync();
            _cache.Remove(CacheConstants.GetSingleTodoCacheKey(id, userId));
            _cache.Remove(CacheConstants.GetAllTodosCacheKey(userId));
            _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(userId, todo.Due));
        }

        /// <inheritdoc />
        /// <exception cref="TodoNotFoundException">When todo is not found</exception>
        public async Task EditTodoAsync(EditTodoViewModel model, string userId)
        {
            var todo = await _db.Todo.SingleOrDefaultAsync(t => t.Id == model.Id && t.Owner.Id == userId);
            if (todo == null)
            {
                throw new TodoNotFoundException();
            }
            var oldDate = todo.Due;
            todo.Edit(model);
            await _db.SaveChangesAsync();
            _cache.Set(CacheConstants.GetSingleTodoCacheKey(todo.Id, userId),
                todo, CacheConstants.GetDefaultCacheOptions());
            _cache.Remove(CacheConstants.GetAllTodosCacheKey(userId));
            _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(userId, oldDate));
            if (oldDate.Date != todo.Due.Date)
            {
                _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(userId, todo.Due));
            }
        }

        /// <summary>
        /// Returns all todos with no filtering.
        /// </summary>
        /// <returns>List of todos</returns>
        private async Task<IEnumerable<TodoDto>> GetAllTodosOrderedByDueWithNoFilterAsync(string userId)
        {
            var cacheKey = CacheConstants.GetAllTodosCacheKey(userId);
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TodoDto> todos))
            {
                todos = await (from t in _db.Todo
                               where t.Owner.Id == userId
                               orderby t.Due
                               select new TodoDto(t)).ToListAsync();
                if (todos.Count() > 0)
                {
                    _cache.Set(cacheKey, CacheConstants.GetDefaultCacheOptions());
                }
            }
            return todos;
        }

        /// <summary>
        /// Filters the list by a date (ignoring time).
        /// </summary>
        /// <param name="date">Valid date to filter by</param>
        /// <returns>List of todos</returns>
        private async Task<IEnumerable<TodoDto>> GetAllTodosForDayOrderedByDueAsync(DateTime date, string userId)
        {
            var cacheKey = CacheConstants.GetAllTodosForDayCacheKey(userId, date);
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TodoDto> todos))
            {
                todos = await (from t in _db.Todo
                               where t.Due.Date == date && t.Owner.Id == userId
                               orderby t.Due
                               select new TodoDto(t)).ToListAsync();
                if (todos.Count() > 0)
                {
                    _cache.Set(cacheKey, CacheConstants.GetDefaultCacheOptions());
                }
            }
            return todos;
        }
    }
}

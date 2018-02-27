using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models.EntityModels;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.TodoServices
{
    public class TodoService : ITodoService
    {
        private readonly AppDataContext _db;

        public TodoService(AppDataContext db)
        {
            _db = db;
        }

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

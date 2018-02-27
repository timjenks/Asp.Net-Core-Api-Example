using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.TodoServices
{
    public interface ITodoService
    {
        Task<int> CreateTodoAsync(CreateTodoViewModel todo);

    }
}

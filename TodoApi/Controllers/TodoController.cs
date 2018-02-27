using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Constants;
using TodoApi.Models.ViewModels;
using TodoApi.Services.TodoServices;

namespace TodoApi.Controllers
{
    [Route(Routes.TodoRoute)]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // todo CreatedAt...
            var id = await _todoService.CreateTodoAsync(model);
            return CreatedAtRoute("", new { }, null);
        }
    }
}
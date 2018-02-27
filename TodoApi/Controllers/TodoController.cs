using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Constants;
using TodoApi.Models.ViewModels;
using TodoApi.Services.TodoServices;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all todo related requests.
    /// </summary>
    [Route(Routes.TodoRoute)]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        /// <summary>
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="todoService">A service that implements ITodoService</param>
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// Create a new todo.
        /// POST api/{version}/todo
        /// </summary>
        /// <param name="model">A model with necessary fields to create a todo</param>
        /// <returns>201 and GET route in header if successful, 400 otherwise</returns>
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
            return StatusCode(201); // CreatedAtRoute("", new { }, null);
        }
    }
}
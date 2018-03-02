using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;
using System.Security.Claims;

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all todo related requests.
    /// </summary>
    [Route(Routes.TodoRoute)]
    [Authorize(Roles = "Admin, User")]
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
        /// Get a specific todo given its id.
        /// GET api/{version}/todo/{id}
        /// </summary>
        /// <param name="todoId">The id of the todo requested</param>
        /// <returns>200 and todo dto if found, 404 otherwise</returns>
        [HttpGet("{todoId:int}", Name = MethodNames.GetSingleTodoMethodName)]
        public async Task<IActionResult> GetTodo(int todoId)
        {
            try
            {
                var userId = User.Claims.SingleOrDefault(c => 
                    c.Type == ClaimTypes.NameIdentifier).Value;
                return Ok(await _todoService.GetTodoByIdAsync(todoId, userId));
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returning all todos.
        /// GET api/{version}/tood
        /// </summary>
        /// <param name="year">With given due date year</param>
        /// <param name="month">With given due date month</param>
        /// <param name="day">With given due date daay</param>
        /// <returns>200 and a list of all of the todos</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllTodos(
            [FromQuery] string year = null,
            [FromQuery] string month = null,
            [FromQuery] string day = null
        )
        {
            var userId = User.Claims.SingleOrDefault(c => 
                c.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(await _todoService.GetAllTodosOrderedByDueAsync(year, month, day, userId));
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
            try
            {
                var userId = User.Claims.SingleOrDefault(c => 
                    c.Type == ClaimTypes.NameIdentifier).Value;
                var id = await _todoService.CreateTodoAsync(model, userId);
                return CreatedAtRoute(
                    MethodNames.GetSingleTodoMethodName,
                    new { todoId = id }, 
                    null
                );
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Delete a specific todo.
        /// DELETE api/{version}/todo/{id}
        /// </summary>
        /// <param name="todoId">The id of the todo requested</param>
        /// <returns>204 if successful, 404 otherwise</returns>
        [HttpDelete("{todoId:int}")]
        public async Task<IActionResult> RemoveTodo(int todoId)
        {
            try
            {
                var userId = User.Claims.SingleOrDefault(c => 
                    c.Type == ClaimTypes.NameIdentifier).Value;
                await _todoService.RemoveTodoByIdAsync(todoId, userId);
                return NoContent();
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Edit an existing todo.
        /// PUT api/{version}/dodo
        /// </summary>
        /// <param name="changedTodo">Data containing id of the todo to edit and edited fields</param>
        /// <returns>400 if invalid body, 404 if todo is not found, 200 otherwise</returns>
        [HttpPut("")]
        public async Task<IActionResult> EditTodo([FromBody] EditTodoViewModel changedTodo)
        {
            if (changedTodo == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.Claims.SingleOrDefault(c => 
                    c.Type == ClaimTypes.NameIdentifier).Value;
                await _todoService.EditTodoAsync(changedTodo, userId);
                return Ok();
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
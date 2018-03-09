﻿#region Imports

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Utils.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;
using System.Security.Claims;
using Swashbuckle.AspNetCore.SwaggerGen;
using TodoApi.Models.DtoModels;
using System.Collections.Generic;

#endregion

namespace TodoApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// A controller for all todo related requests.
    /// </summary>
    [Produces("application/json")]
    [Route(Routes.TodoRoute)]
    [Authorize(Roles = "Admin, User")]
    public class TodoController : Controller
    {
        #region Fields

        private readonly ITodoService _todoService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that injects a service.
        /// </summary>
        /// <param name="todoService">A service that implements ITodoService</param>
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        #endregion

        #region GetTodo

        /// <summary>
        /// Get a specific todo given its id.
        /// </summary>
        /// <remarks>GET api/{version}/todo/{id}</remarks>
        /// <param name="todoId">The id of the todo requested</param>
        /// <response code="200">todo received</response>
        /// <response code="401">Token required</response>
        /// <response code="404">todo requested does not exist</response>
        /// <returns>200 and todo dto if found, 404 otherwise</returns>
        [SwaggerResponse(200, typeof(TodoDto))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(404, typeof(void))]
        [HttpGet("{todoId:int}", Name = MethodNames.GetSingleTodoMethodName)]
        public async Task<IActionResult> GetTodo(int todoId)
        {
            try
            {
                return Ok(await _todoService.GetTodoByIdAsync(todoId, GetUserId()));
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion

        #region GetAllTodos

        /// <summary>
        /// Returning all todos.
        /// </summary>
        /// <remarks>GET api/{version}/tood</remarks>
        /// <param name="year">With given due date year</param>
        /// <param name="month">With given due date month</param>
        /// <param name="day">With given due date daay</param>
        /// <response code="200">todos received</response>
        /// <response code="401">Token required</response>
        /// <returns>200 and a list of all of the todos</returns>
        [SwaggerResponse(200, typeof(IEnumerable<TodoDto>))]
        [SwaggerResponse(401, typeof(void))]
        [HttpGet("")]
        public async Task<IActionResult> GetAllTodos(
            [FromQuery] string year = null,
            [FromQuery] string month = null,
            [FromQuery] string day = null
        )
        {
            return Ok(await _todoService.GetAllTodosOrderedByDueAsync(year, month, day, GetUserId()));
        }

        #endregion

        #region Create

        /// <summary>
        /// Create a new todo.
        /// </summary>
        /// <remarks>POST api/{version}/todo</remarks>
        /// <param name="model">A model with necessary fields to create a todo</param>
        /// <response code="201">Todo created</response>
        /// <response code="400">Invalid data in body</response>
        /// <response code="401">Token required</response>
        /// <returns>201 and GET route in header if successful, 400 otherwise</returns>
        [SwaggerResponse(201, typeof(void))]
        [SwaggerResponse(400, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
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
                var id = await _todoService.CreateTodoAsync(model, GetUserId());
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

        #endregion

        #region Delete

        /// <summary>
        /// Delete a specific todo.
        /// </summary>
        /// <remarks>DELETE api/{version}/todo/{id}</remarks>
        /// <param name="todoId">The id of the todo requested</param>
        /// <response code="204">Todo removed</response>
        /// <response code="401">Token required</response>
        /// <response code="404">Todo does not exist</response>
        /// <returns>204 if successful, 404 otherwise</returns>
        [SwaggerResponse(204, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(404, typeof(void))]
        [HttpDelete("{todoId:int}")]
        public async Task<IActionResult> RemoveTodo(int todoId)
        {
            try
            {
                await _todoService.RemoveTodoByIdAsync(todoId, GetUserId());
                return NoContent();
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion

        #region Edit

        /// <summary>
        /// Edit an existing todo.
        /// </summary>
        /// <remarks>PUT api/{version}/dodo</remarks>
        /// <param name="changedTodo">Data containing id of the todo to edit and edited fields</param>
        /// <response code="200">Todo modified</response>
        /// <response code="400">Invalid data in body</response>
        /// <response code="401">Token required</response>
        /// <response code="404">Todo does not exist</response>
        /// <returns>400 if invalid body, 404 if todo is not found, 200 otherwise</returns>
        [SwaggerResponse(200, typeof(void))]
        [SwaggerResponse(400, typeof(void))]
        [SwaggerResponse(401, typeof(void))]
        [SwaggerResponse(404, typeof(void))]
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
                await _todoService.EditTodoAsync(changedTodo, GetUserId());
                return Ok();
            }
            catch (TodoNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get the user id from the controller's claim.
        /// </summary>
        /// <returns>A string with the user id</returns>
        private string GetUserId()
        {
            return User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        
        #endregion
    }
}
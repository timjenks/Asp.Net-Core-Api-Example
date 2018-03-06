using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.MockData.Controllers;
using Tests.MockData.DtoModels;
using Tests.MockData.EntityModels;
using Tests.MockData.Services;
using Tests.MockData.ViewModels;
using TodoApi.Utils.Constants;
using TodoApi.Controllers;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using Xunit;

namespace Tests.ControllersTests
{
    /// <summary>
    /// Test for user controller using a mocked service.
    /// </summary>
    public class TodoControllerTest
    {
        #region GetTodo

        [Fact]
        public async Task GetTodo_NonExisting_TodoNotFoundException()
        {
            // Arrange
            var service = new MockTodoService
            {
                MGetTodoByIdAsync = (todoId, userId) => throw new TodoNotFoundException()
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, "I am just gonna throw an exception anyway");
            const int tId = 665;

            // Act
            var result = await controller.GetTodo(tId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetTodo_Existing_OkWithDto()
        {
            // Arrange
            var service = new MockTodoService
            {
                MGetTodoByIdAsync = (todoId, userId) => MockTodoDto.Get(0)
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(5).Id);
            const int tId = 10101;

            // Act
            var result = await controller.GetTodo(tId) as OkObjectResult;
            var dto = result?.Value as TodoDto;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(dto);
            Assert.Equal(MockTodoDto.Get(0).Id, dto.Id);
            Assert.Equal(MockTodoDto.Get(0).Due, dto.Due);
            Assert.Equal(MockTodoDto.Get(0).Description, dto.Description);
        }

        #endregion

        #region GetAllTodods

        [Fact]
        public async Task GetAllTodos_ListOfTodos_Ok()
        {
            // Arrange
            var service = new MockTodoService
            {
                MGetAllTodosOrderedByDueAsync = (year, month, day, userId) => new[]
                {
                    MockTodoDto.Get(0),
                    MockTodoDto.Get(1)
                }
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(5).Id);

            // Act
            var result = await controller.GetAllTodos() as OkObjectResult;
            var list = result?.Value as TodoDto[];

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(list);
            Assert.Equal(2, list.Count());
            Assert.Equal(MockTodoDto.Get(0).Id, list[0].Id);
            Assert.Equal(MockTodoDto.Get(0).Due, list[0].Due);
            Assert.Equal(MockTodoDto.Get(0).Description, list[0].Description);
            Assert.Equal(MockTodoDto.Get(1).Id, list[1].Id);
            Assert.Equal(MockTodoDto.Get(1).Due, list[1].Due);
            Assert.Equal(MockTodoDto.Get(1).Description, list[1].Description);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateTodo_NullModel_BadRequest()
        {
            // Arrange
            var service = new MockTodoService();
            var controller = new TodoController(service);

            // Act
            var result = await controller.CreateTodo(null) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task CreateTodo_InvalidModel_BadRequest()
        {
            // Arrange
            const string field = "required";
            const string errorMessage = "something is required";
            var service = new MockTodoService();
            var controller = new TodoController(service);
            controller.ModelState.AddModelError(field, errorMessage);
            var model = MockCreateTodoViewModel.Get(0);

            // Act
            var result = await controller.CreateTodo(model) as BadRequestObjectResult;
            var error = result?.Value as SerializableError;

            // Assert
            Assert.NotNull(error);
            Assert.Equal(400, result.StatusCode);
            Assert.Single(error.Keys);
            Assert.True(error.ContainsKey(field));
            Assert.Equal(new[] { errorMessage }, error.GetValueOrDefault(field));
        }

        [Fact]
        public async Task CreateTodo_InvalidUser_UserNotFoundException()
        {
            // Arrange
            var service = new MockTodoService
            {
                MCreateTodoAsync = (model, userId) => throw new UserNotFoundException()
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.CreateTodo(MockCreateTodoViewModel.Get(0)) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task CreateTodo_ValidUserAndTodo_CreatedAtRoute()
        {
            // Arrange
            var id = 15;
            var service = new MockTodoService
            {
                MCreateTodoAsync = (model, userId) => id
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.CreateTodo(MockCreateTodoViewModel.Get(0)) as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(MethodNames.GetSingleTodoMethodName, result.RouteName);
            Assert.Single(result.RouteValues.Keys);
            Assert.True(result.RouteValues.ContainsKey("todoId"));
            Assert.Equal(id, result.RouteValues.GetValueOrDefault("todoId"));
        }

        #endregion

        #region Remove

        [Fact]
        public async Task RemoveTodo_InvalidTodo_TodoNotFoundException()
        {
            // Arrange
            var tId = 15;
            var service = new MockTodoService
            {
                MRemoveTodoByIdAsync = (todoId, userId) => throw new TodoNotFoundException()
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.RemoveTodo(tId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task RemoveTodo_ValidTodo_NoContent()
        {
            // Arrange
            var tId = 15;
            var service = new MockTodoService
            {
                MRemoveTodoByIdAsync = (todoId, userId) => { }
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.RemoveTodo(tId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        #endregion

        #region Edit

        [Fact]
        public async Task EditTodo_NullModel_BadRequest()
        {
            // Arrange
            var service = new MockTodoService();
            var controller = new TodoController(service);

            // Act
            var result = await controller.EditTodo(null) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task EditTodo_InvalidModel_BadRequest()
        {
            // Arrange
            const string field = "required";
            const string errorMessage = "something is required";
            var service = new MockTodoService();
            var controller = new TodoController(service);
            controller.ModelState.AddModelError(field, errorMessage);
            var model = MockEditTodoViewModel.Get(0);

            // Act
            var result = await controller.EditTodo(model) as BadRequestObjectResult;
            var error = result?.Value as SerializableError;

            // Assert
            Assert.NotNull(error);
            Assert.Equal(400, result.StatusCode);
            Assert.Single(error.Keys);
            Assert.True(error.ContainsKey(field));
            Assert.Equal(new[] { errorMessage }, error.GetValueOrDefault(field));
        }

        [Fact]
        public async Task EditTodo_InvalidTodo_TodoNotFoundException()
        {
            // Arrange
            var service = new MockTodoService
            {
                MEditTodoAsync = (model, userId) => throw new TodoNotFoundException()
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.EditTodo(MockEditTodoViewModel.Get(0)) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task EditTodo_ValidUserAndTodo_Ok()
        {
            // Arrange
            var service = new MockTodoService
            {
                MEditTodoAsync = (model, userId) => { }
            };
            var controller = new TodoController(service);
            MockClaims.AddUserIdClaim(controller, MockApplicationUsers.Get(0).Id);

            // Act
            var result = await controller.EditTodo(MockEditTodoViewModel.Get(0)) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        #endregion
    }
}

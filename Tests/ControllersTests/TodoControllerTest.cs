using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tests.MockData.Controllers;
using Tests.MockData.DtoModels;
using Tests.MockData.EntityModels;
using Tests.MockData.Services;
using TodoApi.Controllers;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using Xunit;

namespace Tests.ControllersTests
{
    public class TodoControllerTest
    {
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
            var tId = 665;

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
            var tId = 10101;

            // Act
            var result = await controller.GetTodo(tId) as OkObjectResult;
            var dto = result.Value as TodoDto;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(dto);
            Assert.Equal(MockTodoDto.Get(0).Id, dto.Id);
            Assert.Equal(MockTodoDto.Get(0).Due, dto.Due);
            Assert.Equal(MockTodoDto.Get(0).Description, dto.Description);
        }
    }
}

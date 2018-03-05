using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tests.MockData.DtoModels;
using Tests.MockData.EntityModels;
using Tests.MockData.Services;
using TodoApi.Controllers;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using Xunit;

namespace Tests.ControllersTests
{
    /// <summary>
    /// Test for user controller using a mocked service.
    /// </summary>
    public class UserControllerTest
    {
        [Fact]
        public async Task GetUserById_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            var service = new MockUserService
            {
                MGetUserByIdAsync = (userId) => throw new UserNotFoundException()
            };
            var controller = new UserController(service);

            // Act
            var result = await controller.GetUserById(MockApplicationUsers.Get(1).Id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_UserDto()
        {
            // Arrange
            var service = new MockUserService
            {
                MGetUserByIdAsync = (userId) =>  MockApplicationUserDto.Get(0)
            };
            var controller = new UserController(service);

            // Act
            var result = await controller.GetUserById(MockApplicationUserDto.Get(0).Id) as OkObjectResult;
            var dto = result.Value as ApplicationUserDto;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(dto);
            Assert.Equal(MockApplicationUserDto.Get(0).Id, dto.Id);
            Assert.Equal(MockApplicationUserDto.Get(0).Email, dto.Email);
            Assert.Equal(MockApplicationUserDto.Get(0).Name, dto.Name);
        }

        [Fact]
        public async Task GetAllUsers_ListOfUsers_Ok()
        {
            // Arrange
            var service = new MockUserService
            {
                MGetAllUsersOrderedByNameAsync = () => new[]
                {
                    MockApplicationUserDto.Get(0),
                    MockApplicationUserDto.Get(1)
                }
            };
            var controller = new UserController(service);

            // Act
            var result = await controller.GetAllUsers() as OkObjectResult;
            var list = result.Value as ApplicationUserDto[];

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(list);
            Assert.Equal(2, list.Count());
            Assert.Equal(MockApplicationUserDto.Get(0).Id, list[0].Id);
            Assert.Equal(MockApplicationUserDto.Get(0).Email, list[0].Email);
            Assert.Equal(MockApplicationUserDto.Get(0).Name, list[0].Name);
            Assert.Equal(MockApplicationUserDto.Get(1).Id, list[1].Id);
            Assert.Equal(MockApplicationUserDto.Get(1).Email, list[1].Email);
            Assert.Equal(MockApplicationUserDto.Get(1).Name, list[1].Name);
        }

        [Fact]
        public async Task GetAllUsers_NonExistingUser_UserNotFoundException()
        {
            // Arrange
            var service = new MockUserService
            {
                MRemoveUserByIdAsync = (userId) => throw new UserNotFoundException()
            };
            var controller = new UserController(service);
            var id = "1763fc4a-a95e-44bb-b9e5-e56273e4c6f8";

            // Act
            var result = await controller.RemoveUserById(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_ExistingUser_NoContent()
        {
            // Arrange
            var service = new MockUserService { MRemoveUserByIdAsync = (userId) => { } };
            var controller = new UserController(service);
            var id = "1763fc4a-a95e-44bb-b9e5-e56273e4c6f8";

            // Act
            var result = await controller.RemoveUserById(id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}

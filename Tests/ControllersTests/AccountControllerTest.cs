using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.MockData.Services;
using Tests.MockData.ViewModels;
using TodoApi.Controllers;
using TodoApi.Exceptions;
using TodoApi.Services.Interfaces;
using Xunit;

namespace Tests.ControllersTests
{
    /// <summary>
    /// Test for account controller using a mocked service.
    /// </summary>
    public class AccountControllerTest
    {
        [Fact]
        public async Task Login_NullModel_BadRequest()
        {
            // Arrange
            var service = new MockAccountService { MLogin = (model) => "Doesn't really matter" };
            var controller = new AccountController(service);

            // Act
            var result = await controller.Login(null) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidState_BadRequest()
        {
            // Arrange
            const string field = "apple-error";
            const string errorMessage = "how about them apples";
            var service = new MockAccountService { MLogin = (model) => "Doesn't really matter a second time" };
            var controller = new AccountController(service);
            controller.ModelState.AddModelError(field, errorMessage);

            // Act
            var result = await controller.Login(MockLoginViewModel.Get(0)) as BadRequestObjectResult;
            var error = result?.Value as SerializableError;

            // Assert
            Assert.NotNull(error);
            Assert.Equal(400, result.StatusCode);
            Assert.Single(error.Keys);
            Assert.True(error.ContainsKey(field));
            Assert.Equal(new[] { errorMessage }, error.GetValueOrDefault(field));
        }

        [Fact]
        public async Task Login_LoginFailException_Unauthorized()
        {
            // Arrange
            var service = new MockAccountService { MLogin = (model) => throw new LoginFailException() };
            var controller = new AccountController(service);

            // Act
            var result = await controller.Login(MockLoginViewModel.Get(1)) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task Login_Valid_OkWithToken()
        {
            // Arrange
            var mockToken = "This is not a love-token song";
            var service = new MockAccountService { MLogin = (model) => string.Copy(mockToken) };
            var controller = new AccountController(service);

            // Act
            var result = await controller.Login(MockLoginViewModel.Get(1)) as OkObjectResult;
            var tokenReceived = result.Value as string;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(tokenReceived);
            Assert.Equal(string.Copy(mockToken), tokenReceived);
        }
    }
}

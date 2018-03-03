using System.Collections.Generic;
using System.Linq;
using Tests.MockData.EntityModels;
using Tests.MockData.ViewModels;
using TodoApi.Models.EntityModels;
using Xunit;

namespace Tests.ModelsTests.EntityModelsTests
{
    public class ApplicationUserTest
    {
        [Fact]
        public void ApplicationUserNoArgumentConstructor_Getter_Matches()
        {
            // Arrange
            // Act
            var user = new ApplicationUser
            {
                Id = "18a86ba8-defd-4075-b822-020d4cc877e2",
                Name = "Hans Gruber",
                Email = "hans@mail.de",
                UserName = "hans@mail.de",
                PasswordHash = "AxvfcNXm$c3968c55f3cb65f662ec29fec1b4dc5ef605e31de2bc0690f18de659609476e3",
                Todos = new HashSet<Todo> { MockTodos.Get(0), MockTodos.Get(1) }
            };
            var todoIds = user.Todos.Select(y => y.Id).ToHashSet();

            // Assert
            Assert.Equal("18a86ba8-defd-4075-b822-020d4cc877e2", user.Id);
            Assert.Equal("hans@mail.de", user.Email);
            Assert.Equal("hans@mail.de", user.UserName);
            Assert.Equal("Hans Gruber", user.Name);
            Assert.Equal("AxvfcNXm$c3968c55f3cb65f662ec29fec1b4dc5ef605e31de2bc0690f18de659609476e3", user.PasswordHash);
            
            Assert.Equal(2, user.Todos.Count);
            Assert.Equal(2, todoIds.Count);
            Assert.Contains(MockTodos.Get(0).Id, todoIds);
            Assert.Contains(MockTodos.Get(1).Id, todoIds);
        }

        [Fact]
        public void ApplicationUserViewModelConstructor_Getter_Matches()
        {
            // Arrange
            var viewModel = MockRegisterViewModel.Get(0);

            // Act
            var user = new ApplicationUser(viewModel);

            // Assert
            Assert.Equal(viewModel.Email, user.Email);
            Assert.Equal(viewModel.Email, user.UserName);
            Assert.Equal(viewModel.Name, user.Name);
        }

        [Fact]
        public void ApplicationUser_Setter_Modifies()
        {

            // Arrange
            var user = MockApplicationUsers.Get(0);

            // Act
            user.Name = "Snake Plissken";
            user.Todos = new HashSet<Todo> { MockTodos.Get(5) };

            // Assert
            Assert.Equal("Snake Plissken", user.Name);
            Assert.Single(user.Todos);
            Assert.Equal(MockTodos.Get(5).Id, user.Todos.SingleOrDefault().Id);
        }
    }
}

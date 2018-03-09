#region Imports

using System;
using Tests.MockData.EntityModels;
using Tests.MockData.ViewModels;
using TodoApi.Models.EntityModels;
using Xunit;

#endregion

namespace Tests.ModelsTests.EntityModelsTests
{
    /// <summary>
    /// Testing Todo entity class.
    /// </summary>
    public class TodoTest
    {
        [Fact]
        public void TodoNoArgumentConstructor_Getter_Matches()
        {
            // Arrange
            // Act
            var todo = new Todo
            {
                Id = 15,
                Due = new DateTime(1999, 10, 15, 12, 12, 12),
                Description = "row row row your boat",
                Owner = MockApplicationUsers.Get(2)
            };

            // Assert
            Assert.Equal(15, todo.Id);
            Assert.Equal(new DateTime(1999, 10, 15, 12, 12, 12), todo.Due);
            Assert.Equal("row row row your boat", todo.Description);
            Assert.Equal(MockApplicationUsers.Get(2).Id, todo.Owner.Id);
            Assert.Equal(MockApplicationUsers.Get(2).Name, todo.Owner.Name);
            Assert.Equal(MockApplicationUsers.Get(2).Email, todo.Owner.Email);
            Assert.Equal(MockApplicationUsers.Get(2).UserName, todo.Owner.UserName);
            Assert.Equal(MockApplicationUsers.Get(2).PasswordHash, todo.Owner.PasswordHash);
        }

        [Fact]
        public void TodoUserAndViewModelConstructor_Getter_Matches()
        {
            // Arrange
            var viewModel = MockCreateTodoViewModel.Get(1);
            var user = MockApplicationUsers.Get(3);

            // Act
            var todo = new Todo(viewModel, user);

            // Assert
            Assert.Equal(0 /* unset */, todo.Id);
            Assert.Equal(viewModel.Due, todo.Due);
            Assert.Equal(viewModel.Description, todo.Description);
            Assert.Equal(user.Id, todo.Owner.Id);
            Assert.Equal(user.Name, todo.Owner.Name);
            Assert.Equal(user.Email, todo.Owner.Email);
            Assert.Equal(user.UserName, todo.Owner.UserName);
            Assert.Equal(user.PasswordHash, todo.Owner.PasswordHash);
        }

        [Fact]
        public void TodoViewModelEdit_Getter_Matches()
        {
            // Arrange
            var viewModel = MockEditTodoViewModel.Get(0);

            // Act
            var todo = MockTodos.Get(3);
            todo.Edit(viewModel);

            // Assert
            Assert.Equal(viewModel.Id, todo.Id);
            Assert.Equal(viewModel.Due, todo.Due);
            Assert.Equal(viewModel.Description, todo.Description);
        }

        [Fact]
        public void Todo_Setter_Modifies()
        {
            // Arrange
            var todo = MockTodos.Get(7);

            // Act
            todo.Id = 44;
            todo.Due = new DateTime(2000, 3, 15, 18, 19, 20);
            todo.Description = "Bring up the wolf's head";
            todo.Owner = MockApplicationUsers.Get(0);

            // Assert
            Assert.Equal(44, todo.Id);
            Assert.Equal(new DateTime(2000, 3, 15, 18, 19, 20), todo.Due);
            Assert.Equal("Bring up the wolf's head", todo.Description);
            Assert.Equal(MockApplicationUsers.Get(0).Id, todo.Owner.Id);
            Assert.Equal(MockApplicationUsers.Get(0).Name, todo.Owner.Name);
            Assert.Equal(MockApplicationUsers.Get(0).Email, todo.Owner.Email);
            Assert.Equal(MockApplicationUsers.Get(0).UserName, todo.Owner.UserName);
            Assert.Equal(MockApplicationUsers.Get(0).PasswordHash, todo.Owner.PasswordHash);
        }
    }
}
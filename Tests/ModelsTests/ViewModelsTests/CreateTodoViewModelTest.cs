using System;
using System.Linq;
using System.Text;
using Tests.MockData.ViewModels;
using Tests.Models.Helpers;
using TodoApi.Constants;
using TodoApi.Models.ViewModels;
using Xunit;

namespace Tests.ModelsTests.ViewModelsTests
{
    /// <summary>
    /// Testing create todo view model.
    /// </summary>
    public class CreateTodoViewModelTest
    {
        [Fact]
        public void CreateTodoViewModel_Getter_Matches()
        {
            // Arrange
            // Act
            var todo = new CreateTodoViewModel
            {
                Description = "Apply dry rub",
                Due = new DateTime(2011, 5, 1, 4, 15, 0)
            };

            Assert.Equal("Apply dry rub", todo.Description);
            Assert.Equal(new DateTime(2011, 5, 1, 4, 15, 0), todo.Due);
        }

        [Fact]
        public void CreateTodoViewModel_Setter_Modifies()
        {
            // Arrange
            var viewModel = MockCreateTodoViewModel.Get(0);

            // Act
            viewModel.Description = "Drink a gallon of habanero tabasco";
            viewModel.Due = new DateTime(2025, 12, 22, 0, 0, 0);

            // Assert
            Assert.Equal("Drink a gallon of habanero tabasco", viewModel.Description);
            Assert.Equal(new DateTime(2025, 12, 22, 0, 0, 0), viewModel.Due);
        }

        [Fact]
        public void CreateTodoViewModel_AllFieldsValid_NoErrors()
        {
            // Arrange
            var model = MockCreateTodoViewModel.Get(1);

            // Act
            var validation = ModelValidator.ValidateModel(model);

            // Assert
            Assert.Equal(0, validation.Count);
        }

        [Fact]
        public void CreateTodoViewModel_DueMissing_Error()
        {
            // Arrange
            var model = new CreateTodoViewModel
            {
                Description = "I'm a vampire!"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.TodoDueRequired, errorMessage);
        }

        [Fact]
        public void CreateTodoViewModel_DescriptionMissing_Error()
        {
            // Arrange
            var model = new CreateTodoViewModel
            {
                Due = DateTime.Now
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.TodoDescriptionRequired, errorMessage);
        }
        
        [Fact]
        public void CreateTodoViewModel_DescriptionTooLong_Error()
        {
            // Arrange
            var len = TodoLimits.DescriptionMaxLength + 1;
            var model = new CreateTodoViewModel
            {
                Description = new StringBuilder(len).Insert(0, "A", len).ToString(),
                Due = DateTime.Now
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.TodoDescriptionMaxLength, errorMessage);
        }
    }
}

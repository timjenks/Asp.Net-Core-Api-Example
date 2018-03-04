using System;
using System.Collections.Generic;
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
    /// Testing edit todo view model.
    /// </summary>
    public class EditTodoViewModelTest
    {
        [Fact]
        public void EditTodoViewModel_Getter_Matches()
        {
            // Arrange
            // Act
            var model = new EditTodoViewModel
            {
                Id = 5199,
                Description = "Walk like an Egyptian",
                Due = new DateTime(2031, 8, 11, 18, 0, 0)
            };

            // Assert
            Assert.Equal(5199, model.Id);
            Assert.Equal("Walk like an Egyptian", model.Description);
            Assert.Equal(new DateTime(2031, 8, 11, 18, 0, 0), model.Due);
        }

        [Fact]
        public void EditTodoViewModel_Setter_Modifies()
        {
            // Arrange
            var viewModel = MockEditTodoViewModel.Get(1);

            // Act
            viewModel.Id = 1551;
            viewModel.Description = "Make Love, Not Warcraft";
            viewModel.Due = new DateTime(2022, 1, 1, 12, 25, 0);

            // Assert
            Assert.Equal(1551, viewModel.Id);
            Assert.Equal("Make Love, Not Warcraft", viewModel.Description);
            Assert.Equal(new DateTime(2022, 1, 1, 12, 25, 0), viewModel.Due);
        }

        [Fact]
        public void EditTodoViewModel_AllFieldsValid_NoErrors()
        {
            // Arrange
            var model = MockEditTodoViewModel.Get(1);

            // Act
            var validation = ModelValidator.ValidateModel(model);

            // Assert
            Assert.Equal(0, validation.Count);
        }

        [Fact]
        public void EditTodoViewModel_IdMissing_Error()
        {
            // Arrange
            var model = new EditTodoViewModel
            {
                Description = "Build a ladder to heaven",
                Due = DateTime.Now
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.TodoIdRequired, errorMessage);
        }

        [Fact]
        public void EditTodoViewModel_DueMissing_Error()
        {
            // Arrange
            var model = new EditTodoViewModel
            {
                Description = "Give an angry speech before being killed by a shark",
                Id = 5555
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.TodoDueRequired, errorMessage);
        }

        [Fact]
        public void EditTodoViewModel_DescriptionMissing_Error()
        {
            // Arrange
            var model = new EditTodoViewModel
            {
                Id = 12345,
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
        public void EditTodoViewModel_DescriptionTooLong_Error()
        {
            // Arrange
            var len = TodoLimits.DescriptionMaxLength + 1;
            var model = new EditTodoViewModel
            {
                Description = new StringBuilder(len).Insert(0, "A", len).ToString(),
                Due = DateTime.Now,
                Id = 314159
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

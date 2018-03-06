using System.Linq;
using System.Text;
using Tests.Helpers.Models;
using Tests.MockData.ViewModels;
using TodoApi.Constants;
using TodoApi.Models.ViewModels;
using Xunit;

namespace Tests.ModelsTests.ViewModelsTests
{
    /// <summary>
    /// Testing register view model.
    /// </summary>
    public class RegisterViewModelTest
    {
        [Fact]
        public void RegisterViewModel_Getter_Matches()
        {
            // Arrange
            // Act
            var model = new RegisterViewModel
            {
                Name = "Curtis",
                Email = "curtis@moveonup.com",
                Password = "A1#b.X"
            };

            // Assert
            Assert.Equal("Curtis", model.Name);
            Assert.Equal("curtis@moveonup.com", model.Email);
            Assert.Equal("A1#b.X", model.Password);
        }

        [Fact]
        public void RegisterViewModel_Setter_Modifies()
        {
            // Arrange
            var model = MockRegisterViewModel.Get(1);

            // Act
            model.Name = "Curtis";
            model.Email = "curtis@superfly.com";
            model.Password = "Xx14$$";

            // Assert
            Assert.Equal("Curtis", model.Name);
            Assert.Equal("curtis@superfly.com", model.Email);
            Assert.Equal("Xx14$$", model.Password);
        }

        [Fact]
        public void RegisterViewModel_AllFieldsValid_NoErrors()
        {
            // Arrange
            var model = MockRegisterViewModel.Get(0);

            // Act
            var validation = ModelValidator.ValidateModel(model);

            // Assert
            Assert.Equal(0, validation.Count);
        }

        [Fact]
        public void RegisterViewModel_NameMissing_Error()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "curtis@freddiesdead.com",
                Password = "##Aa12##",
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.UserNameRequired, errorMessage);
        }

        [Fact]
        public void RegisterViewModel_NameToLong_Error()
        {
            // Arrange
            var len = UserLimits.NameMaxLength + 1;
            var model = new RegisterViewModel
            {
                Email = "curtis@freddiesdead.com",
                Password = "##Aa12##",
                Name = new StringBuilder(len).Insert(0, "X", len).ToString()
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.UserNameMaxLength, errorMessage);
        }

        [Fact]
        public void RegisterViewModel_EmailMissing_Error()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Password = "Abcd#$123",
                Name = "Curts"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountEmailIsRequired, errorMessage);
        }

        [Fact]
        public void RegisterViewModel_EmailInvalid_Error()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "Little child running wild",
                Password = "A1b2#c3d4",
                Name = "Curtis"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal("The Email field is not a valid e-mail address.", errorMessage);
        }

        [Fact]
        public void RegisterViewModel_PasswordMissing_Error()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Name = "Curtis",
                Email = "curtis@missblackamerica.com",
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountPasswordIsRequired, errorMessage);
        }

        [Fact]
        public void RegisterViewModel_PasswordTooShort_Error()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Name = "Curtis",
                Email = "curtis@keeponkeepinon.com",
                Password = "Aa#4"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountPasswordInvalidLength, errorMessage);
        }

        [Fact]
        public void RegisterViewModel_PasswordTooLong_Error()
        {
            // Arrange
            var len = PasswordLimits.AccountMaxPasswordLength + 5;
            var model = new RegisterViewModel
            {
                Name = "Curtis",
                Email = "curtis@hellbelow.com",
                Password = new StringBuilder(len).Insert(0, "Aa#4", len >> 2).ToString()
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountPasswordInvalidLength, errorMessage);
        }
    }
}

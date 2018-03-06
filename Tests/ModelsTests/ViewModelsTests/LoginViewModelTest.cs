using System.Linq;
using System.Text;
using Tests.Helpers.Models;
using Tests.MockData.ViewModels;
using TodoApi.Utils.Constants;
using TodoApi.Models.ViewModels;
using Xunit;

namespace Tests.ModelsTests.ViewModelsTests
{
    /// <summary>
    /// Testing login view model.
    /// </summary>
    public class LoginViewModelTest
    {
        [Fact]
        public void LoginViewModel_Getter_Matches()
        {
            // Arrange
            // Act
            var model = new LoginViewModel
            {
                Email = "dick@shooter.com",
                Password = "R0B0-cop"
            };

            // Assert
            Assert.Equal("dick@shooter.com", model.Email);
            Assert.Equal("R0B0-cop", model.Password);
        }

        [Fact]
        public void LoginViewModel_Setter_Modifies()
        {
            // Arrange
            var model = MockLoginViewModel.Get(0);

            // Act
            model.Password = "St0p-Making-S3ns3";
            model.Email = "talking@heads.com";

            // Assert
            Assert.Equal("talking@heads.com", model.Email);
            Assert.Equal("St0p-Making-S3ns3", model.Password);
        }

        [Fact]
        public void LoginViewModel_AllFieldsValid_NoErrors()
        {
            // Arrange
            var model = MockLoginViewModel.Get(1);

            // Act
            var validation = ModelValidator.ValidateModel(model);

            // Assert
            Assert.Equal(0, validation.Count);
        }

        [Fact]
        public void LoginViewModel_EmailMissing_Error()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Password = "A1b2#c3d4"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountEmailIsRequired, errorMessage);
        }

        [Fact]
        public void LoginViewModel_EmailInvalid_Error()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "This is not a email",
                Password = "A1b2#c3d4"
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation); 
            Assert.Equal("The Email field is not a valid e-mail address.", errorMessage);
        }

        [Fact]
        public void LoginViewModel_PasswordMissing_Error()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "southern@man.com",
            };

            // Act
            var validation = ModelValidator.ValidateModel(model);
            var errorMessage = validation.Select(x => x.ErrorMessage).SingleOrDefault();

            // Assert
            Assert.Single(validation);
            Assert.Equal(ErrorMessages.AccountPasswordIsRequired, errorMessage);
        }

        [Fact]
        public void LoginViewModel_PasswordTooShort_Error()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Email = "harvest@moon.com",
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
        public void LoginViewModel_PasswordTooLong_Error()
        {
            // Arrange
            var len = PasswordLimits.AccountMaxPasswordLength + 5;
            var model = new LoginViewModel
            {
                Email = "old@man.com",
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

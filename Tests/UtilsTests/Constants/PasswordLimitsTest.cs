using TodoApi.Utils.Constants;
using Xunit;

namespace Tests.UtilsTests.Constants
{
    /// <summary>
    /// Tests for password limit error message set.
    /// </summary>
    public class PasswordLimitsTest
    {
        [Fact]
        public void SettingsErrorMessages_CheckIfContainsAllLimits()
        {
            // Arrange
            // Act
            var errorMessages = PasswordLimits.SettingsErrorMessages;

            // Assert
            Assert.Contains("PasswordRequiresUniqueChars", errorMessages);
            Assert.Contains("PasswordRequiresDigit", errorMessages);
            Assert.Contains("PasswordRequiresLower", errorMessages);
            Assert.Contains("PasswordRequiresUpper", errorMessages);
            Assert.Contains("PasswordRequiresNonAlphanumeric", errorMessages);
        }
    }
}

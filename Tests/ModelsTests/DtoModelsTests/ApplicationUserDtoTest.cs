using Tests.MockData.DtoModels;
using Tests.MockData.EntityModels;
using TodoApi.Models.DtoModels;
using Xunit;

namespace Tests.ModelsTests.DtoModelsTests
{
    /// <summary>
    /// Testing ApplicationUser dto class.
    /// </summary>
    public class ApplicationUserDtoTest
    {

        [Fact]
        public void ApplicationUserDtoNoArgumentConstructor_Getter_Matches()
        {
            // Arrange
            // Act
            var user = new ApplicationUserDto
            {
                Id = "96390d97-e499-4c09-93bc-255d3a0fe19a",
                Email = "erdos@nowhere.com",
                Name = "Paul Erdos"
            };

            // Assert
            Assert.Equal("96390d97-e499-4c09-93bc-255d3a0fe19a", user.Id);
            Assert.Equal("erdos@nowhere.com", user.Email);
            Assert.Equal("Paul Erdos", user.Name);
        }

        [Fact]
        public void ApplicationUserDtoEntityConstructor_Getter_Matches()
        {
            // Arrange
            var entity = MockApplicationUsers.Get(8);

            // Act
            var dto = new ApplicationUserDto(entity);

            // Assert
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.Email, dto.Email);
            Assert.Equal(entity.Name, dto.Name);
        }


        [Fact]
        public void ApplicationUserDto_Setter_Modifies()
        {
            // Arrange
            var user = MockApplicationUserDto.Get(1);

            // Act
            user.Id = "1317f39e-6265-44e5-ae62-0d537ab650f7";
            user.Email = "aliens@consumerism.com";
            user.Name = "George Nada";

            // Assert
            Assert.Equal("1317f39e-6265-44e5-ae62-0d537ab650f7", user.Id);
            Assert.Equal("aliens@consumerism.com", user.Email);
            Assert.Equal("George Nada", user.Name);
        }
    }
}

using System;
using Tests.MockData.DtoModels;
using Tests.MockData.EntityModels;
using TodoApi.Models.DtoModels;
using Xunit;

namespace Tests.ModelsTests.DtoModelsTests
{
    /// <summary>
    /// Testing Todo dto class.
    /// </summary>
    public class TodoDtoTest
    {
        [Fact]
        public void TodoDtoNoArgumentConstructor_Getter_Matches()
        {
            // Arrange
            // Act
            var todo = new TodoDto
            {
                Id = 113,
                Due = new DateTime(2029, 5, 23, 9, 35, 0),
                Description = "Watch ArsenalFanTv if they lose"
            };

            // Assert
            Assert.Equal(113, todo.Id);
            Assert.Equal(new DateTime(2029, 5, 23, 9, 35, 0), todo.Due);
            Assert.Equal("Watch ArsenalFanTv if they lose", todo.Description);
        }


        [Fact]
        public void TodoDtoEntityConstructor_Getter_Matches()
        {
            // Arrange
            var entity = MockTodos.Get(18);

            // Act
            var todo = new TodoDto(entity);

            // Assert
            Assert.Equal(entity.Id, todo.Id);
            Assert.Equal(entity.Description, todo.Description);
            Assert.Equal(entity.Due, todo.Due);
        }

        [Fact]
        public void TodoDto_Setter_Modifies()
        {
            // Arrange
            var dto = MockTodoDto.Get(1);

            // Act
            dto.Id = 55;
            dto.Due = new DateTime(2020, 12, 24, 16, 0, 0);
            dto.Description = "Start cooking";

            // Assert
            Assert.Equal(55, dto.Id);
            Assert.Equal(new DateTime(2020, 12, 24, 16, 0 ,0), dto.Due);
            Assert.Equal("Start cooking", dto.Description);
        }
    }
}

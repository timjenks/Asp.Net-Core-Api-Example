using System;
using Tests.MockData.EntityModels;
using TodoApi.Models.EntityModels;
using Xunit;

namespace Tests.ModelsTests.EntityModelsTests
{
    /// <summary>
    /// Testing Todo entity class.
    /// </summary>
    public class TodoTest
    {
        [Fact]
        public void NoArgumentConstructor_Getter_Matches()
        {
            var todo = new Todo
            {
                Id = 15,
                Due = new DateTime(1999, 10, 15, 12, 12, 12),
                Description = "row row row your boat",
                Owner = MockApplicationUsers.Get(2)
            };

            Assert.Equal(15, todo.Id);
            Assert.Equal(new DateTime(1999, 10, 15, 12, 12, 12), todo.Due);
            Assert.Equal("row row row your boat", todo.Description);
            Assert.Equal(MockApplicationUsers.Get(2).Id, todo.Owner.Id);
            Assert.Equal(MockApplicationUsers.Get(2).Name, todo.Owner.Name);
            Assert.Equal(MockApplicationUsers.Get(2).Email, todo.Owner.Email);
            Assert.Equal(MockApplicationUsers.Get(2).UserName, todo.Owner.UserName);
            Assert.Equal(MockApplicationUsers.Get(2).PasswordHash, todo.Owner.PasswordHash);
        }
    }
}
//Bring up the wolf's head
//
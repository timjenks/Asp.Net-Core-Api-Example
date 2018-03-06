﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Helpers.EndSystems;
using Tests.Helpers.Json;
using Tests.MockData.EntityModels;
using TodoApi.Models.DtoModels;
using TodoApi.Models.EntityModels;
using TodoApi.Utils.Constants;
using Xunit;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Start the server with a in memory version of 
    /// our startup using the mock data and test it
    /// with a local client making http requests to
    /// it and assert on responses received. 
    /// </summary>
    public class TodoIntegrationTest
    {
        private readonly MockServerAndClient _endSystems;

        /// <summary>
        /// Before each.
        /// </summary>
        public TodoIntegrationTest()
        {
            _endSystems = new MockServerAndClient();
        }

        #region GetTodo

        [Fact]
        public async Task GetTodo_NoToken_Unauthorized()
        {
            // Arrange
            var todoToFind = MockTodos.Get(14);
            var path = $"{Routes.TodoRoute}/{todoToFind.Id}";

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetTodo_ExistingUserNonExistingToken_NotFound()
        {
            // Arrange
            var user = MockApplicationUsers.Get(3);
            var todoId = 0;
            var path = $"{Routes.TodoRoute}/{todoId}";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetTodo_UserDoesNotOwnTodo_NotFound()
        {
            // Arrange
            var user = MockApplicationUsers.Get(3);
            Todo todo = null;
            for (var i = 0; i < 20; i++)
            {
                todo = MockTodos.Get(i);
                if (todo.Owner.Id != user.Id)
                {
                    break;
                }
            }
            var path = $"{Routes.TodoRoute}/{todo.Id}";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetTodo_UserOwnsTodoAndBothExist_OkWithDto()
        {
            // Arrange
            var todo = MockTodos.Get(14);
            var user = MockApplicationUsers
                .GetAll()
                .SingleOrDefault(w => w.Id == todo.Owner.Id);
            var path = $"{Routes.TodoRoute}/{todo.Id}";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dto = JsonStringSerializer.GetTodoDto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Equal(todo.Id, dto.Id);
            Assert.Equal(todo.Description, dto.Description);
            Assert.Equal(todo.Due, dto.Due);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region GetAllTodos

        [Fact]
        public async Task GetAllTodos_NoToken_Unauthorized()
        {
            // Arrange
            var path = Routes.TodoRoute;

            // Act
            var response = await _endSystems.Get(path);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.Code);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenYearQueryMissing_OkAndDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var todos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id)
                .OrderBy(w => w.Due)
                .ToArray();
            var path = $"{Routes.TodoRoute}?month=5&day=1";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            CheckOrderAndEqual(todos, dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenMonthQueryMissing_OkAndDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(7);
            var todos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id)
                .OrderBy(w => w.Due)
                .ToArray();
            var path = $"{Routes.TodoRoute}?year=2000&day=1";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            CheckOrderAndEqual(todos, dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenDayQueryMissing_OkAndDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(2);
            var todos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id)
                .OrderBy(w => w.Due)
                .ToArray();
            var path = $"{Routes.TodoRoute}?year=2019&month=12";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            CheckOrderAndEqual(todos, dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenYearQueryInvalid_OkAndEmptyDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(5);
            var path = $"{Routes.TodoRoute}?year=XXX&month=12&day=13";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Empty(dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenMonthQueryInvalid_OkAndEmptyDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(6);
            var path = $"{Routes.TodoRoute}?year=2018&month=XXX&day=13";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Empty(dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenDayQueryInvalid_OkAndEmptyDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(9);
            var path = $"{Routes.TodoRoute}?year=2017&month=12&day=XXX";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Empty(dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenDateQueryInvalid_OkAndEmptyDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(2);
            var path = $"{Routes.TodoRoute}?year=2017&month=15&day=33";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Empty(dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenValidQuery_OkAndFilteredDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var todos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id)
                .OrderBy(w => w.Due)
                .ToArray();
            Assert.True(todos.Length > 1);
            var filterDate = todos[0].Due.Date;
            Assert.NotEqual(todos[0].Due.Date, todos[1].Due.Date);
            var filteredTodos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id && w.Due.Date == filterDate)
                .OrderBy(w => w.Due)
                .ToArray();
            Assert.NotEqual(todos.Length, filteredTodos.Length);
            var path = $"{Routes.TodoRoute}?year={filterDate.Year}&month={filterDate.Month}&day={filterDate.Day}";
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.NotEqual(todos.Length, dtos.Length);
            CheckOrderAndEqual(filteredTodos, dtos);

            // Tear down
            _endSystems.Dispose();
        }

        [Fact]
        public async Task GetAllTodos_UserHasTokenNoQuery_OkAndDtoArray()
        {
            // Arrange
            var user = MockApplicationUsers.Get(4);
            var todos = MockTodos
                .GetAll()
                .Where(w => w.Owner.Id == user.Id)
                .OrderBy(w => w.Due)
                .ToArray();
            var path = Routes.TodoRoute;
            var token = await GetToken(user);
            _endSystems.SetBearerToken(token);

            // Act
            var response = await _endSystems.Get(path);
            var dtos = JsonStringSerializer.GetListOfTodoto(response.Body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.Code);
            CheckOrderAndEqual(todos, dtos);

            // Tear down
            _endSystems.Dispose();
        }

        #endregion

        #region Create
        // todo
        #endregion

        #region Remove
        // todo
        #endregion

        #region Edit
        // todo
        #endregion

        #region Helpers

        /// <summary>
        /// Login with a user and return the token as string.
        /// </summary>
        /// <param name="user">A user to login</param>
        /// <returns>A token as string</returns>
        private async Task<string> GetToken(ApplicationUser user)
        {
            Assert.NotNull(user);
            Assert.NotNull(user.Email);
            var body = StringJsonBuilder.LoginJsonBody(
                user.Email, MockApplicationUsers.UniversalPassword);
            var content = new StringContent(body);
            var response = await _endSystems.Post(Routes.AccountRoute + "/login", content);
            Assert.NotNull(response);
            var token = response.Body;
            Assert.NotNull(token);
            return token;
        }

        /// <summary>
        /// Check if dtos matches the todos and if
        /// the dtos are ordered by due time.
        /// </summary>
        /// <param name="todos">The todos some user owns</param>
        /// <param name="dtos">The dtos from a response</param>
        private void CheckOrderAndEqual(Todo[] todos, TodoDto[] dtos)
        {
            TodoDto last = null;
            Assert.Equal(todos.Count(), dtos.Length);
            for (var i = 0; i < dtos.Length; i++)
            {
                Assert.Equal(todos[i].Id, dtos[i].Id);
                Assert.Equal(todos[i].Due, dtos[i].Due);
                Assert.Equal(todos[i].Description, dtos[i].Description);
                if (last != null)
                {
                    Assert.True(dtos[i].Due >= last.Due);
                }
                last = dtos[i];
            }
        }

        #endregion
    }
}

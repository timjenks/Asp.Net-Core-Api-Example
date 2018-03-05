using System;
using System.Collections.Generic;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.EntityModels
{
    /// <summary>
    /// A static coollection for mock todo entities.
    /// </summary>
    public static class MockTodos
    {
        /// <summary>
        /// The first id for mock todo data.
        /// </summary>
        public const int FirstId = 90210;

        /// <summary>
        /// First id, and for each added is incremented by one.
        /// </summary>
        private static int FirstIdRunner = FirstId;

        /// <summary>
        /// Helper to create todo which handles id and owner.
        /// </summary>
        /// <param name="due">The due date</param>
        /// <param name="description">The description</param>
        /// <returns>A Todo</returns>
        private static Todo CreateTodo(DateTime due, string description)
        {
            var todo = new Todo
            {
                Id = FirstIdRunner,
                Due = due,
                Description = description,
                Owner = MockApplicationUsers.Get((FirstIdRunner >> 1) - (FirstId >> 1))
            };
            FirstIdRunner++;
            return todo;
        }

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly Todo[] Data =
        {
            // todo 0
            CreateTodo
            (
                    new DateTime(2029,8,11,14,40,0),
                    "Apply for a job with the least possible amount of responsibility"
            ),
            // todo 1
            CreateTodo
            (
                    new DateTime(2021,2,17,19,20,0),
                    "Chew bubblegum and kick ass"
            ),
            // todo 2
            CreateTodo
            (
                    new DateTime(2020,5,8,0,0,0),
                    "Capture the brain bug"
            ),
            // todo 3
            CreateTodo
            (
                    new DateTime(2027,5,14,16,30,0),
                    "Destroy Skynet"
            ),
            // todo 4
            CreateTodo
            (
                    new DateTime(2021,12,5,9,10,0),
                    "Write a book in a Colorado hotel during winter"
            ),
            // todo 5
            CreateTodo
            (
                    new DateTime(2020,1,6,23,0,0),
                    "Take the blue pill"
            ),
            // todo 6
            CreateTodo
            (
                    new DateTime(2031,9,22,14,30,0),
                    "Fly a plane with Kareem Abdul-Jabbar"
            ),
            // todo 7
            CreateTodo
            (
                    new DateTime(2026,2,28,3,30,0),
                    "Take hostages at the Nakatomi Tower"
            ),
            // todo 8
            CreateTodo
            (
                    new DateTime(2025,9,9,0,50,0),
                    "Cross the streams"
            ),
            // todo 9
            CreateTodo
            (
                    new DateTime(2032,7,3,23,20,0),
                    "Get the Videodrome, long live the new flesh"
            ),
            // todo 10
            CreateTodo
            (
                    new DateTime(2029,9,12,9,40,0),
                    "Stay in the past and be a king"
            ),
            // todo 11
            CreateTodo
            (
                    new DateTime(2022,12,28,18,20,0),
                    "Bring the knights a shrubbery"
            ),
            // todo 12
            CreateTodo
            (
                    new DateTime(2017,3,5,5,40,0),
                    "Take the red pill"
            ),
            // todo 13
            CreateTodo
            (
                    new DateTime(2034,1,10,17,20,0),
                    "Extract stuff from flies in amber"
            ),
            // todo 14
            CreateTodo
            (
                    new DateTime(2025,9,13,20,20,0),
                    "Buy a furry creature in Chinatown"
            ),
            // todo 15
            CreateTodo
            (
                    new DateTime(2027,9,25,18,30,0),
                    "Kidnap Gustafson's daughter"
            ),
            // todo 16
            CreateTodo
            (
                    new DateTime(2024,9,10,13,20,0),
                    "Escape through Raquel Welch"
            ),
            // todo 17
            CreateTodo
            (
                    new DateTime(2028,1,23,13,40,0),
                    "Tell three punks to give me their clothes"
            ),
            // todo 18
            CreateTodo
            (
                    new DateTime(2019,11,5,23,10,0),
                    "Retire the replicants"
            ),
            // todo 19
            CreateTodo
            (
                    new DateTime(2033,7,4,17,20,0),
                    "Save a Norwegian dog on Antarctica"
            ),
        };

        /// <summary>
        /// Get a single mock todo entity. The instance is copied from the array.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <param name="index">The index of the mock todo entity to get</param>
        /// <returns>The mock todo entity with given index in array</returns>
        public static Todo Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new Todo
            {
                Id = Data[index].Id,
                Description = string.Copy(Data[index].Description),
                Due = Data[index].Due,
                Owner = new ApplicationUser
                {
                    Id = string.Copy(Data[index].Owner.Id),
                    Name = string.Copy(Data[index].Owner.Name),
                    Email = string.Copy(Data[index].Owner.Email),
                    UserName = string.Copy(Data[index].Owner.UserName),
                    EmailConfirmed = Data[index].Owner.EmailConfirmed,
                    PasswordHash = string.Copy(Data[index].Owner.PasswordHash)
                }
            };
        }

        /// <summary>
        /// Get some mock todo entities. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which todos we want</param>
        /// <returns>A list with some mocked todo entities</returns>
        public static IEnumerable<Todo> GetSome(params int[] indices)
        {
            var copyList = new List<Todo>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock todo entities. It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked todo entity</returns>
        public static IEnumerable<Todo> GetAll()
        {
            var copyList = new List<Todo>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }
    }
}

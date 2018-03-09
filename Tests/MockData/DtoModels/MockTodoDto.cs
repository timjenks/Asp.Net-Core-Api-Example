#region Imports

using System;
using System.Collections.Generic;
using TodoApi.Models.DtoModels;

#endregion

namespace Tests.MockData.DtoModels
{
    /// <summary>
    /// A static coollection for mock todo dtos.
    /// </summary>
    public static class MockTodoDto
    {
        #region Data

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly TodoDto[] Data =
        {
            // Index: 0
            new TodoDto
            {
                Id = 3333,
                Due = new DateTime(2010, 2, 10, 18, 0, 0),
                Description = "Step right up"
            },
            // Index: 1
            new TodoDto
            {
                Id = 2525,
                Due = new DateTime(2020, 3, 11, 19, 30, 0),
                Description = "Tango till they're sore"
            }
        };

        #endregion

        #region Getters

        /// <summary>
        /// Get a single mock todo dto. The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock todo dto to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock todo dto with given index in array</returns>
        public static TodoDto Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new TodoDto
            {
                Id = Data[index].Id,
                Due = Data[index].Due,
                Description = string.Copy(Data[index].Description)
            };
        }

        /// <summary>
        /// Get some mock todo dtos. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which todos we want</param>
        /// <returns>A list with some mocked todo dtos</returns>
        public static IEnumerable<TodoDto> GetSome(params int[] indices)
        {
            var copyList = new List<TodoDto>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock todo dtos. It will only contain copied instances. 
        /// </summary>
        /// <returns>A list with every mocked todo dtos</returns>
        public static IEnumerable<TodoDto> GetAll()
        {
            var copyList = new List<TodoDto>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }

        #endregion
    }
}

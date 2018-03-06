using System;
using System.Collections.Generic;
using Tests.MockData.EntityModels;
using TodoApi.Models.ViewModels;

namespace Tests.MockData.ViewModels
{
    /// <summary>
    /// A static collection of mock view models for editing todos.
    /// </summary>
    public static class MockEditTodoViewModel
    {
        #region Data

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly EditTodoViewModel[] Data =
        {
            // Index: 0
            new EditTodoViewModel
            {
                Id = MockTodos.Get(3).Id,
                Description = "Dance This Mess Around",
                Due = new DateTime(2015, 5, 7, 10, 15, 0)
            },
            // Index: 1
            new EditTodoViewModel
            {
                Id = MockTodos.Get(14).Id,
                Description = "Rip Her to Shreds",
                Due = new DateTime(2019, 11, 17, 22, 0, 0)
            },
        };

        #endregion

        #region Getters

        /// <summary>
        /// Get a single mock view model for todo editing. 
        /// The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock view model to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock view model with given index in array</returns>
        public static EditTodoViewModel Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new EditTodoViewModel
            {
                Id = Data[index].Id,
                Description = string.Copy(Data[index].Description),
                Due = Data[index].Due
            };
        }

        /// <summary>
        /// Get some mock todo editing view models. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which todo editing view models we want</param>
        /// <returns>A list with some mocked todo editing view models</returns>
        public static IEnumerable<EditTodoViewModel> GetSome(params int[] indices)
        {
            var copyList = new List<EditTodoViewModel>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock view models for todo editing. 
        /// It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked view model for todo editing</returns>
        public static IEnumerable<EditTodoViewModel> GetAll()
        {
            var copyList = new List<EditTodoViewModel>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }

        #endregion
    }
}

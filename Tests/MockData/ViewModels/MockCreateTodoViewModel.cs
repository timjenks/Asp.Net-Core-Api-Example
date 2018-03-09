#region Imports

using System;
using System.Collections.Generic;
using TodoApi.Models.ViewModels;

#endregion

namespace Tests.MockData.ViewModels
{
    /// <summary>
    /// A static collection of mock view models for creating todos.
    /// </summary>
    public static class MockCreateTodoViewModel
    {
        #region Data

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly CreateTodoViewModel[] Data =
        {
            // Index: 0
            new CreateTodoViewModel
            {
                Description = "Create an example api in dotnet core",
                Due = new DateTime(2023, 3, 17, 16, 45, 0)
            },
            // Index: 1
            new CreateTodoViewModel
            {
                Description = "Create an example api in flask",
                Due = new DateTime(2026, 7, 23, 13, 0, 0)
            }
        };

        #endregion

        #region Getters

        /// <summary>
        /// Get a single mock view model for todo creating. 
        /// The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock view model to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock view model with given index in array</returns>
        public static CreateTodoViewModel Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new CreateTodoViewModel
            {
                Description = string.Copy(Data[index].Description),
                Due = Data[index].Due
            };
        }

        /// <summary>
        /// Get some mock todo creating view models. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which todo creating view models we want</param>
        /// <returns>A list with some mocked todo creating view models</returns>
        public static IEnumerable<CreateTodoViewModel> GetSome(params int[] indices)
        {
            var copyList = new List<CreateTodoViewModel>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock view models for todo creation. 
        /// It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked view model for todo creating</returns>
        public static IEnumerable<CreateTodoViewModel> GetAll()
        {
            var copyList = new List<CreateTodoViewModel>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }

        #endregion
    }
}

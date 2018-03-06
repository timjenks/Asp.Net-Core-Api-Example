using System;
using System.Collections.Generic;
using Tests.MockData.EntityModels;
using TodoApi.Models.ViewModels;

namespace Tests.MockData.ViewModels
{
    /// <summary>
    /// A static collection of mock view model data for registering a new user.
    /// </summary>
    public static class MockRegisterViewModel
    {
        #region Data

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly RegisterViewModel[] Data =
        {
            // Index: 0
            new RegisterViewModel
            {
                Email = "emmy@uni-goettingen.de",
                Password = MockApplicationUsers.UniversalPassword,
                Name = "Emmy Noether"
            },
            // Index: 1
            new RegisterViewModel
            {
                Email = "sofia@uni-goettingen.de",
                Password = MockApplicationUsers.UniversalPassword,
                Name = "Sofia Kovalevskaya"
            },
        };

        #endregion

        #region Getters

        /// <summary>
        /// Get a single mock view model for user registration.
        /// The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock view model to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock view model with given index in array</returns>
        public static RegisterViewModel Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new RegisterViewModel
            {
                Email = string.Copy(Data[index].Email),
                Password = string.Copy(Data[index].Password),
                Name = string.Copy(Data[index].Name)
            };
        }

        /// <summary>
        /// Get some mock user registration view models. 
        /// It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which user registration view models we want</param>
        /// <returns>A list with some mocked user registration view models</returns>
        public static IEnumerable<RegisterViewModel> GetSome(params int[] indices)
        {
            var copyList = new List<RegisterViewModel>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock view models for user registration.
        /// It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked view model for user registration</returns>
        public static IEnumerable<RegisterViewModel> GetAll()
        {
            var copyList = new List<RegisterViewModel>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }

        #endregion
    }
}

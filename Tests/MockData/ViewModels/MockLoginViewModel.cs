using System;
using System.Collections.Generic;
using Tests.MockData.EntityModels;
using TodoApi.Models.ViewModels;

namespace Tests.MockData.ViewModels
{
    /// <summary>
    /// A static collection of mock view model data for login with an existing user.
    /// </summary>
    public static class MockLoginViewModel
    {
        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly LoginViewModel[] Data =
        {
            // Index: 0
            new LoginViewModel
            {
                Email = MockApplicationUsers.Get(3).Email,
                Password = MockApplicationUsers.UniversalPassword
            },
            // Index: 1
            new LoginViewModel
            {
                Email = MockApplicationUsers.Get(7).Email,
                Password = MockApplicationUsers.UniversalPassword
            },
        };

        /// <summary>
        /// Get a single mock view model for login info.
        /// The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock view model to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock view model with given index in array</returns>
        public static LoginViewModel Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new LoginViewModel
            {
                Email = string.Copy(Data[index].Email),
                Password = string.Copy(Data[index].Password)
            };
        }

        /// <summary>
        /// Get some mock user login view models. 
        /// It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which user login view models we want</param>
        /// <returns>A list with some mocked user login view models</returns>
        public static IEnumerable<LoginViewModel> GetSome(params int[] indices)
        {
            var copyList = new List<LoginViewModel>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock view models for user login.
        /// It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked view model for user login</returns>
        public static IEnumerable<LoginViewModel> GetAll()
        {
            var copyList = new List<LoginViewModel>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }
    }
}

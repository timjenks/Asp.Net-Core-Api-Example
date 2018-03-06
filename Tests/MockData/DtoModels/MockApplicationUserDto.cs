using System;
using System.Collections.Generic;
using TodoApi.Models.DtoModels;

namespace Tests.MockData.DtoModels
{
    /// <summary>
    /// A static coollection for mock user dtos.
    /// </summary>
    public static class MockApplicationUserDto
    {
        #region Data

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly ApplicationUserDto[] Data =
        {
            // Index: 0
            new ApplicationUserDto
            {
                Id = "0d31cf64-ad31-4f72-9776-036596c08e56",
                Email = "megas@a-bleikum-nattkjolum.is",
                Name = "Megas"
            },
            // Index: 1
            new ApplicationUserDto
            {
                Id = "ea177560-e4ed-4af8-92a9-404d0344b741",
                Email = "theyr@mjotvidur-maer.is",
                Name = "Theyr"
            },
        };

        #endregion

        #region Getters

        /// <summary>
        /// Get a single mock user dto. The instance is copied from the array.
        /// </summary>
        /// <param name="index">The index of the mock user dto to get</param>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <returns>The mock user dto with given index in array</returns>
        public static ApplicationUserDto Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new ApplicationUserDto
            {
                Id = string.Copy(Data[index].Id),
                Name = string.Copy(Data[index].Name),
                Email = string.Copy(Data[index].Email)
            };
        }

        /// <summary>
        /// Get some mock user dtos. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which users we want</param>
        /// <returns>A list with some mocked user dtos</returns>
        public static IEnumerable<ApplicationUserDto> GetSome(params int[] indices)
        {
            var copyList = new List<ApplicationUserDto>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock user dtos. It will only contain copied instances. 
        /// </summary>
        /// <returns>A list with every mocked user dtos</returns>
        public static IEnumerable<ApplicationUserDto> GetAll()
        {
            var copyList = new List<ApplicationUserDto>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }

        #endregion
    }
}

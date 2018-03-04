using System;
using System.Collections.Generic;
using TodoApi.Models.EntityModels;

namespace Tests.MockData.EntityModels
{
    /// <summary>
    /// A static coollection for mock user entities.
    /// </summary>
    public static class MockApplicationUsers
    {
        /// <summary>
        /// The shared password for all users.
        /// </summary>
        public const string UniversalPassword = "Re-Animat0r";

        /// <summary>
        /// A hashed version of the universal password.
        /// </summary>
        private const string UniversalPasswordHash = "AQAAAAEAACcQAAAAEHqsnpvAfTR7VPaobD/BbJTOg3hoUo93yfQ77mTD1YlfT5EUkvw+EdLFV2JSdd4GVQ==";

        /// <summary>
        /// A helper for building instances.
        /// </summary>
        /// <param name="id">The unique identifier (uuid)</param>
        /// <param name="name">The name of the user (not UserName)</param>
        /// <param name="email">The email of ther user</param>
        /// <returns>A newly constructed instance of a user</returns>
        private static ApplicationUser ConstructUser(string id, string name, string email)
        {
            return new ApplicationUser
            {
                Id = id,
                Name = name,
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                PasswordHash = UniversalPasswordHash
            };
        }

        /// <summary>
        /// Array of Mock data.
        /// </summary>
        private static readonly ApplicationUser[] Data =
        {
            // User 0
            ConstructUser
            (
                "f093c069-c93a-4cf1-9abe-cff674b93989", 
                "John Carpenter",
                "carpenter@john.com"
            ),
            // User 1
            ConstructUser
            (
                "bf1f782e-1847-4adc-8da8-31a5125c2a5e", 
                "John Carmack",
                "carmack@john.com"
            ),
            // User 2
            ConstructUser
            (
                "789c0f29-1abd-45f9-a9ce-09151fa5ae99",
                "John Rambo",
                "rambo@john.com"
            ),
            // User 3
            ConstructUser
            (
                "2706a570-ed3c-4fcf-8c7b-8985dcba844b",
                "John Cleese",
                "cleese@john.com"
            ),
            // User 4
            ConstructUser
            (
                "c995b669-c620-43c4-b238-7356b44d6932",
                "John Goodman",
                "goodman@john.com"
            ),
            // User 5
            ConstructUser(
                "84e434fe-8e9e-4b42-b539-13b276f3f250",
                "John Malkovich",
                "malkovich@john.com"
            ),
            // User 6
            ConstructUser
            (
                "fd82177c-9e4b-4723-89f3-854ed0a45006",
                "John the Revelator",
                "therevelator@john.com"
            ),
            // User 7
            ConstructUser
            (
                "08417a3d-d67f-4fae-a853-b675870d22c0",
                "John Adams",
                "adams@john.com"
            ),
            // User 8
            ConstructUser(
                "6acc6c7b-341a-46c9-af46-31d00db46166",
                "John Stockton",
                "stockton@john.com"
            ),
            // User 9
            ConstructUser
            (
                "dae85cd3-2664-4270-bcda-c37ac75e07dd",
                "John Candy",
                "candy@john.com"
            ),
        };

        /// <summary>
        /// Get a single mock user entity. The instance is copied from the array.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when index is out of bounds</exception>
        /// <param name="index">The index of the mock user entity to get</param>
        /// <returns>The mock user entity with given index in array</returns>
        public static ApplicationUser Get(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                throw new ArgumentException("You are accessing test data that does not exist");
            }
            return new ApplicationUser
            {
                Id = string.Copy(Data[index].Id),
                Name = string.Copy(Data[index].Name),
                Email = string.Copy(Data[index].Email),
                UserName = string.Copy(Data[index].UserName),
                EmailConfirmed = Data[index].EmailConfirmed,
                PasswordHash = string.Copy(Data[index].PasswordHash),
                NormalizedEmail = Data[index].Email.ToUpper(),
                NormalizedUserName = Data[index].Email.ToUpper()
            };
        }

        /// <summary>
        /// Get some mock user entities. It will only contain copied instances.
        /// </summary>
        /// <param name="indices">Which users we want</param>
        /// <returns>A list with some mocked user entities</returns>
        public static IEnumerable<ApplicationUser> GetSome(params int[] indices)
        {
            var copyList = new List<ApplicationUser>(indices.Length);
            foreach (var index in indices)
            {
                copyList.Add(Get(index));
            }
            return copyList;
        }

        /// <summary>
        /// Get all mock user entities. It will only contain copied instances.
        /// </summary>
        /// <returns>A list with every mocked user entity</returns>
        public static IEnumerable<ApplicationUser> GetAll()
        {
            var copyList = new List<ApplicationUser>(Data.Length);
            for (var i = 0; i < Data.Length; i++)
            {
                copyList.Add(Get(i));
            }
            return copyList;
        }
    }
}

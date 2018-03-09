#region Imports

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Tests.MockData.EntityModels
{
    /// <summary>
    /// A static class containing the links between users and roles.
    /// </summary>
    public static class MockUserRoles
    {
        #region Data

        /// <summary>
        /// A dictionary between user ids and their role.
        /// </summary>
        private static readonly Dictionary<string, IdentityUserRole<string>> Data = InitialzieData();

        #endregion

        #region Getters

        /// <summary>
        /// Get the UserRole for the userId. In this scenario, each one has only one
        /// even though the relation is many-to-many by default in dot net.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <exception cref="ArgumentException">Thrown when user id is not found</exception>
        /// <returns>User role entity for user</returns>
        public static IdentityUserRole<string> GetUserRoleForUser(string userId)
        {
            if (!Data.ContainsKey(userId))
            {
                throw new ArgumentException("UserId does not exist in mock data!");
            }
            return Data[userId];
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Dictionary builder.
        /// </summary>
        /// <returns>A dictionary with the first user as admin, rest as users</returns>
        private static Dictionary<string, IdentityUserRole<string>> InitialzieData()
        {
            var allUsers = MockApplicationUsers.GetAll().ToArray();
            var mapInitSize = (int)(allUsers.Count() * 1.5);
            var hashMap = new Dictionary<string, IdentityUserRole<string>>(mapInitSize);
            var first = true;
            foreach (var user in allUsers)
            {
                if (first)
                {
                    first = false;
                    hashMap[user.Id] = new IdentityUserRole<string>
                    {
                        UserId = user.Id,
                        RoleId = MockRoles.Admin.Id
                    };
                }
                else
                {
                    hashMap[user.Id] = new IdentityUserRole<string>
                    {
                        UserId = user.Id,
                        RoleId = MockRoles.User.Id
                    };
                }
            }
            return hashMap;
        }

        #endregion
    }
}

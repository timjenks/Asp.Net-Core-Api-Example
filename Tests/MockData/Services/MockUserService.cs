#region Imports

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models.DtoModels;
using TodoApi.Services.Interfaces;

#endregion

namespace Tests.MockData.Services
{
    /// <inheritdoc />
    /// <summary>
    /// A mock of user service.
    /// </summary>
    public class MockUserService : IUserService
    {
        #region Method Variables

        /// <summary>
        /// A method to control what GetAllUsersOrderedByNameAsync does. Needs to be set in test.
        /// </summary>
        public Func<IEnumerable<ApplicationUserDto>> MGetAllUsersOrderedByNameAsync { get; set; }

        /// <summary>
        /// A method to control what GetUserByIdAsync does. Needs to be set in test.
        /// </summary>
        public Func<string, ApplicationUserDto> MGetUserByIdAsync { get; set; }

        /// <summary>
        /// A method to control what RemoveUserByIdAsync does. Needs to be set in test.
        /// </summary>
        public Action<string> MRemoveUserByIdAsync { get; set; }

        #endregion

        #region Methods implmented

        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationUserDto>> GetAllUsersOrderedByNameAsync()
        {
            await Task.Run(() => { });
            return MGetAllUsersOrderedByNameAsync();
        }

        /// <inheritdoc />
        public async Task<ApplicationUserDto> GetUserByIdAsync(string userId)
        {
            await Task.Run(() => { });
            return MGetUserByIdAsync(userId);
        }

        /// <inheritdoc />
        public async Task RemoveUserByIdAsync(string userId)
        {
            await Task.Run(() => { });
            MRemoveUserByIdAsync(userId);
        }

        #endregion
    }
}

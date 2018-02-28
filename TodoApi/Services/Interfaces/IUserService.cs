using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models.DtoModels;

namespace TodoApi.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        Task<ApplicationUserDto> GetUserByIdAsync(string userId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId">TODO</param>
        /// <returns>TODO</returns>
        Task RemoveUserByIdAsync(string userId);
    }
}

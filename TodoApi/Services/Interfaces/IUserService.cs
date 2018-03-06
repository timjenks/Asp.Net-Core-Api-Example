using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models.DtoModels;

namespace TodoApi.Services.Interfaces
{
    /// <summary>
    /// An interface for a user service that defines 
    /// what methods such a service must implement.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get a single user.
        /// </summary>
        /// <param name="userId">The id of the user to get</param>
        /// <returns>The user dto of the requested user</returns>
        Task<ApplicationUserDto> GetUserByIdAsync(string userId);

        /// <summary>
        /// Get all users ordered by name.
        /// </summary>
        /// <returns>A list of user dtos</returns>
        Task<IEnumerable<ApplicationUserDto>> GetAllUsersOrderedByNameAsync();

        /// <summary>
        /// Remmve a specific user.
        /// </summary>
        /// <param name="userId">The id of the user to remove</param>
        Task RemoveUserByIdAsync(string userId);
    }
}

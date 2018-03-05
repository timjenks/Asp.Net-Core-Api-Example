using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using TodoApi.Models.EntityModels;
using TodoApi.Services.Interfaces;

namespace TodoApi.Services
{
    /// <inheritdoc />
    /// <summary>
    /// The user service that the production API uses.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppDataContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// A constructor that injects AppDataContext, UserManager and MemoryCache.
        /// </summary>
        /// <param name="db">A DbContext to access a database</param>
        /// <param name="userManager">User manager for Application users</param>
        /// <param name="cache">A cache memory to utilize RAM to save db queries</param>
        public UserService
        (
            AppDataContext db,
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache
        )
        {
            _db = db;
            _userManager = userManager;
            _cache = cache;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user is not found</exception>
        public async Task<ApplicationUserDto> GetUserByIdAsync(string userId)
        {
            var cacheKey = CacheConstants.GetSingleUserCacheKey(userId);
            if (!_cache.TryGetValue(cacheKey, out ApplicationUser user))
            {
                user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new UserNotFoundException();
                }
                _cache.Set(cacheKey, user, CacheConstants.GetDefaultCacheOptions());
            }
            return new ApplicationUserDto(user);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationUserDto>> GetAllUsersOrderedByNameAsync()
        {
            var cacheKey = CacheConstants.AllUsersCacheKey;
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ApplicationUserDto> users))
            {
                users = await _userManager
                        .Users
                        .Select(u => new ApplicationUserDto(u))
                        .OrderBy(u => u.Name)
                        .ToListAsync();
                _cache.Set(cacheKey, users, CacheConstants.GetDefaultCacheOptions());
            }
            return users;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user is not found</exception>
        /// <exception cref="RemoveUserFailedException">Thrown if we fail to remove user</exception>
        public async Task RemoveUserByIdAsync(string userId)
        {
            var userToRemove = await _userManager.FindByIdAsync(userId);
            if (userToRemove == null)
            {
                throw new UserNotFoundException();
            }
            await _db.Entry(userToRemove).Collection(u => u.Todos).LoadAsync();

            // Clear cached todos owned by the user
            foreach (Todo todo in userToRemove.Todos)
            {
                _cache.Remove(CacheConstants.GetSingleTodoCacheKey(
                    todo.Id, userToRemove.Id));
                _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(
                    userToRemove.Id, todo.Due.Date));
            }
            _cache.Remove(CacheConstants.GetAllTodosCacheKey(userToRemove.Id));
            _db.RemoveRange(userToRemove.Todos);

            await _userManager.DeleteAsync(userToRemove);
            await _db.SaveChangesAsync();

            // Clear user if cached and all user list.
            _cache.Remove(CacheConstants.GetSingleUserCacheKey(userToRemove.Id));
            _cache.Remove(CacheConstants.AllUsersCacheKey);
        }
    }
}

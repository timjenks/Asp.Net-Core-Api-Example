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

    /// <summary>
    /// TODO
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppDataContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="db">TODO</param>
        /// <param name="userManager">TODO</param>
        /// <param name="cache">TODO</param>
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
        public async Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync()
        {
            var cacheKey = CacheConstants.AllUsersCacheKey;
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ApplicationUserDto> users))
            {
                users = await _userManager.Users.Select(u => new ApplicationUserDto(u)).ToListAsync();
                _cache.Set(cacheKey, users, CacheConstants.GetDefaultCacheOptions());
            }
            return users;
        }

        /// <inheritdoc />
        /// <exception cref="TODO">TODO</exception>
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
        /// <exception cref="TODO">TODO</exception>
        /// <exception cref="TODO">TODO</exception>
        public async Task RemoveUserByIdAsync(string userId)
        {
            var userToRemove = await _userManager.FindByIdAsync(userId);
            if (userToRemove == null)
            {
                throw new UserNotFoundException();
            }
            await _db.Entry(userToRemove).Collection(u => u.Todos).LoadAsync();

            foreach (Todo todo in userToRemove.Todos)
            {
                _cache.Remove(CacheConstants.GetSingleTodoCacheKey(todo.Id, userToRemove.Id));
                _cache.Remove(CacheConstants.GetAllTodosForDayCacheKey(userToRemove.Id, todo.Due.Date));
            }
            _cache.Remove(CacheConstants.GetAllTodosCacheKey(userToRemove.Id));
            _db.RemoveRange(userToRemove.Todos);

            var res = await _userManager.DeleteAsync(userToRemove);
            if (!res.Succeeded)
            {
                throw new RemoveUserFailedException();
            }
            await _db.SaveChangesAsync();

            _cache.Remove(CacheConstants.GetSingleUserCacheKey(userToRemove.Id));
            _cache.Remove(CacheConstants.AllUsersCacheKey);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models.DtoModels;
using TodoApi.Models.EntityModels;

namespace TodoApi.Services.UserServices
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
            return await _userManager.Users.Select(u => new ApplicationUserDto(u)).ToListAsync();
        }

        /// <inheritdoc />
        /// <exception cref="TODO">TODO</exception>
        public async Task<ApplicationUserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
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
            _db.RemoveRange(userToRemove.Todos);
            var res = await _userManager.DeleteAsync(userToRemove);
            if (!res.Succeeded)
            {
                throw new RemoveUserFailedException();
            }
            await _db.SaveChangesAsync();
        }
    }
}

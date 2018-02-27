using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Constants;
using TodoApi.Exceptions;
using TodoApi.Models.EntityModels;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.AccountServices
{
    /// <summary>
    /// TODO
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// TODO
        /// </summary>
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <inheritdoc />
        /// <exception cref="LoginFailException">When user is not found</exception>
        public async Task<string> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                throw new LoginFailException();
            }
            var appUser = await _userManager.Users.SingleOrDefaultAsync(r => r.UserName == model.Email);
            return await GenerateJwtToken(appUser);
        }

        /// <inheritdoc />
        /// <exception cref="RegisterFailException">When email already in use</exception>
        public async Task<string> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new RegisterFailException();
            }
            await _signInManager.SignInAsync(user, false);
            await _userManager.AddToRoleAsync(user, Roles.User);
            return await GenerateJwtToken(user);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user">TODO</param>
        /// <returns>TODO</returns>
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = await getClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["TokenExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Issuer"],
                _configuration["Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user">TODO</param>
        /// <returns>TODO</returns>
        private async Task<List<Claim>> getClaims(ApplicationUser user)
        {
            var roleClaims = (await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role));
            var claims = new List<Claim>(4 + roleClaims.Count())
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(roleClaims);
            return claims;
        }
    }
}

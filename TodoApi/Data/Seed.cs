#region Imports

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TodoApi.Models.EntityModels;
using TodoApi.Utils.Constants;

#endregion

namespace TodoApi.Data
{
    /// <summary>
    /// A staticcclass containing data seeds methods.
    /// </summary>
    public static class Seed
    {
        /// <summary>
        /// Creates the roles 'Admin' and 'User' if they do not
        /// exist as well as an admin user.
        /// </summary>
        /// <param name="serviceProvider">TODO</param>
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var roleName in new[] { Roles.User, Roles.Admin })
            {
                if (!(await roleManager.RoleExistsAsync(roleName)))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var findUser = await userManager.FindByEmailAsync("kong@mail.com");
            if (findUser == null)
            {
                var admin = new ApplicationUser()
                {
                    Name = "King Kong",
                    UserName = "kong@mail.com",
                    Email = "kong@mail.com"
                };
                var password = "D0nkey_K0ng";
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}

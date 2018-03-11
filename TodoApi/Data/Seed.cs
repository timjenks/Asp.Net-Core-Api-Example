﻿#region Imports

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        public static async Task CreateAll(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDataContext>();

#if DEBUG
            await ClearDatabase(roleManager, userManager, context);
#endif
            await FillDatabase(roleManager, userManager, context);
        }

        #region Helpers

        #region Database

        private static async Task FillDatabase(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, AppDataContext context)
        {
            await CreateRoles(roleManager);
            var users = await CreateUsers(userManager);
#if DEBUG
            await CreateTodo(context, users);
#endif
        }

        private static async Task ClearDatabase(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, AppDataContext context)
        {
            context.RemoveRange(await context.Todo.ToListAsync());
            await context.SaveChangesAsync();
            foreach (var user in await userManager.Users.ToListAsync())
            {
                await userManager.DeleteAsync(user);
            }
            await context.SaveChangesAsync();
            foreach (var role in await roleManager.Roles.ToListAsync())
            {
                await roleManager.DeleteAsync(role);
            }
            await context.SaveChangesAsync();
        }

        #endregion

        #region Roles

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in new[] { Roles.User, Roles.Admin })
            {
                await CreateRole(roleManager, roleName);
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!(await roleManager.RoleExistsAsync(roleName)))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

#endregion

        #region Users

        private static async Task<Dictionary<string, ApplicationUser>> CreateUsers(UserManager<ApplicationUser> userManager)
        {
            var dictionary = new Dictionary<string, ApplicationUser>
            {
                { "kong@mail.com", await CreateAdmin(userManager, "King Kong", "kong@mail.com", "D0nkey_K0ng") }
            };
#if DEBUG
            dictionary.Add
            (
                "tyrion@mail.com", 
                await CreateUser(userManager, "Tyrion Lannister", "tyrion@mail.com", "D0nkey_K0ng")
            );
            dictionary.Add
            (
                "jonsteinn@gmail.com", 
                await CreateUser(userManager, "Jón Steinn Elíasson", "jonsteinn@gmail.com", "D0nkey_K0ng")
            );
#endif
            return dictionary;
        }

        private static async Task<ApplicationUser> CreateUser(
            UserManager<ApplicationUser> userManager,
            string name, string email, string password)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser != null) return findUser;
            var user = new ApplicationUser()
            {
                Name = name,
                UserName = email,
                Email = email
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User);
            }
            return user;
        }

        private static async Task<ApplicationUser> CreateAdmin(
            UserManager<ApplicationUser> userManager,
            string name, string email, string password)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser != null) return findUser;
            var user = new ApplicationUser()
            {
                Name = name,
                UserName = email,
                Email = email
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            return user;
        }

        #endregion

        #region Todo

        private static async Task CreateTodo(AppDataContext context, Dictionary<string, ApplicationUser> users)
        {
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("kong@mail.com", null),
                "Grab a girl",
                new DateTime(2019, 12, 12, 15, 0, 0)
            );
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("kong@mail.com", null),
                "Climb the Empire State building",
                new DateTime(2019, 12, 12, 22, 45, 0)
            );
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("tyrion@mail.com", null),
                "Slap a king",
                new DateTime(2020, 3, 7, 12, 30, 0)
            );
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("tyrion@mail.com", null),
                "Drink self to death",
                new DateTime(2021, 9, 11, 19, 0, 0)
            );
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("jonsteinn@gmail.com", null),
                "Make coffee",
                new DateTime(2018, 2, 4, 20, 0, 0)
            );
            await CreateTodo
            (
                context,
                users.GetValueOrDefault("jonsteinn@gmail.com", null),
                "Drink coffee",
                new DateTime(2019, 4, 2, 17, 0, 0)
            );
            await context.SaveChangesAsync();
        }

        private static async Task CreateTodo(AppDataContext context,
            ApplicationUser owner, string description, DateTime due)
        {
            if (owner == null) return;
            var findTodo = await context.Todo.SingleOrDefaultAsync(
                z => z.Description == description && z.Due == due && z.Owner.Id == owner.Id);
            if (findTodo == null)
            {
                var todo = new Todo
                {
                    Due = due,
                    Description = description,
                    Owner = owner
                };
                await context.AddAsync(todo);
            }
        }

        #endregion

        #endregion
    }
}

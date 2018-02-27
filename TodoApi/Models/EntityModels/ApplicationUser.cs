using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TodoApi.Models.ViewModels;

namespace TodoApi.Models.EntityModels
{
    /// <summary>
    /// The user as stored in the database.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// No argument constructor.
        /// </summary>
        public ApplicationUser() { }

        /// <summary>
        /// Map a view model to entity.
        /// </summary>
        /// <param name="model">A view model containing email and name</param>
        public ApplicationUser(RegisterViewModel model)
        {
            Email = string.Copy(model.Email);
            UserName = string.Copy(model.Email);
            Name = string.Copy(model.Name);
        }

        /// <summary>
        /// The name of the user (UserName field is used for email).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A colletion of all todos belonging to this user.
        /// </summary>
        public ICollection<Todo> Todos { get; set; }
    }
}

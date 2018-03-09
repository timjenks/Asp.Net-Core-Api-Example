#region Imports

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TodoApi.Models.ViewModels;

# endregion

namespace TodoApi.Models.EntityModels
{
    /// <inheritdoc />
    public class ApplicationUser : IdentityUser
    {
        #region Fields

        /// <summary>
        /// The name of the user (UserName field is used for email).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A colletion of all todos belonging to this user.
        /// </summary>
        public ICollection<Todo> Todos { get; set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        public ApplicationUser() { }

        /// <inheritdoc />
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

        #endregion
    }
}

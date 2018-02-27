using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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
        /// The name of the user (UserName field is used for email).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A colletion of all todos belonging to this user.
        /// </summary>
        public ICollection<Todo> Todos { get; set; }
    }
}

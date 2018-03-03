using TodoApi.Models.EntityModels;

namespace TodoApi.Models.DtoModels
{
    /// <summary>
    /// The data transfer object model for users, as we want our clients to see them.
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// A no argument constructor.
        /// </summary>
        public ApplicationUserDto() { }

        /// <summary>
        /// Creates a corresponding dto of a entity model.
        /// </summary>
        /// <param name="user">The entity model of a user</param>
        public ApplicationUserDto(ApplicationUser user)
        {
            Email = string.Copy(user.Email);
            Name = string.Copy(user.Name);
            Id = string.Copy(user.Id);
        }

        /// <summary>
        /// A unique identifier for a user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A unique email for a user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The name of a user.
        /// </summary>
        public string Name { get; set; }
    }
}

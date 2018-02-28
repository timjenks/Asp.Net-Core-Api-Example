using TodoApi.Models.EntityModels;

namespace TodoApi.Models.DtoModels
{
    /// <summary>
    /// TODO
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// TODO
        /// </summary>
        public ApplicationUserDto() { }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user">TODO</param>
        public ApplicationUserDto(ApplicationUser user)
        {
            Email = string.Copy(user.Email);
            Name = string.Copy(user.Name);
            Id = string.Copy(user.Id);
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Name { get; set; }
    }
}

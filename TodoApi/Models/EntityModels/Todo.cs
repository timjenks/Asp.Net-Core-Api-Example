using System;

namespace TodoApi.Models.EntityModels
{
    /// <summary>
    /// Todo as stored in the database.
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// No argument constructor.
        /// </summary>
        public Todo() { }

        /// <summary>
        /// A unique identifier for todos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A time limit to complete the todo.
        /// </summary>
        public DateTime Due { get; set; }

        /// <summary>
        /// A description of what to do.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The owner of this todo.
        /// </summary>
        public ApplicationUser Owner { get; set; }
    }
}

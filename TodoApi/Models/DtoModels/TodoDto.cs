using System;
using TodoApi.Models.EntityModels;

namespace TodoApi.Models.DtoModels
{
    /// <summary>
    /// The data transfer object model for todos, as we want our clients to see them.
    /// </summary>
    public class TodoDto
    {
        /// <summary>
        /// No argument constructor.
        /// </summary>
        public TodoDto() { }

        /// <summary>
        /// Converts a todo entity model to its corresponding data transfer object.
        /// </summary>
        /// <param name="evt">An entity model for a todo</param>
        public TodoDto(Todo todo)
        {
            Id = todo.Id;
            Due = todo.Due;
            Description = string.Copy(todo.Description);
        }

        /// <summary>
        /// A unique identifier for the todo.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The due date of the todo.
        /// </summary> 
        public DateTime Due { get; set; }

        /// <summary>
        /// A description of the todo.
        /// </summary>
        public string Description { get; set; }
    }
}

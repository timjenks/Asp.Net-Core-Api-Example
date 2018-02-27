using System;
using System.ComponentModel.DataAnnotations;
using TodoApi.Constants;

namespace TodoApi.Models.ViewModels
{
    /// <summary>
    /// The expected attributes when editing an existing todo.
    /// </summary>
    public class EditTodoViewModel
    {
        [Required(ErrorMessage = ErrorMessages.TodoIdRequired)]
        public int? Id { get; set; }

        /// <summary>
        /// The time when the todo is suppose to be done.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.TodoDueRequired)]
        public DateTime? Due { get; set; }


        /// <summary>
        /// A description of the todo.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.TodoDescriptionRequired)]
        [MaxLength(TodoLimits.DescriptionMaxLength, ErrorMessage = ErrorMessages.TodoDescriptionMaxLength)]
        public string Description { get; set; }
    }
}

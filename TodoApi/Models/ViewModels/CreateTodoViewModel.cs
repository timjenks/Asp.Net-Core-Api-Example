using System;
using System.ComponentModel.DataAnnotations;
using TodoApi.Constants;

namespace TodoApi.Models.ViewModels
{
    public class CreateTodoViewModel
    {
        /// <summary>
        /// The time when the todo is suppose to be done.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.TodoDueRequired)]
        public DateTime? Due { get; set; }


        /// <summary>
        /// A description of the todo.
        /// </summary>
        [Required(ErrorMessage = ErrorMessages.TodoDescriptionRequired)]
        [MaxLength(TodoLimits.MaxLength, ErrorMessage = ErrorMessages.TodoDescriptionMaxLength)]
        public string Description { get; set; }
    }
}

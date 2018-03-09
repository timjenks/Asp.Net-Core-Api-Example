#region Imports

using System;
using System.ComponentModel.DataAnnotations;
using TodoApi.Utils.Constants;

#endregion

namespace TodoApi.Models.ViewModels
{
    /// <summary>
    /// The expected attributes when creating a new todo.
    /// </summary>
    public class CreateTodoViewModel
    {
        #region Fields

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

        #endregion
    }
}
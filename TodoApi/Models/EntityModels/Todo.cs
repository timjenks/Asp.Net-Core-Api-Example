﻿#region Imports

using System;
using TodoApi.Models.ViewModels;

#endregion

namespace TodoApi.Models.EntityModels
{
    /// <summary>
    /// Todo as stored in the database.
    /// </summary>
    public class Todo
    {
        #region Fields

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

        #endregion

        #region Constructors

        /// <summary>
        /// No argument constructor.
        /// </summary>
        public Todo() { }

        /// <summary>
        /// Map a view model to a corresponding entity.
        /// </summary>
        /// <param name="model">A model for creating todos</param>
        /// <param name="user">The owner of this todo</param>
        public Todo(CreateTodoViewModel model, ApplicationUser user)
        {
            //Due = model.Due.Value;
            if (model.Due != null) Due = model.Due.Value;
            Description = string.Copy(model.Description);
            Owner = user;
        }

        #endregion

        /// <summary>
        /// Edit this entity using a editing view model.
        /// </summary>
        /// <param name="model">A view model containing changed fields</param>
        public void Edit(EditTodoViewModel model)
        {
            if (model.Due != null) Due = model.Due.Value;
            Description = string.Copy(model.Description);
        }
    }
}

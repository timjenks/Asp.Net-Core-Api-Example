﻿#region Imports

using System;

#endregion

namespace TodoApi.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown in service when todo is not found.
    /// </summary>
    public class TodoNotFoundException : Exception { }
}

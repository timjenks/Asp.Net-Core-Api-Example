#region Imports

using System;

#endregion

namespace TodoApi.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown in service when user is not found.
    /// </summary>
    public class UserNotFoundException : Exception { }
}

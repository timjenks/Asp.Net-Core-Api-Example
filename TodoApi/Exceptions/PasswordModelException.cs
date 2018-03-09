#region Imports

using System;

#endregion

namespace TodoApi.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown when model state of password is caught at service level.
    /// </summary>
    public class PasswordModelException : Exception { }
}

#region Imports

using System;

#endregion

namespace TodoApi.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown in service layer if login fails.
    /// </summary>
    public class LoginFailException : Exception { }
}

﻿using System.Text;

namespace Tests.Helpers.Json
{
    public static class StringJsonBuilder
    {
        /// <summary>
        /// Create a json string body for registration.
        /// </summary>
        /// <param name="name">The name of the person registering</param>
        /// <param name="email">The email of the person registering</param>
        /// <param name="password">The password of the person registering</param>
        /// <returns>A json string</returns>
        public static string RegisterJsonBody(string name, string email, string password)
        {
            return new StringBuilder(36 + name.Length + email.Length + password.Length)
                .Append('{')
                .Append('"')
                .Append("Email")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(email)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Password")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(password)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Name")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(name)
                .Append('"')
                .Append('}')
                .ToString();
        }

        /// <summary>
        /// Create a json string body for signing in.
        /// </summary>
        /// <param name="email">The email of the person signing in</param>
        /// <param name="password">The password of the person signing in</param>
        /// <returns>A json string</returns>
        public static string LoginJsonBody(string email, string password)
        {
            return new StringBuilder(26 + email.Length + password.Length)
                .Append('{')
                .Append('"')
                .Append("Email")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(email)
                .Append('"')
                .Append(',')
                .Append('"')
                .Append("Password")
                .Append('"')
                .Append(':')
                .Append('"')
                .Append(password)
                .Append('"')
                .Append('}')
                .ToString();
        }
    }
}

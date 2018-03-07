using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TodoApi.Models.DtoModels;

namespace Tests.Helpers.Json
{
    /// <summary>
    /// A static class containing various methods to convert a class as 
    /// json string to their corresponding C# object.
    /// </summary>
    public static class JsonStringSerializer
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        /// <summary>
        /// Convert a string json body to an ApplicationUserDto.
        /// </summary>
        /// <param name="body">The json object as string</param>
        /// <returns>An user extracted from the json object</returns>
        public static ApplicationUserDto GetApplicationUserDto(string body)
        {
            return (ApplicationUserDto)Serializer
                .Deserialize
                (
                    new JTokenReader(JObject.Parse(body)), 
                    typeof(ApplicationUserDto)
                );
        }

        /// <summary>
        /// Convert a string json body to a array of user dtos.
        /// </summary>
        /// <param name="body">The json object as string</param>
        /// <returns>An array of users extracted from json</returns>
        public static ApplicationUserDto[] GetListOfApplicationUserDto(string body)
        {
            return (ApplicationUserDto[])Serializer
                    .Deserialize
                    (
                        new JTokenReader(JArray.Parse(body)),
                        typeof(ApplicationUserDto[])
                    );
        }

        /// <summary>
        /// Convert a string json body to a todo.
        /// </summary>
        /// <param name="body">The json object as string</param>
        /// <returns>A todo extracted from the json object</returns>
        public static TodoDto GetTodoDto(string body)
        {
            return (TodoDto)Serializer
                .Deserialize
                (
                    new JTokenReader(JObject.Parse(body)),
                    typeof(TodoDto)
                );
        }

        /// <summary>
        /// Convert a string json body to a array of todo dtos.
        /// </summary>
        /// <param name="body">The json object as string</param>
        /// <returns>A todo of users extracted from json</returns>
        public static TodoDto[] GetListOfTodoto(string body)
        {
            return (TodoDto[])Serializer
                    .Deserialize
                    (
                        new JTokenReader(JArray.Parse(body)),
                        typeof(TodoDto[])
                    );
        }
    }
}

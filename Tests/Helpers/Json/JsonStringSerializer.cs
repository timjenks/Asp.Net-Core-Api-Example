using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    }
}

#region Imports

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

#endregion

namespace Tests.Helpers.EndSystems
{
    /// <summary>
    /// A mock server and client, with communication between the two.
    /// Allows us to make requests from client to server and get response.
    /// </summary>
    public class MockServerAndClient
    {

        #region Fields

        private const string BaseAddress = "http://localhost";
        private readonly TestServer _server;
        private readonly HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a server using the EndSystems setup file.
        /// </summary>
        /// <param name="acceptJsonOnly">Adds {Accept, application/json} 
        /// field to all request headers if true</param>
        public MockServerAndClient(bool acceptJsonOnly = true)
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<StartUp>()
                .UseApplicationInsights();

            _server = new TestServer(builder);
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri(BaseAddress);
            if (acceptJsonOnly)
            {
                _client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        }

        #endregion

        #region Token settings

        /// <summary>
        /// Add a bearer token to default request headers.
        /// </summary>
        /// <param name="token">A bearer token</param>
        public void SetBearerToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Clear the default request headers of Authentication field.
        /// </summary>
        public void RemoveBearerToken()
        {
            _client.DefaultRequestHeaders.Remove("Authentication");
        }

        #endregion

        #region Http methods

        /// <summary>
        /// Make a GET request from the client to the server.
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Get(string route)
        {
            return await Response(await _client.GetAsync(route));
        }

        /// <summary>
        /// Make a POST request from the client to the server. 
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <param name="content">The json object from the client</param>
        /// <param name="mediaType">Optional parameter for content type</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Post(string route, 
            HttpContent content, string mediaType = "application/json")
        {
            content.Headers.ContentType.MediaType = mediaType;
            return await Response(await _client.PostAsync(route, content));
        }

        /// <summary>
        /// Make a PUT request from the client to the server. 
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <param name="content">The json object from the client</param>
        /// <param name="mediaType">Optional parameter for content type</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Put(string route, 
            HttpContent content, string mediaType = "application/json")
        {
            content.Headers.ContentType.MediaType = mediaType;
            return await Response(await _client.PutAsync(route, content));
        }

        /// <summary>
        /// Make a DELETE request from the client to the server. 
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Delete(string route)
        {
            return await Response(await _client.DeleteAsync(route));
        }

        #endregion

        /// <summary>
        /// Cleans up resources of both the server and client.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        #region Helpers

        /// <summary>
        /// Converts a HttpResponseMessage to a mock response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<MockResponse> Response(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var headers = response.Headers;

            return new MockResponse
            {
                Code = response.StatusCode,
                Body = body,
                Headers = headers
            };
        }

        #endregion
    }
}

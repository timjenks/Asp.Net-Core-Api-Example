using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Tests.Helpers.EndSystems
{
    /// <summary>
    /// A mock server and client, with communication between the two.
    /// Allows us to make requests from client to server and get response.
    /// </summary>
    public class MockServerAndClient
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        /// <summary>
        /// Create a server using the EndSystems setup file.
        /// </summary>
        public MockServerAndClient()
        {
            var builder = new WebHostBuilder()
                //.UseContentRoot(@"...")
                .UseEnvironment("Development")
                .UseStartup<StartUp>()
                .UseApplicationInsights();


            _server = new TestServer(builder);
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost");
        }

        /// <summary>
        /// Make a GET request from the client to the server.
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Get(string route)
        {
            var x = await _client.GetAsync(route);
            return await Response(x);
        }

        /// <summary>
        /// Make a POST request from the client to the server. 
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <param name="content">The json object from the client</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Post(string route, HttpContent content)
        {
            content.Headers.ContentType.MediaType = "application/json";
            return await Response(await _client.PostAsync(route, content));
        }

        /// <summary>
        /// Make a PUT request from the client to the server. 
        /// </summary>
        /// <param name="route">The route for the request (without host)</param>
        /// <param name="content">The json object from the client</param>
        /// <returns>The response from the server (asynchronous)</returns>
        public async Task<MockResponse> Put(string route, HttpContent content)
        {
            content.Headers.ContentType.MediaType = "application/json";
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

        /// <summary>
        /// Cleans up resources of both the server and client.
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

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
    }
}

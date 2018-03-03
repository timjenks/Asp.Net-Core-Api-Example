using System.Net;
using System.Net.Http.Headers;

namespace Tests.Helpers.EndSystems
{
    /// <summary>
    /// Reponse used in integration tests.
    /// </summary>
    public class MockResponse
    {
        /// <summary>
        /// The status code of the response.
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// The headers of the response.
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }

        /// <summary>
        /// The body of the response.
        /// </summary>
        public string Body { get; set; }
    }
}

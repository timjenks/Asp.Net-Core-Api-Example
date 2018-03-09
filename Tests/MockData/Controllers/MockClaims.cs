#region Imports

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;

#endregion

namespace Tests.MockData.Controllers
{
    /// <summary>
    /// A static class holding methods to add claims to controllers.
    /// </summary>
    public static class MockClaims
    {
        /// <summary>
        /// Add a user id claim to a controller.
        /// </summary>
        /// <param name="controller">A controller to add claim to</param>
        /// <param name="userId">The user id to add</param>
        public static void AddUserIdClaim(Controller controller, string userId)
        {
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext
                .SetupGet(z => z.User.Claims)
                .Returns(new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                });
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
        }
    }
}

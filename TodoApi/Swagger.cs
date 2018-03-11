#region Imports

using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using TodoApi.Utils.Constants;

#endregion

namespace TodoApi
{
    /// <summary>
    /// Contains various helpers for Swagger.
    /// </summary>
    public static class Swagger
    {
        /// <summary>
        /// Set options for swagger generation.
        /// </summary>
        /// <param name="options">An options parameter to set</param>
        public static void SetGenOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc
            (
                Routes.Version,
                new Info
                {
                    Title = "Todo API",
                    Version = Routes.Version,
                    Description = "An example restful api project in dotnet core",
                    Contact = new Contact
                    {
                        Name = "Jón Steinn Elíasson",
                        Email = "jonsteinn@gmail.com",
                        Url = "https://github.com/jonsteinn"
                    },
                    License = new License
                    {
                        Name = "MIT",
                        Url = "https://opensource.org/licenses/MIT",
                    }
                }
            );
            var filePath = Path.Combine
            (
                PlatformServices.Default.Application.ApplicationBasePath,
                "TodoApiDocumentation.xml"
            );
            options.IncludeXmlComments(filePath);
        }


        /// <summary>
        /// Authorization filter for swagger to add authorization field to all requests.
        /// </summary>
        public class AuthorizationInputOperationFilter : IOperationFilter
        {
            /// <summary>
            /// Add Authorization parameter to swagger requests.
            /// </summary>
            /// <param name="operation">Swagger operation</param>
            /// <param name="context">Swagger context</param>
            public void Apply(Operation operation, OperationFilterContext context)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "Enter: Bearer {token}",
                    Required = false,
                    Type = "string"
                });
            }
        }
    }
}
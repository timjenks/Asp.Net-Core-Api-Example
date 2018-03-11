#region Imports

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using TodoApi.Data;

#endregion

namespace TodoApi
{
    /// <summary>
    /// Starting point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function for Api.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    Seed.CreateAll(scope.ServiceProvider.GetRequiredService<IServiceProvider>()).Wait();
                }
                catch (Exception)
                {
                }
            }
            host.Run();
        }

        /// <summary>
        /// Construct configuration from arguments.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Configurations as IWeb>Host</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

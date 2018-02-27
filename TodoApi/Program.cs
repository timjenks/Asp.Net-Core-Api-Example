﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
            BuildWebHost(args).Run();
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

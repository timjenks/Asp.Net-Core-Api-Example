﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoApi.Utils.Constants;
using TodoApi.Data;
using TodoApi.Models.EntityModels;
using TodoApi.Services;
using TodoApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi
{
    /// <summary>
    /// Start up settings for production Api.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor that injects configuration.
        /// </summary>
        /// <param name="configuration">Configuration key-value pairs</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// variable that stores configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Dependency injection services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext 
            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("TodoApi")));

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = PasswordLimits.PasswordSettings;
            }).AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();

            // JWT Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Issuer"],
                        ValidAudience = Configuration["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Inject services
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();

            services.AddCors();

            services.AddMvc();

            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc
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
                c.IncludeXmlComments(filePath);
                c.OperationFilter<AuthorizationInputOperationFilter>();
            });

        }
        
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Configuration of app pipeline</param>
        /// <param name="env">Hosting information</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Location")
            );

            app.UseAuthentication();

            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Routes.SwaggerRoute, "Todo Example API" );
            }); 
        }

        /// <summary>
        /// Authorization filter for swagger to add authorization field to all requests.
        /// </summary>
        private class AuthorizationInputOperationFilter : IOperationFilter
        {
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
                    Description = "Bearer token if required, e.g. 'Bearer {token}'",
                    Required = false,
                    Type = "string"
                });
            }
        }
    }
}
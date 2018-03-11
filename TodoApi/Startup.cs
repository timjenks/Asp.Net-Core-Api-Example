#region Imports

using System;
using System.IO;
using System.Text;
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

#endregion

namespace TodoApi
{
    /// <summary>
    /// Start up settings for production Api.
    /// </summary>
    public class Startup
    {
        #region Fields
        
        /// <summary>
        /// variable that stores configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that injects configuration.
        /// </summary>
        /// <param name="configuration">Configuration key-value pairs</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Configure Services

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

            #region JWT

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

            #endregion

            // Inject services
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();

            services.AddCors();

            services.AddMvc();

            services.AddMemoryCache();
            
            services.AddSwaggerGen(c => Swagger.SetGenOptions(c));
        }

        #endregion

        #region Configure

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

        #endregion
    }
}
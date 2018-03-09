#region Imports

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Tests.MockData.Data;
using Tests.MockData.EntityModels;
using TodoApi.Utils.Constants;
using TodoApi.Data;
using TodoApi.Models.EntityModels;
using TodoApi.Services;
using TodoApi.Services.Interfaces;

#endregion

namespace Tests.Helpers.EndSystems
{
    /// <summary>
    /// Mock startup for the mock client.
    /// </summary>
    public class StartUp
    {
        #region Fields

        /// <summary>
        /// Configuration interface object.
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Constructor for mock startup.
        /// </summary>
        /// <param name="configuration">The configuration injected</param>
        public StartUp(IConfiguration configuration)
        {
            if (configuration == null)
            {
                // Temp warning supression.
                // TODO: remove configuration parameter
            }
            Configuration = new MockConfiguration();
        }

        #endregion

        #region Configure Services

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Use in memory database for integration tests
            services.AddDbContext<AppDataContext>(
                optionsBuilder => optionsBuilder.UseInMemoryDatabase("InMemoryDb"));

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = PasswordLimits.PasswordSettings;
            }).AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();

            #region JWT
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
            #endregion

            // Inject services
            services.AddTransient<ITodoService, TodoService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IConfiguration, MockConfiguration>();

            services.AddCors();
            services.AddMvc();
            services.AddMemoryCache();
        }

        #endregion

        #region Configure

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var db = app.ApplicationServices.GetService<AppDataContext>();
                FillDatabase(db);
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
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Fills the database with mock data.
        /// </summary>
        /// <param name="db">The database to add to</param>
        private static void FillDatabase(AppDataContext db)
        {
            db.Roles.Add(MockRoles.Admin);
            db.Roles.Add(MockRoles.User);
            var allUsers = MockApplicationUsers.GetAll().ToArray();
            db.Users.AddRange(allUsers);
            db.SaveChanges();
            foreach (var user in allUsers)
            {
                db.UserRoles.Add(MockUserRoles.GetUserRoleForUser(user.Id));
            }

            db.Todo.AddRange(MockTodos.GetAll());
            db.SaveChanges();
        }

        #endregion
    }
}
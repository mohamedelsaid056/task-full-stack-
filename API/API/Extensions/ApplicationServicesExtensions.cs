using API.Errors;
using Core.Interfaces;
using Core.Models.Auth;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // Add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()

                .AddDefaultTokenProviders();

            // Add JWT Authentication
            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                // any identity more options 
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>();


            // identity options

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });




            services.AddAuthorization();

            // Add Scoped Services
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITokenService, TokenService>();


            // service for enhanced validation error messages
            services.Configure<ApiBehaviorOptions>(options =>
             {
                 options.InvalidModelStateResponseFactory = actionContext =>
                 {
                     var errors = actionContext.ModelState
                         .Where(e => e.Value.Errors.Count > 0)
                         .SelectMany(x => x.Value.Errors)
                         .Select(x => x.ErrorMessage).ToArray();

                     var errorResponse = new ApiValidationErrorResponse
                     {
                         Errors = errors
                     };

                     return new BadRequestObjectResult(errorResponse);
                 };
             });

            // swager for identity


            return services;
        }
    }
}

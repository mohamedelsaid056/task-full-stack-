using Core.Models.Auth;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using API.Errors;
using API.Extensions;
using Infrastructure.Identity;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //cores policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });


            // for services see this in extensions folder with class name ApplicationServicesExtensions
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSwaggerDocumentation();


            var app = builder.Build();

            // Configure the HTTP request pipeline.



            //handle Exceptions 
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Handling errors take care i have contorller for this 
            app.UseSwaggerDocumentation();


            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();





            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // First ensure database is created and migrations are applied
                    await context.Database.MigrateAsync();

                    // Then seed users
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);

                  

                    logger.LogInformation("Database seeded successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database");
                    throw;
                }
            }

            app.Run();
        }
    }
}

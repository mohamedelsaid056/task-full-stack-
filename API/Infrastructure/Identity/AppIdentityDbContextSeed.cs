
using Core.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed 
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
               

                // Create admin user
                var adminUser = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    UserName = "admin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                

                // Create regular user
                var regularUser = new ApplicationUser
                {
                    FirstName = "Regular",
                    LastName = "User",
                    Email = "user@example.com",
                    UserName = "user@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(regularUser, "User@123");
               
            }
        }
    }
}
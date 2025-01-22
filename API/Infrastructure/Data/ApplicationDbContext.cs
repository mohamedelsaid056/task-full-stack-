using Core.Models;
using Core.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);



        }


        // if i need to add seeding data for products in database auto 
        private void SeedData(ModelBuilder modelBuilder)
        {

            // Add the new products to the seeding data
            var products = CreateProducts();
            modelBuilder.Entity<Product>().HasData(products);

        }

        private List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Title = "Product 1",
                    Description = "Description for product 1"
                },
                new Product
                {
                    Id = 2,
                    Title = "Product 2",
                    Description = "Description for product 2"
                },
                new Product
                {
                    Id = 3,
                    Title = "Product 3",
                    Description = "Description for product 3"
                },
                new Product
                {
                    Id = 4,
                    Title = "Product 4",
                    Description = "Description for product 4"
                },
                new Product
                {
                    Id = 5,
                    Title = "Product 5",
                    Description = "Description for product 5"
                }

            };
        }

        public async Task SeedUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if there are any users
            if (!userManager.Users.Any())
            {
                // First create roles
                var roles = new[] { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

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
                await userManager.AddToRoleAsync(adminUser, "Admin");

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
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }


}

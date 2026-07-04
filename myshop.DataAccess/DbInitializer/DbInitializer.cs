using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using System;
using System.Linq;

namespace myshop.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // Apply migrations if there are pending migrations
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception) { }

            // Create roles if they do not exist
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();

                // Seed admin user
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@myshop.com",
                    Email = "admin@myshop.com",
                    Name = "Admin User",
                    FullName = "Admin User",
                    City = "Cairo",
                    Address = "123 Main St",
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                }
            }
        }
    }
}

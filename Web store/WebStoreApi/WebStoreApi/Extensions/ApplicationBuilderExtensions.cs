using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Api.Constants;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;

namespace WebStore.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
                var adminRole = roleManager.FindByNameAsync(RoleNames.AdminRoleName).Result;
                if (adminRole == null)
                {
                    roleManager.CreateAsync(new UserRole(RoleNames.AdminRoleName)).Wait();
                }
                var userRole = roleManager.FindByNameAsync(RoleNames.UserRoleName).Result;
                if (userRole == null)
                {
                    roleManager.CreateAsync(new UserRole(RoleNames.UserRoleName)).Wait();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var adminUser = userManager.FindByNameAsync("Administrator").Result;
                if (adminUser == null)
                {
                    var admin = new User()
                    {
                        UserName = "Administrator",
                        Email = "admin@gmail.com",
                        LockoutEnabled = false
                    };

                    userManager.CreateAsync(admin, "admin").Wait();
                    userManager.AddToRolesAsync(admin, new[] { RoleNames.UserRoleName, RoleNames.AdminRoleName }).Wait();
                }

                db.SaveChanges();
            }
        }
    }
}

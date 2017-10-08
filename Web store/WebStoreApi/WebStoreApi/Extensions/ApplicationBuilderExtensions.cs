using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
                var adminRole = roleManager.FindByNameAsync("Administrator").Result;
                if (adminRole == null)
                {
                    roleManager.CreateAsync(new UserRole("Administrator")).Wait();
                }
                var userRole = roleManager.FindByNameAsync("User").Result;
                if (userRole == null)
                {
                    roleManager.CreateAsync(new UserRole("User")).Wait();
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
                    userManager.AddToRolesAsync(admin, new[] { "User", "Administrator" }).Wait();
                }

                db.SaveChanges();
            }
        }
    }
}

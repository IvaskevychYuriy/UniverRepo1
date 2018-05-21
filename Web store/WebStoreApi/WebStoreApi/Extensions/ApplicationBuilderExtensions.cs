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
                foreach (var roleName in new[] { RoleNames.User, RoleNames.Admin, RoleNames.Owner })
                {
                    var role = roleManager.FindByNameAsync(roleName).Result;
                    if (role == null)
                    {
                        roleManager.CreateAsync(new UserRole(roleName)).Wait();
                    }
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
                    userManager.AddToRolesAsync(admin, new[] { RoleNames.User, RoleNames.Admin }).Wait();
                }
                
                var ownerUser = userManager.FindByNameAsync("Owner").Result;
                if (ownerUser == null)
                {
                    var owner = new User()
                    {
                        UserName = "Owner",
                        Email = "owner@gmail.com",
                        LockoutEnabled = false
                    };

                    userManager.CreateAsync(owner, "owner").Wait();
                    userManager.AddToRolesAsync(owner, new[] { RoleNames.User, RoleNames.Admin, RoleNames.Owner }).Wait();
                }

                db.SaveChanges();
            }
        }
    }
}

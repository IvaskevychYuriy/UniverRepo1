using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Contexts;
using WebStore.Models.Entities;
using WebStore.Api.Extensions;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Hangfire.AspNetCore;
using WebStore.Api.Helpers;
using WebStore.Api.Constants;
using WebStoreApi.Jobs;

namespace WebStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                //options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddCors();
            
            services.AddHangfire(cfg => 
                cfg.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDBConnection")));

            services.AddMvc();
            
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.SeedData();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseAuthentication();

            app.UseCors(cfg =>
            {
                cfg.AllowCredentials();
                cfg.AllowAnyMethod();
                cfg.AllowAnyHeader();
                cfg.WithOrigins("http://localhost:4200", "http://localhost:5000");
            });
            
            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                Authorization = new [] { new HandfireDashboardAuthFilter() }
            });
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                Activator = new AspNetCoreJobActivator(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>())
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });

            LaunchJobs();
        }

        private void LaunchJobs()
        {
            RecurringJob.AddOrUpdate<DronesUpdateSimulatorJob>(JobIds.DronesArrivalCheckerJob, job => job.UpdateDronesStates(), Cron.Minutely());
            RecurringJob.AddOrUpdate<OrdersStateUpdateJob>(JobIds.OrdersStateUpdateJob, job => job.UpdateOrderStates(), Cron.MinuteInterval(2));
            RecurringJob.AddOrUpdate<OrdersProcessingJob2>(JobIds.OrdersProcessingJob, job => job.ProcessOrders(), Cron.MinuteInterval(2));
        }
    }
}

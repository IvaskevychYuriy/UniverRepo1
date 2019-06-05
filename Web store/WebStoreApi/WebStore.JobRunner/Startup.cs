using Hangfire;
using Hangfire.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.Contexts;
using WebStore.JobRunner.Constants;
using WebStoreApi.Jobs;

namespace WebStore.JobRunner
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

			services.AddCors();

			services.AddHangfire(cfg =>
				cfg.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDBConnection")));

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			
			app.UseCors(cfg =>
			{
				cfg.AllowCredentials();
				cfg.AllowAnyMethod();
				cfg.AllowAnyHeader();
				cfg.WithOrigins("http://localhost:4200", "http://localhost:5000");
			});

			app.UseHangfireDashboard("/jobs", new DashboardOptions());
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

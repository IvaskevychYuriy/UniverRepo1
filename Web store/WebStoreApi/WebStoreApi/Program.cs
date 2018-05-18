using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            string url = (args == null || args.Length == 0) ? string.Empty : args[0];

            return WebHost.CreateDefaultBuilder()
                .UseUrls(url)
                .UseStartup<Startup>()
                .Build();
        }
    }
}

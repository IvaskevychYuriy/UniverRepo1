using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

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
            UselessTaskManager.ServerIdentifier = new string(url.Split(new[] { ':' }).LastOrDefault()?.TakeWhile(c => c != '/').ToArray());

            return WebHost.CreateDefaultBuilder()
                .UseUrls(url)
                .UseStartup<Startup>()
                .Build();
        }
    }
}

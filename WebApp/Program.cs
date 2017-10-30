using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Options;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        var tmpconfig = config.Build();

                        config.AddCBAOptionsConfig(options =>
                            options.UseSqlServer(tmpconfig.GetConnectionString("CBA_Database")));

                        // added a 2nd time to ensure environment variables can override settings in the CBA database
                        config.AddEnvironmentVariables();
                    })
                .UseStartup<Startup>()
                .Build();
    }
}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Entities;
using WebApp.Options;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            MigrateAndSeedCBADatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        var tmpconfig = config.Build();

                        config.AddCBAOptionsConfig(options =>
                            options.UseSqlServer(tmpconfig.GetConnectionString("CBA_Database")));

                        // added a 2nd time to ensure environment variables can override settings in the CBA database
                        config.AddEnvironmentVariables();
                    })
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors","true")
                .UseStartup<Startup>();

        private static void MigrateAndSeedCBADatabase(IWebHost host)
        {
            var env = (IHostingEnvironment)host.Services.GetService(typeof(IHostingEnvironment));
            if (!env.IsDevelopment()) return;
            
            var app = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = app.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CBAContext>();
                context.Database.Migrate();

                var seeder = scope.ServiceProvider.GetService<CBASeeder>();
                seeder.Seed();
            }
        }
    }

}

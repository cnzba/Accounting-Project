using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Options;
using CryptoService;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApp
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
            services.AddDbContext<CBAContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CBA_Database")));
            services.AddScoped<ICryptography, Cryptography>();
            services.AddTransient<CBASeeder>();
            services.AddScoped<IInvoiceService, InvoiceService>();

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Cookie Authentication 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login/";
                options.Cookie.Name = "InvoiceCbaNZ";
                // Controls how much time the authentication ticket stored in the cookie will remain valid from the point it is created.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Account WEB/API", Version = "v1" });
            });

            services.AddOptions();
            services.Configure<CBAOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // allow angular to pick up from index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();

            // Enable Authentication
            app.UseAuthentication();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CBA WEB/API");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });



        }
    }
}

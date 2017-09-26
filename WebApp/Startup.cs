using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

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
            services.AddDbContext<CBAWEBACCOUNTContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CBA_Database")));

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Account WEB/API", Version = "v1" });
            });
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

            app.UseStaticFiles();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CBA WEB/API");
            });



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404 &&
            //       !Path.HasExtension(context.Request.Path.Value) &&
            //       !context.Request.Path.Value.StartsWith("/api/"))
            //    {
            //        context.Request.Path = "/index.html";
            //        await next();
            //    }
            //});
            //app.UseMvcWithDefaultRoute();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
        }
    }
}

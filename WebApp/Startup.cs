using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Options;
using CryptoService;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceUtil.Email;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using AutoMapper;
using WebApp.Services;

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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Cookie Authentication 
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "InvoiceCbaNZ";
                // Controls how much time the authentication ticket stored in the cookie will remain valid from the point it is created.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(240);

                // ensures a 401 instead of 404 response if authorization fails
                // (404 comes because default is to redirect to asp.net core login page, which doesn't exist
                // as we are just a web api)
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Account WEB/API", Version = "v1" });
            });


            services.AddOptions();
            services.AddCNZBA(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // allow angular to pick up from index.html
            // app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCookiePolicy();

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
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }

    public static class CNZBA_Services
    {
        public static void AddCNZBA(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICryptography, Cryptography>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IEmail, Email>();
            services.AddTransient<CBASeeder>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IStripePaymentService, StripePaymentService>();

            services.Configure<CBAOptions>(configuration);
            services.Configure<EmailConfig>(configuration.GetSection("EmailConfig"));
            services.Configure<PdfServiceOptions>(configuration.GetSection("PdfService"));
        }
    }
}

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
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.IO;

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

            //The configuration of user identity
            //services.AddDefaultIdentity<CBAUser>( 
            //    options =>
            //{
            //    options.User = new UserOptions
            //    {
            //        RequireUniqueEmail = true
            //    };

            //    options.Password = new PasswordOptions
            //    {
            //        RequiredLength = 8,
            //        RequireDigit = true,
            //        RequireUppercase = true
            //    };

            //}
            //)
            

            //Set the requirement of the password, email and email confirmation.
            services.AddDefaultIdentity<CBAUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                
            }).AddDefaultUI()
            .AddEntityFrameworkStores<CBAContext>(); 

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //To upload large files, set the length of upload file to max value.
            services.Configure<FormOptions>(form =>
            {
                form.ValueLengthLimit = int.MaxValue;
                form.MultipartBodyLengthLimit = int.MaxValue;
                form.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //JWT Authentication

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience =false,
                    ClockSkew = TimeSpan.Zero
                };
            });


            

            #region Cookie authentication, deprived.
            // Cookie Authentication 
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.Cookie.Name = "InvoiceCbaNZ";
            //    // Controls how much time the authentication ticket stored in the cookie will remain valid from the point it is created.
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(240);

            //    // ensures a 401 instead of 404 response if authorization fails
            //    // (404 comes because default is to redirect to asp.net core login page, which doesn't exist
            //    // as we are just a web api)
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = 401;
            //        return Task.CompletedTask;
            //    };
            //}); 
            #endregion

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
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), 
                    @"Resources")),
                RequestPath= new PathString("/Resources")
            });

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
            //add pdf converter
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }
    }
}

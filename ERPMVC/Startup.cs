using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ERP.Contexts;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace ERPMVC
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            /*  services.AddAuthentication()
              .AddCookie(options =>
              {
                  options.LoginPath = "/Account/Login/";
                  options.AccessDeniedPath = "/Account/Forbidden/";
              })
              .AddJwtBearer(options =>
              {
                  options.Audience = "https://localhost:44347/";
                  options.Authority = "https://localhost:44347/";
              });
              */




            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //{
            //    options.Audience = "http://localhost:5001/";
            //    options.Authority = "https://localhost:44347/";
            //});

            services.AddIdentity<ApplicationUser, IdentityRole>(
                  options =>
                  {
                      options.Lockout.MaxFailedAccessAttempts = 6;

                      options.Password.RequiredLength = 6;
                      options.Password.RequiredUniqueChars = 3;
                      options.Password.RequireLowercase = false;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireUppercase = true;

                      options.SignIn.RequireConfirmedEmail = false;
                      options.User.RequireUniqueEmail = true;
                  })
                  .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            //services.Configure<RequestLocalizationOptions>(
            //options =>
            //    {
            //        var supportedCultures = new List<CultureInfo>
            //        {
            //                new CultureInfo("en-US"),
            //                new CultureInfo("es-hn"),
            //                new CultureInfo("de-CH"),
            //                new CultureInfo("fr-CH"),
            //                new CultureInfo("it-CH")
            //        };

            //        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            //        options.SupportedCultures = supportedCultures;
            //        options.SupportedUICultures = supportedCultures;
            //    });

            services.Configure<MyConfig>(Configuration.GetSection("AppSettings"));
         
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                  .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

               services.AddKendo();


            services.AddAuthorization(options =>
        {

          options.AddPolicy("Admin", policy =>
          {

                    //policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    //   policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new AdminRequirement());

          });

                //options.AddPolicy("RecursosHumanos", policy => policy.Requirements.Add(new CategoriaEmpleadoRequirement()));
                //options.AddPolicy("RequireRolesLogin", policy
                //    => policy.RequireRole(rolestologin));
            });


            services.AddSingleton<IAuthorizationHandler, AdminHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

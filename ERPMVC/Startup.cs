using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
using Microsoft.IdentityModel.Tokens;
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
              options.CheckConsentNeeded = context => true;
              options.MinimumSameSitePolicy = SameSiteMode.None;
           });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddAutoMapper(options =>
        {
              //options.CreateMap<AutorCreacionDTO, Autor>();
          });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
            // Set a short timeout for easy testing.
            options.IdleTimeout = TimeSpan.FromHours(10);
              //  options.Cookie.HttpOnly = true;
            // Make the session cookie essential
            options.Cookie.IsEssential = true;
            });


            services.AddIdentity<ApplicationUser, ApplicationRole>(
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



           // services.AddLogging();
         
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                  .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

               services.AddKendo();


            //List<Product> _listproduct = new List<Product>();
            services.AddAuthorization(options =>
              {
                  //foreach (var item in _listproduct)
                  //{
                  //    options.AddPolicy(item.ProductName, policy =>
                  //   {

                  //   });
                  //}


                  //options.AddPolicy("Admin", policy =>
                  //{
                  //   //policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                  //   //policy.RequireAuthenticatedUser();
                  //   policy.Requirements.Add(new AdminRequirement());
                  //});


                  //options.AddPolicy("Usuario", policy =>
                  //{
                  //    policy.Requirements.Add(new UsuarioRequirement());
                  //});


              });




            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, HasScopeHandler>();
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
            app.UseSession();
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

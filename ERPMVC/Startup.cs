using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Contexts;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.Policies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Syncfusion.Licensing;
using static ERPMVC.Helpers.ViewRenderService;

namespace ERPMVC
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration, IServiceProvider serviceProvider)
        {

            string License = File.ReadAllText(System.IO.Path.Combine(env.ContentRootPath, "SyncfusionLicense.txt"), Encoding.UTF8);
            SyncfusionLicenseProvider.RegisterLicense(License);

            //var mvcBuilder = serviceProvider.GetService<IMvcBuilder>();
            //new MvcConfiguration().ConfigureMvc(mvcBuilder);

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.IdleTimeout = TimeSpan.FromSeconds(20);
                // options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = new PathString("/Account/login/");
                options.AccessDeniedPath = new PathString("/Account/Forbidden/");
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<MyConfig>(Configuration.GetSection("AppSettings"));

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential("erp@almacafehn.com", "Almacafe09");
            client.Host = "mail.almacafehn.com";
            client.Port = 587;

            services
               .AddFluentEmail("erp@almacafehn.com")
               .AddRazorRenderer()
               .AddSmtpSender(client);

            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<ViewRender, ViewRender>();

            var value = (GetQuantityFailedRequest().Result as Int32?);
            int maxfailed = value == null ? 3 : value.Value;


            var valuecaracteresminimos = (GetCaracteresMinimos().Result as Int32?);
            int caracteresminimos = valuecaracteresminimos == null ? 8 : valuecaracteresminimos.Value;


            services.AddIdentity<ApplicationUser, ApplicationRole>(
              options =>
              {
                  options.Lockout.MaxFailedAccessAttempts = maxfailed;

                  options.Password.RequiredLength = caracteresminimos;
                  options.Password.RequiredUniqueChars = 3;
                  options.Password.RequireLowercase = false;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = true;


                  options.SignIn.RequireConfirmedEmail = false;
                  options.User.RequireUniqueEmail = true;

              })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            //services.AddScoped<Filters.SessionsAuthorizationFilter>();

            //services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            //{
            //    builder.WithOrigins("http://localhost:9200").AllowAnyMethod().AllowAnyHeader();
            //}));

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {

                builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            //  .WithMethods("GET")
                            //  .WithOrigins("http://localhost:9200");
                            .AllowCredentials();

            }));


            services.AddMvc(config =>
            {
                //var policy = new AuthorizationPolicyBuilder()
                //.RequireAuthenticatedUser()
                //.Build();
                //config.Filters.Add(new AuthorizeFilter(policy));
            })
            //.ConfigureApplicationPartManager(manager =>
            //{
            //    var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
            //    manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
            //    manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
            //})
            .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();


            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });


            services.AddKendo();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, HasScopeHandler>();
        }

        private async Task<int> GetQuantityFailedRequest()
        {
            int maxaccess = 0;
            try
            {               
                var config = Configuration.GetSection("AppSettings").Get<MyConfig>();
                string baseadress = config.urlbase;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.UserEmail, Password = config.UserPassword });
                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + 15);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        string res = await result.Content.ReadAsStringAsync();
                        ElementoConfiguracion _elc = JsonConvert.DeserializeObject<ElementoConfiguracion>(res);
                        if (_elc == null) { _elc = new ElementoConfiguracion { Valordecimal = 0 }; }
                        maxaccess = Convert.ToInt32(_elc.Valordecimal);
                    }
                }
            }
            catch (Exception ex)
            {
                maxaccess = 0;
            }
           

            return maxaccess;

        }


        private async Task<int> GetCaracteresMinimos()
        {
            int maxaccess = 0;
            try
            {
                var config = Configuration.GetSection("AppSettings").Get<MyConfig>();
                string baseadress = config.urlbase;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = config.UserEmail, Password = config.UserPassword });
                if (resultlogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                    var result = await _client.GetAsync(baseadress + "api/ElementoConfiguracion/GetElementoConfiguracionById/" + 21);
                    //string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        string res = await result.Content.ReadAsStringAsync();
                        ElementoConfiguracion _elc = JsonConvert.DeserializeObject<ElementoConfiguracion>(res);
                        if (_elc == null) { _elc = new ElementoConfiguracion { Valordecimal = 0 }; }
                        maxaccess = Convert.ToInt32(_elc.Valordecimal);
                    }
                }
            }
            catch (Exception ex)
            {
                maxaccess = 0;
            }


            return maxaccess;

        }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //   services.Configure<CookiePolicyOptions>(options =>
        //   {           
        //      options.CheckConsentNeeded = context => true;
        //      options.MinimumSameSitePolicy = SameSiteMode.None;
        //   });

        //    services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


        //    services.AddAutoMapper(options =>
        //   {
        //      //options.CreateMap<AutorCreacionDTO, Autor>();
        //  });

        //    services.AddDistributedMemoryCache();
        //    services.AddSession(options =>
        //    {
        //    // Set a short timeout for easy testing.
        //    options.IdleTimeout = TimeSpan.FromHours(10);
        //      //  options.Cookie.HttpOnly = true;
        //    // Make the session cookie essential
        //    options.Cookie.IsEssential = true;
        //    });


        //    services.AddIdentity<ApplicationUser, ApplicationRole>(
        //          options =>
        //          {
        //              options.Lockout.MaxFailedAccessAttempts = 6;

        //              options.Password.RequiredLength = 6;
        //              options.Password.RequiredUniqueChars = 3;
        //              options.Password.RequireLowercase = false;
        //              options.Password.RequireNonAlphanumeric = false;
        //              options.Password.RequireUppercase = true;

        //              options.SignIn.RequireConfirmedEmail = false;
        //              options.User.RequireUniqueEmail = true;

        //          })
        //          .AddEntityFrameworkStores<ApplicationDbContext>()
        //          .AddDefaultTokenProviders();

        //    services.Configure<RequestLocalizationOptions>(
        //    options =>
        //        {
        //            var supportedCultures = new List<CultureInfo>
        //            {
        //                    new CultureInfo("es-HN"),
        //                    new CultureInfo("en-US"),
        //                   // new CultureInfo("es-hn"),
        //                    new CultureInfo("de-CH"),
        //                    new CultureInfo("fr-CH"),
        //                    new CultureInfo("it-CH")
        //            };

        //            options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        //            options.SupportedCultures = supportedCultures;
        //            options.SupportedUICultures = supportedCultures;
        //        });

        //    services.Configure<MyConfig>(Configuration.GetSection("AppSettings"));

        //    services.AddAuthorization(options =>
        //        {

        //        });

        //        services.AddLogging();


        //    //services.AddAuthentication(sharedOptions =>
        //    //{
        //    //    sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    //    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //    //    // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        //    //})
        //    //.AddCookie(
        //    //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    //    options =>
        //    //    {
        //    //        options.LoginPath = "/Account/login"; ;
        //    //        options.AccessDeniedPath = new PathString("/account/login");
        //    //        options.Cookie.Name = "AUTHCOOKIE";
        //    //        options.ExpireTimeSpan = new TimeSpan(365, 0, 0, 0);
        //    //        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //    //        options.Cookie.SameSite = SameSiteMode.None;

        //    //    }
        //    //);

        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });

        //    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        //    options =>
        //    {
        //        options.LoginPath = new PathString("/Account/Login/");
        //        options.AccessDeniedPath = new PathString("/Account/Forbidden/");
        //    });


        //    services.AddMvc(options =>
        //    {
        //        //   options.ModelBinderProviders.Insert(0, new MyViewModelBinderProvider());
        //    }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        //    //.AddJsonOptions(options => {
        //    //    // send back a ISO date
        //    //    var settings = options.SerializerSettings;
        //    //    settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
        //    //    // dont mess with case of properties
        //    //    var resolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
        //    //    resolver.NamingStrategy = null;
        //    //});

        //    .AddJsonOptions(options =>
        //     {
        //         // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //          options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        //          options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        //           options.SerializerSettings.DateFormatString = "dd/MM/yyyy hh:MM:ss";
        //           options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        //           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        //     });

        //    services.AddKendo();


        //    //List<Product> _listproduct = new List<Product>();
        //    //services.AddAuthorization(options =>
        //    //  {
        //    //      //foreach (var item in _listproduct)
        //    //      //{
        //    //      //    options.AddPolicy(item.ProductName, policy =>
        //    //      //   {

        //    //      //   });
        //    //      //}


        //    //      //options.AddPolicy("Admin", policy =>
        //    //      //{
        //    //      //   //policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        //    //      //   //policy.RequireAuthenticatedUser();
        //    //      //   policy.Requirements.Add(new AdminRequirement());
        //    //      //});


        //    //      //options.AddPolicy("Usuario", policy =>
        //    //      //{
        //    //      //    policy.Requirements.Add(new UsuarioRequirement());
        //    //      //});


        //    //  });




        //    services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        //    services.AddScoped<IAuthorizationHandler, HasScopeHandler>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          

            app.UseAuthentication();
          
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


            //app.UseCors(builder =>
            //     builder.WithOrigins("http://localhost:9200"));

            app.UseCors("AllowAllOrigins");

            //app.UseCors(builder => builder.WithOrigins("http://localhost:9200")
            //                  .AllowAnyOrigin()
            //                  .AllowAnyMethod()
            //                  .AllowAnyHeader());

            //   app.UseHttpsRedirection();
            app.UseStaticFiles();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.SameAsRequest,
                MinimumSameSitePolicy = SameSiteMode.None
            };

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseSession();
           

            app.UseMvc(routes =>
            {
             
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

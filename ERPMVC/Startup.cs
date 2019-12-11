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
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });

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

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {

                builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowCredentials();

            }));


            services.AddMvc(config =>
            {
            })
            .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();


            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });

            string baseadress = Configuration.GetSection("AppSettings").GetSection("urlbase").Value;
            HttpClient _client = new HttpClient();
            var result = _client.GetAsync(baseadress + "api/Permisos/ListarPermisos");
            if (result.Result.IsSuccessStatusCode)
            {
                var respuesta = (result.Result.Content.ReadAsStringAsync());
                List<string> permisos = JsonConvert.DeserializeObject<List<string>>(respuesta.Result);
                services.AddAuthorization(options =>
                {
                    string permisosActualizados = "";
                    foreach (var permiso in permisos)
                    {
                        permisosActualizados += permiso + "\r\n";
                        options.AddPolicy(permiso, policy => policy.RequireClaim(permiso, "true"));
                    }
                    //Actualiza el archivo local de permisos
                    File.WriteAllText("PermisosSistema.txt",permisosActualizados);

                });
            }
            else
            {//No se logro comunicar con el servidor de backend para cargar los permisos actualizados
                var permisosText = File.ReadAllText("PermisosSistema.txt");
                permisosText = permisosText.Replace("\r", "");
                var permisos = permisosText.Split("\n");
                services.AddAuthorization(options =>
                                          {
                                              foreach (var permiso in permisos)
                                              {
                                                  options.AddPolicy(permiso, policy => policy.RequireClaim(permiso, "true"));
                                              }
                                          });
            }
            

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

            app.UseCors("AllowAllOrigins");
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

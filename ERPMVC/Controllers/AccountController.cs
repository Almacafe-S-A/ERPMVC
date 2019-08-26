using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOptions<MyConfig> config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
            ILogger<HomeController> logger,
            IOptions<MyConfig> config
               )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this.config = config;
        }


        public IActionResult login()
        {
            return View();
        }

     
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            List<MessageClassUtil> _message = new List<MessageClassUtil>();
            try
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                var res = HttpContext.User.Identity.IsAuthenticated;
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = model.Email, Password = model.Password });
                if (result.Succeeded)
                {
                 
                 

                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);


                    if (_userToken.LastPasswordChangedDate != null)
                    {
                        if (_userToken != null
                      && _userToken.LastPasswordChangedDate.Date.AddDays(_userToken.Passworddias) < DateTime.Now.Date
                            && !Request.Path.ToString().EndsWith("/Account/ChangePassword.aspx"))
                        {
                            HttpContext.Session.SetString("token", _userToken.Token);
                            HttpContext.Session.SetString("user", model.Email);
                            HttpContext.Session.SetString("Expiration", _userToken.Expiration.ToString());
                            return RedirectToAction("ChangePassword", "Account");
                        }
                    }



                    if (_userToken.IsEnabled.Value)
                    {
                        HttpContext.Session.SetString("token", _userToken.Token);
                        HttpContext.Session.SetString("Expiration", _userToken.Expiration.ToString());
                        HttpContext.Session.SetString("user", model.Email);
                        HttpContext.Session.SetString("BranchId", _userToken.BranchId.ToString());

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _message.Add(new MessageClassUtil { key = "Login", name = "error", mensaje = "Error en login" });
                        model.Failed = true;
                        return View(model);
                    }

                }
                else
                {
                    _message.Add(new MessageClassUtil { key = "Login", name = "error", mensaje = "Error en login" });
                    model.Failed = true;
                    return View(model);
                }

                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                model.Failed = true;
                return View(model);
                // throw ex;

            }


        }

        public async  Task<ActionResult> ChangePassword()
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return await Task.Run(()=> View());
        }





        [HttpGet]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            //  await _signInManager.SignOutAsync();                 
            //  _logger.LogInformation($"User signed out");
            return await Task.Run(() => RedirectToAction(nameof(HomeController.Index), "Home"));
        }



    }
}
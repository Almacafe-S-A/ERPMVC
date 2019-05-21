using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
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
                //if (ModelState.IsValid)
                //{
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                //ApplicationUser _appuser = new ApplicationUser { Email = model.Email  };
                //var result = await _signInManager.CheckPasswordSignInAsync(_appuser, model.Password, lockoutOnFailure: false);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                var resultlogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = model.Email, Password = model.Password });
                if (result.Succeeded)
                {
                    string webtoken = await (resultlogin.Content.ReadAsStringAsync());
                    UserToken _userToken = JsonConvert.DeserializeObject<UserToken>(webtoken);
                    HttpContext.Session.SetString("token", _userToken.Token);
                    HttpContext.Session.SetString("Expiration", _userToken.Expiration.ToString());
                    HttpContext.Session.SetString("user", model.Email);
                   

                    return RedirectToAction("Index", "Home");
                    //return Task.Factory.StartNew(()=>_login()).ContinueWith<ActionResult>(
                    //  t =>
                    //  {
                    //      return RedirectToAction("Index", "Home");
                    //  });
                }
                else
                {
                    _message.Add(new MessageClassUtil { key = "Login", name = "error", mensaje = "Error en login" });
                    return View(model);
                }

                // }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



          

        }



        [HttpGet]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
              HttpContext.Session.Clear();    
          //  await _signInManager.SignOutAsync();                 
          //  _logger.LogInformation($"User signed out");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERPMVC.Controllers
{
    public class AccountController : Controller
    {


        public IActionResult login()
        {
            return View();
        }


        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
            ILogger<HomeController> logger
               )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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
                if (result.Succeeded)
                {
                    //_message.Add(new MessageClassUtil { key = "Login", name = "success", mensaje = Url.Action("Index", "Home") });
                    //return Json(_message);
                    // return RedirectToLocal(returnUrl);

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

                throw ex;
            }



          

        }

        private void _login()
        {

        }



    }
}
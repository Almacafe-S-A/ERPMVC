using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOptions<MyConfig> config;
        private readonly IConfiguration configuration;

        public AccountController(
            ILogger<HomeController> logger,
            IOptions<MyConfig> config,
            IConfiguration  iConfig
               )
        {
            this._logger = logger;
            this.config = config;
            this.configuration = iConfig;
        }


        public IActionResult login()
        {
            return View();
        }

        public async Task<ActionResult> ChangePassword()
        {
            return await Task.Run(() => View());
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            List<MessageClassUtil> _message = new List<MessageClassUtil>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                var resultLogin = await _client.PostAsJsonAsync(baseadress + "api/cuenta/login", new UserInfo { Email = model.Email, Password = model.Password });
                
                if (resultLogin.IsSuccessStatusCode)
                {
                    string webtoken = await (resultLogin.Content.ReadAsStringAsync());
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

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        JwtSecurityToken secToken = handler.ReadJwtToken(_userToken.Token);
                        
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaims(secToken.Claims);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        HttpClient cliente = new HttpClient();
                        cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + _userToken.Token);
                        var resultado = await cliente.GetAsync(baseadress + "api/Reportes/CadenaConexionBD");
                        if (resultado.IsSuccessStatusCode)
                        {
                            var cadena = await resultado.Content.ReadAsStringAsync();
                            Utils.ConexionReportes = cadena;
                        }

                        
                        
                        var resultadoCierre = await cliente.GetAsync(baseadress + "api/CierreContable/UltimoCierre");                        
                        string ultimoCierre = await resultadoCierre.Content.ReadAsStringAsync();
                        BitacoraCierreContable cierre = JsonConvert.DeserializeObject<BitacoraCierreContable>(ultimoCierre);
                        DateTime fechaactual = DateTime.Now;
                        fechaactual = fechaactual.AddDays(-1);
                        Utils.Cerrado = cierre.FechaCierre.Date >= fechaactual;




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
        

        [HttpGet]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return await Task.Run(() => RedirectToAction(nameof(HomeController.Index), "Home"));
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Permisos()
        {
            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            return Json(claims);
        }


    }
}
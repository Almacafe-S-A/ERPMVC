using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERPMVC.Models;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    // [Authorize(Policy ="Admin")]
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [EnableCors("AllowAllOrigins")]
    public class HomeController : Controller
    {
        // [Authorize(Policy ="Admin")]

        private readonly IOptions<MyConfig> _config;
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this._config = config;
            this._logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            ViewData["AccountManagements"] = await AccountManagementCampos();

            return View();
        }


        [HttpPost]
        public ActionResult Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }


        async Task<IEnumerable<AccountManagement>> AccountManagementCampos()
        {
            IEnumerable<AccountManagement> AccountManagementss = null;

            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetAccountManagement");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    AccountManagementss = JsonConvert.DeserializeObject<IEnumerable<AccountManagement>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            ViewData["AccountManagements"] = AccountManagementss.FirstOrDefault();
            return AccountManagementss;

        }


    }
}

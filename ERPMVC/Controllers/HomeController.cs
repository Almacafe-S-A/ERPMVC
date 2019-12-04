using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ERPMVC.Controllers
{
    // [Authorize(Policy ="Admin")]
    [Authorize]
    //[CustomAuthorization]
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

        public IActionResult Index()
        {
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
    }
}

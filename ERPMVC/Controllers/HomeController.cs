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
using ERPMVC.DTO;
using NLog;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Text;

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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this._config = config;
            this._logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            ViewData["AccountManagements"] = await AccountManagementCampos();
            HomeViewModel model = new HomeViewModel();
            // model.Notificaciones = await GetNotifications();
            //return View(model);
            ViewData["Notifications"] = await GetNotifications();
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

		[HttpGet("[controller]/[action]")]
		public async Task<List<Notifications>> GetNotifications()
		{
			List<Notifications> notificationsList = new List<Notifications>();
			try
			{
				string baseAddress = _config.Value.urlbase;
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

				var result = await client.GetAsync($"{baseAddress}api/Notifications/GetNotifications");
				string responseValue = "";
				if (result.IsSuccessStatusCode)
				{
					responseValue = await result.Content.ReadAsStringAsync();
					notificationsList = JsonConvert.DeserializeObject<List<Notifications>>(responseValue);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Ocurrió un error: {ex.ToString()}");
				throw ex;
			}

			return notificationsList;
		}

        [HttpPost("[action]")]
        public async Task<ActionResult> MarkNotificationAsRead(int Id)
        {
            try
            {
                string baseAddress = _config.Value.urlbase;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await client.PostAsync($"{baseAddress}api/Notifications/MarkNotificationAsRead?Id={Id}", null);

                if (result.IsSuccessStatusCode)
                {
                    var responseValue = await result.Content.ReadAsStringAsync();
                    var existingNotification = JsonConvert.DeserializeObject<Notifications>(responseValue);

                    // Actualizar la notificación como leída en el proyecto MVC
                    existingNotification.Leido = true;
                    existingNotification.FechaLectura = DateTime.Now;

                    return Ok(existingNotification);
                }
                else
                {
                    return BadRequest("No se pudo marcar la notificación como leída en la API");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar la notificación como leída");
                return BadRequest(ex);
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.Contexts;
using ERPMVC.Models;

using System.Net.Http;
using ERPMVC.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HoursWorkedController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public HoursWorkedController(ILogger<HoursWorkedController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetHoursWorked([DataSourceRequest]DataSourceRequest request)
        {
            List<HoursWorked> _hours = new List<HoursWorked>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/HoursWorked/GetHoursWorked");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _hours = JsonConvert.DeserializeObject<List<HoursWorked>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_hours.ToDataSourceResult(request));
        }

        
    }
}

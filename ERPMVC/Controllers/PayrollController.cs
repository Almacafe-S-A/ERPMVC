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
    public class PayrollController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PayrollController(ILogger<PayrollController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Payroll
        public IActionResult Index()
        {
            return View();
        }

        //Listado Empleados
        public ActionResult PayrollEmployee()
        {
            return PartialView();
        }

        public async Task<ActionResult> Detalles(Int64 Id)
        {
            Payroll _payroll = new Payroll();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Payroll/GetPayroll/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _payroll = JsonConvert.DeserializeObject<Payroll>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return View(_payroll);

        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Payroll> _hours = new List<Payroll>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Payroll/GetPayroll");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _hours = JsonConvert.DeserializeObject<List<Payroll>>(valorrespuesta);

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
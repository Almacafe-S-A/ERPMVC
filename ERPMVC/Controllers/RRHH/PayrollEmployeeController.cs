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
    public class PayrollEmployeeController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PayrollEmployeeController(ILogger<PayrollEmployeeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [HttpGet("[action]")]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PayrollEmployee> _hours = new List<PayrollEmployee>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PayrollEmployee/GetPayrollEmployee");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _hours = JsonConvert.DeserializeObject<List<PayrollEmployee>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_hours.ToDataSourceResult(request));
        }

        /// <summary>
        /// Listado de empleados por planilla.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IdPlanilla"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetByPlanillaId([DataSourceRequest]DataSourceRequest request, Int64 IdPlanilla)
        {
            List<PayrollEmployee> _PayrollEmployee = new List<PayrollEmployee>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PayrollEmployee/GetPayrollEmployee/" + IdPlanilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PayrollEmployee = JsonConvert.DeserializeObject<List<PayrollEmployee>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _PayrollEmployee.ToDataSourceResult(request);
        }

    }
}
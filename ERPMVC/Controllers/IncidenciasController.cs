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
    public class IncidenciasController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public IncidenciasController(ILogger<IncidenciasController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Incidencias
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Incidencias> _incidencias = new List<Incidencias>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Incidencias/GetIncidencias");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _incidencias = JsonConvert.DeserializeObject<List<Incidencias>>(valorrespuesta);

                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return Json(_incidencias.ToDataSourceResult(request));
        }

        /// <summary>
        /// Listado de vacaciones por empleado.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IdPlanilla"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetVacacionesByEmp([DataSourceRequest]DataSourceRequest request, Int64 IdEmpleado)
        {
            List<Incidencias> _vacaciones = new List<Incidencias>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Incidencias/GetVacaciones/" + IdEmpleado);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _vacaciones = JsonConvert.DeserializeObject<List<Incidencias>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }
            
            return _vacaciones.ToDataSourceResult(request);
        }

        /// <summary>
        /// Listado de incapacidades por empleado.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="IdPlanilla"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetIncapacidadesByEmp([DataSourceRequest]DataSourceRequest request, Int64 IdEmp)
        {
            List<Incidencias> _vacaciones = new List<Incidencias>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Incidencias/GetIncapacidades/" + IdEmp);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _vacaciones = JsonConvert.DeserializeObject<List<Incidencias>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }

            return _vacaciones.ToDataSourceResult(request);
        }

    }
}
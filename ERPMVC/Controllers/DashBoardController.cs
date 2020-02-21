using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DashBoardController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DashBoardController(ILogger<ConditionsController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpPost("[controller]/[action]")]
        public async  Task<ActionResult> FacturacionMes(Fechas _Fecha)
        {
            List<FacturacionMensual> _Facturacionmensual = new List<FacturacionMensual>();
            try
            {
                string baseadress = config.Value.urlbase;
             
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/DashBoard/FacturacionMes", _Fecha);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Facturacionmensual = JsonConvert.DeserializeObject<List<FacturacionMensual>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
         

          

            return Json(_Facturacionmensual);
            
        }

        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> FacturacionProductoByYear([DataSourceRequest]DataSourceRequest request, Fechas _Fecha)
        {
            List<FacturacionMensual> _Facturacionmensual = new List<FacturacionMensual>();
            try
            {

               // Fechas _Fecha = new Fechas { FechaInicio = DateTime.Now.AddMonths(-8), FechaFin = DateTime.Now };
                string baseadress = config.Value.urlbase;

                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/DashBoard/FacturacionByProductByYear", _Fecha);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Facturacionmensual = JsonConvert.DeserializeObject<List<FacturacionMensual>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_Facturacionmensual);
           // return Json(_Facturacionmensual.ToDataSourceResult(request));

        }


        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantitySalesOrders()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantitySalesOrders");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityJournalEntriesAprovement()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityJournalEntriesAprovement");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityInvoices()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityInvoices");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }

        
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityCertificadoDeposito()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _users = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_users);
        }




    }
}
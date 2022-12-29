using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Models;
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
        public async Task<JsonResult> GetEndososporVencer()
        {
            List<EndososCertificados> endosos = new List<EndososCertificados>() ;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificados/GetEndososporVencer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    endosos = JsonConvert.DeserializeObject<List<EndososCertificados>>(valorrespuesta);

                }

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw (new Exception(ex.Message));
            }
            return Json(endosos.Count);
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
        public async Task<JsonResult> GetQuantityCheckAccountLines()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityCheckAccountLines");
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
        public async Task<JsonResult> GetQuantityJournalEntriestotaldehoy()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityJournalEntriestotaldehoy");
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
        public async Task<JsonResult> GetQuantityFacturasproveedor()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityFacturasproveedor");
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
        public async Task<JsonResult> GetQuantityJournalPendientes()
        {
            Int32 _users = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityJournalPendientes");
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



        //=========================DAHSBOARD DE RIESGOS=================================================

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityAlerts()
        {
            Int32 _Alertas = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityAlerts");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alertas = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Alertas);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityInformacionMediatica()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityInformacionMediatica");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityProductoProhibidos()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityProductoProhibidos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityPEPS()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityPEPS");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        //=====================FIN DAHSBOARD DE RIESGOS=================================================


        //=========================DAHSBOARD PRECIDENCIAS=================================================
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetCertificadosbyFecha()
        {
            decimal _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetCertificadosbyFecha");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<decimal>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetCertificadosFechCatidad()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetCertificadosFechCatidad");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCuantasBancosSaldos([DataSourceRequest]DataSourceRequest request)
        {
            List<Accounting> _Accounting = new List<Accounting>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DashBoard/GetSaldoCuentasBanco");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Accounting = JsonConvert.DeserializeObject<List<Accounting>>(valorrespuesta);
                    _Accounting = _Accounting.OrderByDescending(e => e.AccountId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Accounting.ToDataSourceResult(request);

        }


        //=========================FIN DAHSBOARD PRECIDENCIAS=============================================




        //=========================DAHSBOARD RRHH=================================================
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityEmployees()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityEmployees");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }



        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityDepartamentos()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityDepartamentos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetSalarioTotal()
        {
            decimal _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetSalarioTotal");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode  )
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    if (valorrespuesta == "")
                    {
                        return Json(_Cantidad);
                    }

                    _Cantidad = JsonConvert.DeserializeObject<decimal>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        //===============================FIN RRHH=================================================


        //===================DAHSBOARD OPERACIONES=================================================
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityIngresos()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityIngresos");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantitySalidas()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantitySalidas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetQuantityAreasOcupadas()
        {
            Int32 _Cantidad = 0;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dashboard/GetQuantityAreasOcupadas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cantidad = JsonConvert.DeserializeObject<Int32>(valorrespuesta);

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (new Exception(ex.Message));
            }
            return Json(_Cantidad);
        }

        //============================FIN DAHSBOARD OPERACIONES====================================



    }
}
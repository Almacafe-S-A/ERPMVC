using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
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
    public class CostCenterController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CostCenterController(ILogger<CostCenterController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCostCenter([FromBody]CostCenterDTO _sarpara)
        {
            CostCenterDTO _CostCenter = new CostCenterDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenterById/" + _sarpara.CostCenterId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<CostCenterDTO>(valorrespuesta);

                }

                if (_CostCenter == null)
                {
                    _CostCenter = new CostCenterDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CostCenter);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CostCenter> _CostCenter = new List<CostCenter>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenter");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<List<CostCenter>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CostCenter.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetCostCenter([DataSourceRequest]DataSourceRequest request)
        {
            List<CostCenter> _CostCenter = new List<CostCenter>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenter");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<List<CostCenter>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_CostCenter.ToDataSourceResult(request));

        }
        

        [HttpPost("[action]")]
        public async Task<ActionResult<CostCenter>> SaveCostCenter([FromBody]CostCenter _CostCenter)
        {

            try
            {
                CostCenter _listCostCenter = new CostCenter();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenterById/" + _CostCenter.CostCenterId);
                string valorrespuesta = "";
                _CostCenter.FechaModificacion = DateTime.Now;
                _CostCenter.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                   
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCostCenter = JsonConvert.DeserializeObject<CostCenter>(valorrespuesta);
                }

                if (_listCostCenter == null) { _listCostCenter = new CostCenter(); }

                if (_listCostCenter.CostCenterId == 0)
                {
                    _CostCenter.FechaCreacion = DateTime.Now;
                    _CostCenter.UsuarioCreacion = HttpContext.Session.GetString("user");
                    CostCenterDTO _CostCenterDuplicated = new CostCenterDTO();
                    //string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenterByCostCenterName/" + _CostCenter.CostCenterName);
                    string valorrespuesta2 = "";

                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _CostCenterDuplicated = JsonConvert.DeserializeObject<CostCenterDTO>(valorrespuesta2);

                    }
                    if (_CostCenterDuplicated != null)
                    {

                        //  Errors = $"Ocurrio un error:{error} El password debe tener mayusculas y minusculas!"

                        string error = await result.Content.ReadAsStringAsync();
                        return await Task.Run(() => BadRequest($"El nombre del centro de costo ya esta ingresado..."));

                        /* return this.Json(new DataSourceResult
                         {
                             Errors = $"Ocurrio un error: {error} El centro de Costo ya esta ingresado."
                         });*/
                    }

                    var insertresult = await Insert(_CostCenter);
                }
                else
                {
                    CostCenterDTO _CostCenterDuplicated = new CostCenterDTO();
                    //string baseadress = config.Value.urlbase;
                    HttpClient _client2 = new HttpClient();
                    _client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var resultado = await _client.GetAsync(baseadress + "api/CostCenter/GetCostCenterByCostCenterName/" + _CostCenter.CostCenterName);
                    string valorrespuesta2 = "";

                    if (resultado.IsSuccessStatusCode)
                    {
                        valorrespuesta2 = await (resultado.Content.ReadAsStringAsync());
                        _CostCenterDuplicated = JsonConvert.DeserializeObject<CostCenterDTO>(valorrespuesta2);

                    }
                    if (_CostCenterDuplicated != null)
                    {
                        if(_CostCenterDuplicated.CostCenterId !=_CostCenter.CostCenterId)
                        //  Errors = $"Ocurrio un error:{error} El password debe tener mayusculas y minusculas!"
                        { 
                            string error = await result.Content.ReadAsStringAsync();
                            return await Task.Run(() => BadRequest($"El nombre del centro de costo ya esta ingresado..."));
                        }
                        /* return this.Json(new DataSourceResult
                         {
                             Errors = $"Ocurrio un error: {error} El centro de Costo ya esta ingresado."
                         });*/
                    }

                    var updateresult = await Update(_CostCenter.CostCenterId, _CostCenter);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");

                throw ex;
            }

            return Json(_CostCenter);
        }

        // POST: CostCenter/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CostCenter>> Insert(CostCenter _CostCenter)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CostCenter.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CostCenter.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CostCenter/Insert", _CostCenter);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<CostCenter>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");
            }
            return Ok(_CostCenter);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CostCenter }, Total = 1 });
        }

        [HttpPut("{CenterCostId}")]
        public async Task<IActionResult> Update(Int64 CostCenterId, CostCenter _CostCenter)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CostCenter/Update", _CostCenter);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<CostCenter>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _CostCenter }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CostCenter>> Delete([FromBody]CostCenter _CostCenter)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CostCenter/Delete", _CostCenter);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CostCenter = JsonConvert.DeserializeObject<CostCenter>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un Error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CostCenter }, Total = 1 });
        }





    }
}
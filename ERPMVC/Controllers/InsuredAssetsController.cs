using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERPMVC.Models;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using ERPMVC.DTO;
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class InsuredAssetsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public InsuredAssetsController(ILogger<InsuredAssetsController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Contabilidad.Seguros.Seguros Endosados")]
        public IActionResult InsuredAssets()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SFSegurosEndosados()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwAddInsuredAssets([FromBody]InsuredAssets _InsuredAssetsp)
        {
            InsuredAssets _InsuredAssets = new InsuredAssets();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssetsById/" + _InsuredAssetsp.Id);
                string valorrespuesta = "";
                _InsuredAssets.FechaCreacion = DateTime.Now;
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);

                }

                if (_InsuredAssets == null)
                {
                    _InsuredAssets = new InsuredAssets();
                    _InsuredAssets.FechaCreacion = DateTime.Now;
                }
                else
                {
                    // _InsuredAssets.NumeroDEIString = $"{_InsuredAssets.Sucursal}-{_InsuredAssets.Caja}-01-{_InsuredAssets.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InsuredAssets);

        }



        //COMIENZA APROVACIÓN
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<InsuredAssets>> Recibido([FromBody]InsuredAssets _InsuredAssets)
        {
            InsuredAssets _so = new InsuredAssets();
            if (_InsuredAssets != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssetsById/" + _InsuredAssets.Id);
                    string jsonresult = "";
                    jsonresult = JsonConvert.SerializeObject(_InsuredAssets);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);
                        _so.FechaCreacion = DateTime.Now;

                        var resultsalesorder = await Update(_so.Id, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        InsuredAssets resultado = ((InsuredAssets)(value));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }

            return await Task.Run(() => Ok(_so));
        }

        //TERMINA

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsuredAssets> _InsuredAssets = new List<InsuredAssets>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssets");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<List<InsuredAssets>>(valorrespuesta);
                    _InsuredAssets = _InsuredAssets.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsuredAssets.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetByInsurancePolicy([DataSourceRequest] DataSourceRequest request , int insurancePolicyId)
        {
            List<InsuredAssets> _InsuredAssets = new List<InsuredAssets>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssetsByInsurancePolicyId/"+ insurancePolicyId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<List<InsuredAssets>>(valorrespuesta);
                    _InsuredAssets = _InsuredAssets.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsuredAssets.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuredAssets>> SaveInsuredAssets([FromBody]InsuredAssets _InsuredAssets)
        {

            try
            {
                InsuredAssets _listInsuredAssets = new InsuredAssets();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuredAssets/GetInsuredAssetsById/" + _InsuredAssets.Id);
                string valorrespuesta = "";
                _InsuredAssets.FechaModificacion = DateTime.Now;
                _InsuredAssets.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);
                }

                if (_listInsuredAssets == null) { _listInsuredAssets = new InsuredAssets(); }

                if (_listInsuredAssets.Id == 0)
                {
                    _InsuredAssets.EstadoId = 1;                    

                    _InsuredAssets.FechaCreacion = DateTime.Now;
                    _InsuredAssets.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsuredAssets);
                    var value = (insertresult.Result as ObjectResult).Value;

                    InsuredAssets resultado = ((InsuredAssets)(value));
                    if (resultado.Id <= 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero la factura!"));
                    }
                    else
                    {
                        // _InsuredAssets.NumeroDEIString = $"{resultado.Sucursal}-01-{resultado.NumeroDEI.ToString().PadLeft(8, '0')} ";
                    }

                }
                else
                {
                    var updateresult = await Update(_InsuredAssets.Id, _InsuredAssets);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));

                throw ex;
            }

            return Json(_InsuredAssets);
        }

        // POST: InsuredAssets/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsuredAssets>> Insert(InsuredAssets _InsuredAssets)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InsuredAssets.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InsuredAssets.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuredAssets/Insert", _InsuredAssets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (ex);
            }
            return Ok(_InsuredAssets);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsuredAssets>> Update(Int64 id, InsuredAssets _InsuredAssets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsuredAssets/Update", _InsuredAssets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_InsuredAssets));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuredAssets }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuredAssets>> Delete([FromBody]InsuredAssets _InsuredAssets)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuredAssets/Delete", _InsuredAssets);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsuredAssets = JsonConvert.DeserializeObject<InsuredAssets>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_InsuredAssets));
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InsuredAssets }, Total = 1 });
        }

    }
}
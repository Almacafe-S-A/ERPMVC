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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class DimensionsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;


        public DimensionsController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: Branch
        public ActionResult Dimensions()
        {
            return View();
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> GetDimensions([DataSourceRequest]DataSourceRequest request)
        {
            List<Dimensions> _customers = new List<Dimensions> ();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dimensions/GetDimensions");///GetDimensions
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Dimensions>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }


        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Dimensions> _customers = new List<Dimensions> ();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dimensions/GetDimensions");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Dimensions>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }

        public async Task<ActionResult<Dimensions>> SaveDimensions([FromBody]DimensionsDTO _DimensionsP)
        {
            Dimensions _Dimensions = _DimensionsP;
            try
            {
                Dimensions _listDimensions = new Dimensions();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dimensions/GetDimensionsById/" + _Dimensions.Num);
                string valorrespuesta = "";
                _Dimensions.FechaModificacion = DateTime.Now;
                _Dimensions.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dimensions = JsonConvert.DeserializeObject<Dimensions>(valorrespuesta);
                }

               // if (_Dimensions == null) { }

                if (_Dimensions == null)
                {
                    _Dimensions = new Models.Dimensions();
                    _Dimensions.FechaCreacion = DateTime.Now;
                    _Dimensions.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DimensionsP);
                }
                else
                {
                    _DimensionsP.UsuarioCreacion = _Dimensions.UsuarioCreacion;
                    _DimensionsP.FechaCreacion = _Dimensions.FechaCreacion;
                    var updateresult = await Update(_Dimensions.Num, _DimensionsP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Dimensions);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddDimensions([FromBody]DimensionsDTO _sarpara)
        {
            DimensionsDTO _Dimensions = new DimensionsDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Dimensions/GetDimensionsById/" + _sarpara.Num);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dimensions = JsonConvert.DeserializeObject<DimensionsDTO>(valorrespuesta);

                }

                if (_Dimensions == null)
                {
                    _Dimensions = new DimensionsDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Dimensions);

        }

        // POST: Dimensions/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Dimensions _Dimensions)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Dimensions.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Dimensions.FechaCreacion = DateTime.Now;
                _Dimensions.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Dimensions/Insert", _Dimensions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dimensions = JsonConvert.DeserializeObject<Dimensions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Dimensions }, Total = 1 });
        }

        [HttpPut("DimensionsId")]
        public async Task<IActionResult> Update(string DimensionsId, Dimensions _Dimensions)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Dimensions/Update", _Dimensions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dimensions = JsonConvert.DeserializeObject<Dimensions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Dimensions }, Total = 1 });
        }

        [HttpDelete("DimensionsId")]
        public async Task<ActionResult<Dimensions>> Delete(string Num, Dimensions _Dimensions)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Dimensions/Delete", _Dimensions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Dimensions = JsonConvert.DeserializeObject<Dimensions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Dimensions }, Total = 1 });
        }


    }
}
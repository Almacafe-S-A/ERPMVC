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
    public class PEPSController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PEPSController(ILogger<PEPSController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult PEPS()
        {
            return View();
        }

        public IActionResult PEPSFind()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPEPS([FromBody]PESPDTO _sarpara)
        {
            PESPDTO _PEPS = new PESPDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PEPS/GetPEPSById/" + _sarpara.PEPSId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PESPDTO>(valorrespuesta);

                }

                if (_PEPS == null)
                {
                    _PEPS = new PESPDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_PEPS);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PEPS> _PEPS = new List<PEPS>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PEPS/GetPEPS");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<List<PEPS>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PEPS.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetByParams([DataSourceRequest]DataSourceRequest request,PEPS _PEPSP)
        {
            List<PEPS> _PEPS = new List<PEPS>();
            try
            {
               
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PEPS/GetByParams", _PEPSP);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<List<PEPS>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PEPS.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<PEPS>> SavePEPS([FromBody]PESPDTO _PEPSP)
        {
            PEPS _PEPS = _PEPSP;
            try
            {
                PEPS _listPEPS = new PEPS();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PEPS/GetPEPSById/" + _PEPS.PEPSId);
                string valorrespuesta = "";
                _PEPS.FechaModificacion = DateTime.Now;
                _PEPS.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);
                }

                if (_PEPS == null) { _PEPS = new Models.PEPS(); }

                if (_PEPSP.PEPSId == 0)
                {
                    _PEPSP.FechaCreacion = DateTime.Now;
                    _PEPSP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PEPSP);
                }
                else
                {
                    _PEPSP.UsuarioCreacion = _PEPS.UsuarioCreacion;
                    _PEPSP.FechaCreacion = _PEPS.FechaCreacion;
                    var updateresult = await Update(_PEPS.PEPSId, _PEPSP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PEPS);
        }

        // POST: PEPS/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<PEPS>> Insert(PEPS _PEPS)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PEPS.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PEPS.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/PEPS/Insert", _PEPS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_PEPS);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _PEPS }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PEPS>> Update(Int64 id, PEPS _PEPS)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/PEPS/Update", _PEPS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_PEPS);
           // return new ObjectResult(new DataSourceResult { Data = new[] { _PEPS }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<PEPS>> Delete(Int64 PEPSId, PEPS _PEPS)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/PEPS/Delete", _PEPS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _PEPS }, Total = 1 });
        }





    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class PEPSController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PEPSController(ILogger<PEPSController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwPEPS(Int64 Id = 0)
        {
            PEPS _PEPS = new PEPS();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PEPS/GetPEPSById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);

                }

                if (_PEPS == null)
                {
                    _PEPS = new PEPS();
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

        [HttpPost("[action]")]
        public async Task<ActionResult<PEPS>> SavePEPS([FromBody]PEPS _PEPS)
        {

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
                    _listPEPS = JsonConvert.DeserializeObject<PEPS>(valorrespuesta);
                }

                if (_listPEPS.PEPSId == 0)
                {
                    _PEPS.FechaCreacion = DateTime.Now;
                    _PEPS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PEPS);
                }
                else
                {
                    var updateresult = await Update(_PEPS.PEPSId, _PEPS);
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

        [HttpPost("[action]")]
        public async Task<ActionResult<PEPS>> Delete([FromBody]PEPS _PEPS)
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
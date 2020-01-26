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
    public class SeveridadRiesgoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public SeveridadRiesgoController(ILogger<SeveridadRiesgoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        //--------------------------------------------------------------------------------------

        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> GetSeveridadRiesgo([DataSourceRequest]DataSourceRequest request)
        {
            List<SeveridadRiesgo> _SeveridadRiesgo = new List<SeveridadRiesgo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SeveridadRiesgoes/GetSeveridadRiesgo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<List<SeveridadRiesgo>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _SeveridadRiesgo.ToDataSourceResult(request);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddSeveridadRiesgo([FromBody]SeveridadRiesgoDTO _sarpara)
        {
            SeveridadRiesgoDTO _ServeridadRiesgo = new SeveridadRiesgoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SeveridadRiesgoes/GetSeveridadRiesgoById/" + _sarpara.IdSeveridad);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ServeridadRiesgo = JsonConvert.DeserializeObject<SeveridadRiesgoDTO>(valorrespuesta);
                }

                if (_ServeridadRiesgo == null)
                {
                    _ServeridadRiesgo = new SeveridadRiesgoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_ServeridadRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<SeveridadRiesgo>> SaveSeveridadRiesgo([FromBody]SeveridadRiesgoDTO _SeveridadRiesgoP)
        {
            SeveridadRiesgo _SeveridadRiesgo = _SeveridadRiesgoP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SeveridadRiesgoes/GetSeveridadRiesgoById/" + _SeveridadRiesgo.IdSeveridad);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<SeveridadRiesgoDTO>(valorrespuesta);
                }

                if (_SeveridadRiesgo == null) { _SeveridadRiesgo = new Models.SeveridadRiesgo(); }

                if (_SeveridadRiesgoP.IdSeveridad == 0)
                {
                    _SeveridadRiesgoP.FechaCreacion = DateTime.Now;
                    _SeveridadRiesgoP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _SeveridadRiesgoP.FechaModificacion = DateTime.Now;
                    _SeveridadRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_SeveridadRiesgoP);
                }
                else
                {
                    _SeveridadRiesgoP.FechaCreacion = _SeveridadRiesgo.FechaCreacion;
                    _SeveridadRiesgoP.UsuarioCreacion = _SeveridadRiesgo.UsuarioCreacion;
                    _SeveridadRiesgoP.FechaModificacion = DateTime.Now;
                    _SeveridadRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_SeveridadRiesgo.IdSeveridad, _SeveridadRiesgoP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_SeveridadRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(SeveridadRiesgo _SeveridadRiesgoS)
        {
            SeveridadRiesgo _SeveridadRiesgo = _SeveridadRiesgoS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SeveridadRiesgoes/Insert", _SeveridadRiesgoS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgoS = JsonConvert.DeserializeObject<SeveridadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _SeveridadRiesgoS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 IdSeveridad, SeveridadRiesgo _SeveridadRiesgoP)
        {
            SeveridadRiesgo _SeveridadRiesgo = _SeveridadRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/SeveridadRiesgoes/Update", _SeveridadRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<SeveridadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _SeveridadRiesgo }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<SeveridadRiesgo>> Delete(Int64 Id, SeveridadRiesgo _SeveridadRiesgoP)
        {
            SeveridadRiesgo _SeveridadRiesgo = _SeveridadRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SeveridadRiesgoes/Delete", _SeveridadRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<SeveridadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _SeveridadRiesgo }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> BrechasEntreRango([DataSourceRequest]DataSourceRequest request)
        {
            List<SeveridadRiesgo> _SeveridadRiesgo = new List<SeveridadRiesgo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SeveridadRiesgoes/GetSeveridadRiesgo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SeveridadRiesgo = JsonConvert.DeserializeObject<List<SeveridadRiesgo>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _SeveridadRiesgo.ToDataSourceResult(request);
        }
    }
}
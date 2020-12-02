using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class ContextoRiesgoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public ContextoRiesgoController(ILogger<ContextoRiesgoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Policy = "Monitoreo.Contexto de Riesgos")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> GetContextoRiesgo([DataSourceRequest]DataSourceRequest request)
        {
            List<ContextoRiesgo> _ContextoRiesgo = new List<ContextoRiesgo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContextoRiesgo/GetContextoRiesgo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgo = JsonConvert.DeserializeObject<List<ContextoRiesgo>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _ContextoRiesgo.ToDataSourceResult(request);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddContextoRiesgo([FromBody]ContextoRiesgoDTO _sarpara)
        {
            ContextoRiesgoDTO _ContextoRiesgo = new ContextoRiesgoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContextoRiesgo/GetContextoRiesgoById/" + _sarpara.IdContextoRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgo = JsonConvert.DeserializeObject<ContextoRiesgoDTO>(valorrespuesta);
                }

                if (_ContextoRiesgo == null)
                {
                    _ContextoRiesgo = new ContextoRiesgoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_ContextoRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<ContextoRiesgo>> SaveContextoRiesgo([FromBody]ContextoRiesgoDTO _ContextoRiesgoP)
        {
            ContextoRiesgo _ContextoRiesgo = _ContextoRiesgoP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ContextoRiesgo/GetContextoRiesgoById/" + _ContextoRiesgo.IdContextoRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgo = JsonConvert.DeserializeObject<ContextoRiesgoDTO>(valorrespuesta);
                }

                if (_ContextoRiesgo == null) { _ContextoRiesgo = new Models.ContextoRiesgo(); }

                if (_ContextoRiesgoP.IdContextoRiesgo == 0)
                {
                    _ContextoRiesgoP.FechaCreacion = DateTime.Now;
                    _ContextoRiesgoP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _ContextoRiesgoP.FechaModificacion = DateTime.Now;
                    _ContextoRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ContextoRiesgoP);
                }
                else
                {
                    _ContextoRiesgoP.FechaCreacion = _ContextoRiesgo.FechaCreacion;
                    _ContextoRiesgoP.UsuarioCreacion = _ContextoRiesgo.UsuarioCreacion;
                    _ContextoRiesgoP.FechaModificacion = DateTime.Now;
                    _ContextoRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_ContextoRiesgo.IdContextoRiesgo, _ContextoRiesgoP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_ContextoRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(ContextoRiesgo _ContextoRiesgoS)
        {
            ContextoRiesgo _ContextoRiesgo = _ContextoRiesgoS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ContextoRiesgo/Insert", _ContextoRiesgoS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgoS = JsonConvert.DeserializeObject<ContextoRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ContextoRiesgoS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 IdContextoRiesgo, ContextoRiesgo _ContextoRiesgoP)
        {
            ContextoRiesgo _ContextoRiesgo = _ContextoRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ContextoRiesgo/Update", _ContextoRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgo = JsonConvert.DeserializeObject<ContextoRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ContextoRiesgo }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<ContextoRiesgo>> Delete(Int64 Id, [FromBody]ContextoRiesgo _ContextoRiesgoP)
        {
            ContextoRiesgo _ContextoRiesgo = _ContextoRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ContextoRiesgo/Delete", _ContextoRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ContextoRiesgo = JsonConvert.DeserializeObject<ContextoRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ContextoRiesgo }, Total = 1 });
        }
    }
}
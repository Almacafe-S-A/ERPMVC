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
    public class ProbabilidadRiesgoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public ProbabilidadRiesgoController(ILogger<ProbabilidadRiesgoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        //--------------------------------------------------------------------------------------
        [Authorize(Policy = "Monitoreo.Probabilidad de Riesgos")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> GetProbabilidadRiesgo([DataSourceRequest]DataSourceRequest request)
        {
            List<ProbabilidadRiesgo> _ProbabilidadRiesgo = new List<ProbabilidadRiesgo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProbabilidadRiesgo/GetProbabilidadRiesgo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgo = JsonConvert.DeserializeObject<List<ProbabilidadRiesgo>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _ProbabilidadRiesgo.ToDataSourceResult(request);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddProbabilidadRiesgo([FromBody]ProbabilidadRiesgoDTO _sarpara)
        {
            ProbabilidadRiesgoDTO _ProbabilidadRiesgo = new ProbabilidadRiesgoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProbabilidadRiesgo/GetProbabilidadRiesgoById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgo = JsonConvert.DeserializeObject<ProbabilidadRiesgoDTO>(valorrespuesta);
                }

                if (_ProbabilidadRiesgo == null)
                {
                    _ProbabilidadRiesgo = new ProbabilidadRiesgoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_ProbabilidadRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<ProbabilidadRiesgo>> SaveProbabilidadRiesgo([FromBody]ProbabilidadRiesgoDTO _ProbabilidadRiesgoP)
        {
            ProbabilidadRiesgo _ProbabilidadRiesgo = _ProbabilidadRiesgoP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProbabilidadRiesgo/GetProbabilidadRiesgoById/" + _ProbabilidadRiesgo.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgo = JsonConvert.DeserializeObject<ProbabilidadRiesgoDTO>(valorrespuesta);
                }

                if (_ProbabilidadRiesgo == null) { _ProbabilidadRiesgo = new Models.ProbabilidadRiesgo(); }

                if (_ProbabilidadRiesgoP.Id == 0)
                {
                    _ProbabilidadRiesgoP.FechaCreacion = DateTime.Now;
                    _ProbabilidadRiesgoP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _ProbabilidadRiesgoP.FechaModificacion = DateTime.Now;
                    _ProbabilidadRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProbabilidadRiesgoP);
                }
                else
                {
                    _ProbabilidadRiesgoP.FechaCreacion = _ProbabilidadRiesgo.FechaCreacion;
                    _ProbabilidadRiesgoP.UsuarioCreacion = _ProbabilidadRiesgo.UsuarioCreacion;
                    _ProbabilidadRiesgoP.FechaModificacion = DateTime.Now;
                    _ProbabilidadRiesgoP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_ProbabilidadRiesgo.Id, _ProbabilidadRiesgoP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_ProbabilidadRiesgo);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(ProbabilidadRiesgo _ProbabilidadRiesgoS)
        {
            ProbabilidadRiesgo _ProbabilidadRiesgo = _ProbabilidadRiesgoS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProbabilidadRiesgo/Insert", _ProbabilidadRiesgoS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgoS = JsonConvert.DeserializeObject<ProbabilidadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ProbabilidadRiesgoS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 IdProbabilidadRiesgo, ProbabilidadRiesgo _ProbabilidadRiesgoP)
        {
            ProbabilidadRiesgo _ProbabilidadRiesgo = _ProbabilidadRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ProbabilidadRiesgo/Update", _ProbabilidadRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgo = JsonConvert.DeserializeObject<ProbabilidadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ProbabilidadRiesgo }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<ProbabilidadRiesgo>> Delete(Int64 Id, [FromBody]ProbabilidadRiesgo _ProbabilidadRiesgoP)
        {
            ProbabilidadRiesgo _ProbabilidadRiesgo = _ProbabilidadRiesgoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProbabilidadRiesgo/Delete", _ProbabilidadRiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProbabilidadRiesgo = JsonConvert.DeserializeObject<ProbabilidadRiesgo>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _ProbabilidadRiesgo }, Total = 1 });
        }
    }
}
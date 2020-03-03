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
    public class JournalEntryConfigurationController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public JournalEntryConfigurationController(ILogger<JournalEntryConfigurationController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwJournalEntryConfiguration([FromBody]JournalEntryConfiguration _JournalEntryConfigurationp)
        {
            JournalEntryConfigurationDTO _JournalEntryConfiguration = new JournalEntryConfigurationDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfiguration/GetJournalEntryConfigurationById/" + _JournalEntryConfigurationp.JournalEntryConfigurationId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfiguration = JsonConvert.DeserializeObject<JournalEntryConfigurationDTO>(valorrespuesta);

                }

                if (_JournalEntryConfiguration == null)
                {
                    _JournalEntryConfiguration = new JournalEntryConfigurationDTO();
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_JournalEntryConfiguration);

        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntryConfiguration> _JournalEntryConfiguration = new List<JournalEntryConfiguration>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfiguration/GetJournalEntryConfiguration");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfiguration = JsonConvert.DeserializeObject<List<JournalEntryConfiguration>>(valorrespuesta);
                    _JournalEntryConfiguration = _JournalEntryConfiguration.OrderByDescending(q => q.JournalEntryConfigurationId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _JournalEntryConfiguration.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<JournalEntryConfiguration>> SaveJournalEntryConfiguration([FromBody]JournalEntryConfiguration _JournalEntryConfiguration)
        {

            try
            {
                JournalEntryConfiguration _listJournalEntryConfiguration = new JournalEntryConfiguration();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfiguration/GetJournalEntryConfigurationById/" + _JournalEntryConfiguration.JournalEntryConfigurationId);
                string valorrespuesta = "";
                _JournalEntryConfiguration.FechaModificacion = DateTime.Now;
                _JournalEntryConfiguration.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listJournalEntryConfiguration = JsonConvert.DeserializeObject<JournalEntryConfiguration>(valorrespuesta);
                }

                if (_listJournalEntryConfiguration == null)
                {
                    _listJournalEntryConfiguration = new JournalEntryConfiguration();
                }

                if (_listJournalEntryConfiguration.JournalEntryConfigurationId == 0)
                {
                    _JournalEntryConfiguration.FechaCreacion = DateTime.Now;
                    _JournalEntryConfiguration.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_JournalEntryConfiguration);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _JournalEntryConfiguration = ((JournalEntryConfiguration)(value));
                }
                else
                {
                    _JournalEntryConfiguration.FechaCreacion = _listJournalEntryConfiguration.FechaCreacion;
                    _JournalEntryConfiguration.UsuarioCreacion = _listJournalEntryConfiguration.UsuarioCreacion;
                    var updateresult = await Update(_JournalEntryConfiguration.JournalEntryConfigurationId, _JournalEntryConfiguration);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_JournalEntryConfiguration);
        }

        // POST: JournalEntryConfiguration/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<JournalEntryConfiguration>> Insert(JournalEntryConfiguration _JournalEntryConfiguration)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _JournalEntryConfiguration.UsuarioCreacion = HttpContext.Session.GetString("user");
                _JournalEntryConfiguration.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryConfiguration/Insert", _JournalEntryConfiguration);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfiguration = JsonConvert.DeserializeObject<JournalEntryConfiguration>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_JournalEntryConfiguration);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryConfiguration }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JournalEntryConfiguration>> Update(Int64 id, JournalEntryConfiguration _JournalEntryConfiguration)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/JournalEntryConfiguration/Update", _JournalEntryConfiguration);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfiguration = JsonConvert.DeserializeObject<JournalEntryConfiguration>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryConfiguration }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<JournalEntryConfiguration>> Delete([FromBody]JournalEntryConfiguration _JournalEntryConfiguration)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryConfiguration/Delete", _JournalEntryConfiguration);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfiguration = JsonConvert.DeserializeObject<JournalEntryConfiguration>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryConfiguration }, Total = 1 });
        }





    }
}
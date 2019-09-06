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
    public class AlertController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public AlertController(ILogger<AlertController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("[controller]/[action]")]
        public IActionResult Alerts()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAlert([FromBody]Alert _AlertP)
        {
            Alert _Alert = new Alert();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Alert/GetAlertById/" + _AlertP.AlertId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);

                }

                if (_Alert == null)
                {
                    _Alert = new Alert();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Alert);

        }

        public ActionResult Country()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddCountry([FromBody]CountryDTO _sarpara)
        {
            CountryDTO _Country = new CountryDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Country/GetCountryById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Country = JsonConvert.DeserializeObject<CountryDTO>(valorrespuesta);

                }

                if (_Country == null)
                {
                    _Country = new CountryDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Country);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Alert> _Alert = new List<Alert>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Alert/GetAlert");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alert = JsonConvert.DeserializeObject<List<Alert>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Alert.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Alert>> SaveAlert([FromBody]Alert _Alert)
        {

            try
            {
                Alert _listAlert = new Alert();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Alert/GetAlertById/" + _Alert.AlertId);
                string valorrespuesta = "";
                _Alert.FechaModificacion = DateTime.Now;
                _Alert.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listAlert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                }

                if (_listAlert.AlertId == 0)
                {
                    _Alert.FechaCreacion = DateTime.Now;
                    _Alert.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Alert);
                }
                else
                {
                    var updateresult = await Update(_Alert.AlertId, _Alert);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Alert);
        }

        // POST: Alert/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Alert>> Insert(Alert _Alert)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Alert.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Alert.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Alert/Insert", _Alert);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Alert);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Alert }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Alert>> Update(Int64 id, Alert _Alert)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Alert/Update", _Alert);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Alert }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Alert>> Delete([FromBody]Alert _Alert)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Alert/Delete", _Alert);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Alert }, Total = 1 });
        }





    }
}
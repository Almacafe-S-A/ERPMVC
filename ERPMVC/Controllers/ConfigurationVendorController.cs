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
    public class ConfigurationVendorController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ConfigurationVendorController(ILogger<ConfigurationVendorController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: ConfigurationVendor
        [Authorize(Policy = "Proveedores.Configuracion de Proveedores")]
        public ActionResult ConfigurationVendor()
        {
            return View();
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> GetConfigurationVendorActive([DataSourceRequest]DataSourceRequest request)
        {
            ConfigurationVendor _ConfigurationVendor = new ConfigurationVendor();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConfigurationVendor/GetConfigurationVendorActive");
     
                string valorrespuesta = ""; // 
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<ConfigurationVendor>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_ConfigurationVendor);

        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetConfigurationVendor([DataSourceRequest]DataSourceRequest request)
        {
            List<ConfigurationVendor> _ConfigurationVendor = new List<ConfigurationVendor>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConfigurationVendor/GetConfigurationVendor");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<List<ConfigurationVendor>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_ConfigurationVendor.ToDataSourceResult(request));

        }
        public async Task<ActionResult<ConfigurationVendor>> SaveConfigurationVendor([FromBody]ConfigurationVendorDTO _ConfigurationVendorP)
        {
            ConfigurationVendor _ConfigurationVendor = _ConfigurationVendorP;
            try
            {
                ConfigurationVendor _listAccount = new ConfigurationVendor();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConfigurationVendor/GetConfigurationVendorById/" + _ConfigurationVendor.ConfigurationVendorId);
                string valorrespuesta = "";
                _ConfigurationVendor.ModifiedDate = DateTime.Now;
                _ConfigurationVendor.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<ConfigurationVendor>(valorrespuesta);
                }

                if (_ConfigurationVendor == null) { _ConfigurationVendor = new Models.ConfigurationVendor(); }

                if (_ConfigurationVendorP.ConfigurationVendorId == 0)
                {
                    _ConfigurationVendor.CreatedDate = DateTime.Now;
                    _ConfigurationVendor.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ConfigurationVendorP);
                }
                else
                {
                    _ConfigurationVendorP.CreatedUser = _ConfigurationVendor.CreatedUser;
                    _ConfigurationVendorP.CreatedDate = _ConfigurationVendor.CreatedDate;
                    var updateresult = await Update(_ConfigurationVendor.ConfigurationVendorId, _ConfigurationVendorP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ConfigurationVendorP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddConfigurationVendor([FromBody]ConfigurationVendorDTO _sarpara)
        {
            ConfigurationVendorDTO _ConfigurationVendor = new ConfigurationVendorDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConfigurationVendor/GetConfigurationVendorById/" + _sarpara.ConfigurationVendorId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<ConfigurationVendorDTO>(valorrespuesta);

                }

                if (_ConfigurationVendor == null)
                {
                    _ConfigurationVendor = new ConfigurationVendorDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ConfigurationVendor);

        }

        // POST: ConfigurationVendor/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(ConfigurationVendor _ConfigurationVendor)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ConfigurationVendor.CreatedUser = HttpContext.Session.GetString("user");
                _ConfigurationVendor.CreatedDate = DateTime.Now;
                _ConfigurationVendor.ModifiedUser = HttpContext.Session.GetString("user");
                _ConfigurationVendor.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ConfigurationVendor/Insert", _ConfigurationVendor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<ConfigurationVendor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConfigurationVendor }, Total = 1 });
        }

        [HttpPut("ConfigurationVendorId")]
        public async Task<IActionResult> Update(Int64 ConfigurationVendorId, ConfigurationVendor _ConfigurationVendor)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ConfigurationVendor/Update", _ConfigurationVendor);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConfigurationVendor = JsonConvert.DeserializeObject<ConfigurationVendor>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConfigurationVendor }, Total = 1 });
        }

    }
}
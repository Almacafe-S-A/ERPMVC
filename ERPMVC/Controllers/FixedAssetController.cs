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
    public class FixedAssetController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FixedAssetController(ILogger<FixedAssetController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwFixedAsset(Int64 Id = 0)
        {
            FixedAsset _FixedAsset = new FixedAsset();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAsset/GetFixedAssetById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAsset = JsonConvert.DeserializeObject<FixedAsset>(valorrespuesta);

                }

                if (_FixedAsset == null)
                {
                    _FixedAsset = new FixedAsset();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_FixedAsset);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<FixedAsset> _FixedAsset = new List<FixedAsset>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAsset/GetFixedAsset");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAsset = JsonConvert.DeserializeObject<List<FixedAsset>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FixedAsset.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<FixedAsset>> SaveFixedAsset([FromBody]FixedAsset _FixedAsset)
        {

            try
            {
                FixedAsset _listFixedAsset = new FixedAsset();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAsset/GetFixedAssetById/" + _FixedAsset.FixedAssetId);
                string valorrespuesta = "";
                _FixedAsset.FechaModificacion = DateTime.Now;
                _FixedAsset.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFixedAsset = JsonConvert.DeserializeObject<FixedAsset>(valorrespuesta);
                }

                if (_listFixedAsset == null) { _listFixedAsset = new FixedAsset(); }

                if (_listFixedAsset.FixedAssetId == 0)
                {
                    _FixedAsset.FechaCreacion = DateTime.Now;
                    _FixedAsset.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_FixedAsset);
                }
                else
                {
                    var updateresult = await Update(_FixedAsset.FixedAssetId, _FixedAsset);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_FixedAsset);
        }

        // POST: FixedAsset/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<FixedAsset>> Insert(FixedAsset _FixedAsset)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _FixedAsset.UsuarioCreacion = HttpContext.Session.GetString("user");
                _FixedAsset.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/FixedAsset/Insert", _FixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAsset = JsonConvert.DeserializeObject<FixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_FixedAsset);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAsset }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FixedAsset>> Update(Int64 id, FixedAsset _FixedAsset)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/FixedAsset/Update", _FixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAsset = JsonConvert.DeserializeObject<FixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAsset }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<FixedAsset>> Delete([FromBody]FixedAsset _FixedAsset)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/FixedAsset/Delete", _FixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAsset = JsonConvert.DeserializeObject<FixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAsset }, Total = 1 });
        }





    }
}
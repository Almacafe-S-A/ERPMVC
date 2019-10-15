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
    public class DepreciationFixedAssetController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DepreciationFixedAssetController(ILogger<DepreciationFixedAssetController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwDepreciationFixedAsset(Int64 Id = 0)
        {
            DepreciationFixedAsset _DepreciationFixedAsset = new DepreciationFixedAsset();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DepreciationFixedAsset/GetDepreciationFixedAssetById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DepreciationFixedAsset = JsonConvert.DeserializeObject<DepreciationFixedAsset>(valorrespuesta);

                }

                if (_DepreciationFixedAsset == null)
                {
                    _DepreciationFixedAsset = new DepreciationFixedAsset();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_DepreciationFixedAsset);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<DepreciationFixedAsset> _DepreciationFixedAsset = new List<DepreciationFixedAsset>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DepreciationFixedAsset/GetDepreciationFixedAsset");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DepreciationFixedAsset = JsonConvert.DeserializeObject<List<DepreciationFixedAsset>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _DepreciationFixedAsset.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DepreciationFixedAsset>> SaveDepreciationFixedAsset([FromBody]DepreciationFixedAsset _DepreciationFixedAsset)
        {

            try
            {
                DepreciationFixedAsset _listDepreciationFixedAsset = new DepreciationFixedAsset();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DepreciationFixedAsset/GetDepreciationFixedAssetById/" + _DepreciationFixedAsset.DepreciationFixedAssetId);
                string valorrespuesta = "";
                _DepreciationFixedAsset.FechaModificacion = DateTime.Now;
                _DepreciationFixedAsset.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listDepreciationFixedAsset = JsonConvert.DeserializeObject<DepreciationFixedAsset>(valorrespuesta);
                }

                if (_listDepreciationFixedAsset == null) { _listDepreciationFixedAsset = new DepreciationFixedAsset(); }

                if (_listDepreciationFixedAsset.DepreciationFixedAssetId == 0)
                {
                    _DepreciationFixedAsset.FechaCreacion = DateTime.Now;
                    _DepreciationFixedAsset.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DepreciationFixedAsset);
                }
                else
                {
                    var updateresult = await Update(_DepreciationFixedAsset.DepreciationFixedAssetId, _DepreciationFixedAsset);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_DepreciationFixedAsset);
        }

        // POST: DepreciationFixedAsset/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<DepreciationFixedAsset>> Insert(DepreciationFixedAsset _DepreciationFixedAsset)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _DepreciationFixedAsset.UsuarioCreacion = HttpContext.Session.GetString("user");
                _DepreciationFixedAsset.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/DepreciationFixedAsset/Insert", _DepreciationFixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DepreciationFixedAsset = JsonConvert.DeserializeObject<DepreciationFixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_DepreciationFixedAsset);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _DepreciationFixedAsset }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepreciationFixedAsset>> Update(Int64 id, DepreciationFixedAsset _DepreciationFixedAsset)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/DepreciationFixedAsset/Update", _DepreciationFixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DepreciationFixedAsset = JsonConvert.DeserializeObject<DepreciationFixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _DepreciationFixedAsset }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DepreciationFixedAsset>> Delete([FromBody]DepreciationFixedAsset _DepreciationFixedAsset)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/DepreciationFixedAsset/Delete", _DepreciationFixedAsset);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DepreciationFixedAsset = JsonConvert.DeserializeObject<DepreciationFixedAsset>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _DepreciationFixedAsset }, Total = 1 });
        }





    }
}
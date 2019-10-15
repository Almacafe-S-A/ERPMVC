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
    public class FixedAssetGroupController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FixedAssetGroupController(ILogger<FixedAssetGroupController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwFixedAssetGroup(Int64 Id = 0)
        {
            FixedAssetGroup _FixedAssetGroup = new FixedAssetGroup();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAssetGroup/GetFixedAssetGroupById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAssetGroup = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);

                }

                if (_FixedAssetGroup == null)
                {
                    _FixedAssetGroup = new FixedAssetGroup();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_FixedAssetGroup);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<FixedAssetGroup> _FixedAssetGroup = new List<FixedAssetGroup>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAssetGroup/GetFixedAssetGroup");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAssetGroup = JsonConvert.DeserializeObject<List<FixedAssetGroup>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FixedAssetGroup.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<FixedAssetGroup>> SaveFixedAssetGroup([FromBody]FixedAssetGroup _FixedAssetGroup)
        {

            try
            {
                FixedAssetGroup _listFixedAssetGroup = new FixedAssetGroup();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAssetGroup/GetFixedAssetGroupById/" + _FixedAssetGroup.FixedAssetGroupId);
                string valorrespuesta = "";
                _FixedAssetGroup.FechaModificacion = DateTime.Now;
                _FixedAssetGroup.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFixedAssetGroup = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);
                }

                if (_listFixedAssetGroup == null) { _listFixedAssetGroup = new FixedAssetGroup(); }

                if (_listFixedAssetGroup.FixedAssetGroupId == 0)
                {
                    _FixedAssetGroup.FechaCreacion = DateTime.Now;
                    _FixedAssetGroup.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_FixedAssetGroup);
                }
                else
                {
                    var updateresult = await Update(_FixedAssetGroup.FixedAssetGroupId, _FixedAssetGroup);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_FixedAssetGroup);
        }

        // POST: FixedAssetGroup/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<FixedAssetGroup>> Insert(FixedAssetGroup _FixedAssetGroup)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _FixedAssetGroup.UsuarioCreacion = HttpContext.Session.GetString("user");
                _FixedAssetGroup.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/FixedAssetGroup/Insert", _FixedAssetGroup);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAssetGroup = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_FixedAssetGroup);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAssetGroup }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FixedAssetGroup>> Update(Int64 id, FixedAssetGroup _FixedAssetGroup)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/FixedAssetGroup/Update", _FixedAssetGroup);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAssetGroup = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            //  return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAssetGroup }, Total = 1 });
            return Ok(_FixedAssetGroup);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<FixedAssetGroup>> Delete([FromBody]FixedAssetGroup _FixedAssetGroup)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/FixedAssetGroup/Delete", _FixedAssetGroup);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FixedAssetGroup = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return Ok(_FixedAssetGroup);

            //return new ObjectResult(new DataSourceResult { Data = new[] { _FixedAssetGroup }, Total = 1 });
        }





    }
}
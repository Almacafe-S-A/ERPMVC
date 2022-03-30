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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class FixedAssetGroupController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public FixedAssetGroupController(ILogger<FixedAssetGroupController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Contabilidad.Activos.Grupo de Activos")]
        public ActionResult FixedAssetGroup()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public async Task<ActionResult> pvwAddFixedAssetGroup([FromBody]FixedAssetGroupDTO _sarpara)
        {
            FixedAssetGroupDTO _Grupo = new FixedAssetGroupDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FixedAssetGroup/GetFixedAssetGroupById/" + _sarpara.FixedAssetGroupId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Grupo = JsonConvert.DeserializeObject<FixedAssetGroupDTO>(valorrespuesta);

                }

                if (_Grupo == null)
                {
                    _Grupo = new FixedAssetGroupDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView(_Grupo);
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


        public async Task<DataSourceResult> GetActivos([DataSourceRequest] DataSourceRequest request)
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
                    _FixedAssetGroup = JsonConvert.DeserializeObject<List<FixedAssetGroup>>(valorrespuesta).Where(w => w.IdEstado == 1).ToList();

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

        [HttpDelete("FixedAssetGroupId")]
        public async Task<ActionResult<FixedAssetGroup>> Delete(Int64 Id, FixedAssetGroup _FixedAssetGroupP)
        {
            FixedAssetGroup _Grupo = _FixedAssetGroupP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/FixedAssetGroup/Delete", _Grupo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Grupo = JsonConvert.DeserializeObject<FixedAssetGroup>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Grupo }, Total = 1 });
        }        
    }
}
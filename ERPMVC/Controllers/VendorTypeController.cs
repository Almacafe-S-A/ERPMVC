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
    public class VendorTypeController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public VendorTypeController(ILogger<VendorTypeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public ActionResult VendorType()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetVendorType([DataSourceRequest]DataSourceRequest request)
        {
            List<VendorType> _VendorType = new List<VendorType>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorType/GetVendorType");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<List<VendorType>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_VendorType.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult> Insert(VendorType _VendorType)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _VendorType.UsuarioCreacion = HttpContext.Session.GetString("user");
                _VendorType.FechaCreacion = DateTime.Now;
                _VendorType.UsuarioModificacion = HttpContext.Session.GetString("user");
                _VendorType.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorType/Insert", _VendorType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<VendorType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _VendorType }, Total = 1 });
        }


        [HttpPut("VendorTypeId")]
        public async Task<IActionResult> Update(Int64 VendorTypeId, VendorType _VendorType)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/VendorType/Update", _VendorType);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<VendorType>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _VendorType }, Total = 1 });
        }

        public async Task<ActionResult<VendorType>> SaveVendorType([FromBody]VendorTypeDTO _VendorTypeP)
        {
            VendorType _VendorType = _VendorTypeP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorType/GetVendorTypeById/" + _VendorType.VendorTypeId);
                string valorrespuesta = "";
                _VendorType.FechaModificacion = DateTime.Now;
                _VendorType.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<VendorType>(valorrespuesta);
                }

                if (_VendorType == null) { _VendorType = new Models.VendorType(); }

                if (_VendorTypeP.VendorTypeId == 0)
                {
                    _VendorType.FechaCreacion = DateTime.Now;
                    _VendorType.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_VendorTypeP);
                }
                else
                {
                    _VendorTypeP.UsuarioCreacion = _VendorType.UsuarioCreacion;
                    _VendorTypeP.FechaCreacion = _VendorType.FechaCreacion;
                    var updateresult = await Update(_VendorType.VendorTypeId, _VendorTypeP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_VendorTypeP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddVendorType([FromBody]VendorTypeDTO _sarpara)
        {
            VendorTypeDTO _VendorType = new VendorTypeDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorType/GetVendorTypeById/" + _sarpara.VendorTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<VendorTypeDTO>(valorrespuesta);

                }

                if (_VendorType == null)
                {
                    _VendorType = new VendorTypeDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_VendorType);

        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetVendorTypeById(Int64 VendorTypeId)
        {
            VendorType _VendorType = new VendorType();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorType/GetVendorTypeById/" + VendorTypeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _VendorType = JsonConvert.DeserializeObject<VendorType>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_VendorType));
        }

    }
}
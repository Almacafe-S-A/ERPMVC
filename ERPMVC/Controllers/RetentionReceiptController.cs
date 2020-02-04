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
    public class RetentionReceiptController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public RetentionReceiptController(ILogger<RetentionReceiptController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<RetentionReceipt> _RetentionReceipt = new List<RetentionReceipt>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/RetentionReceipt/GetRetentionReceipt");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceipt = JsonConvert.DeserializeObject<List<RetentionReceipt>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => _RetentionReceipt.ToDataSourceResult(request));
        }

        //--------------------------------------------------------------------------------------

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddRetentionReceipt([FromBody]RetentionReceiptDTO _sarpara)
        {
            RetentionReceiptDTO _RetentionReceipt = new RetentionReceiptDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/RetentionReceipt/GetRetentionReceiptById/" + _sarpara.RetentionReceiptId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceipt = JsonConvert.DeserializeObject<RetentionReceiptDTO>(valorrespuesta);
                }
                if (_RetentionReceipt == null)
                {
                    _RetentionReceipt = new RetentionReceiptDTO { RetentionReceiptId = 0, FechaEmision = DateTime.Now, DueDate = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_RetentionReceipt);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<RetentionReceiptDTO>> SaveComprobanteRetencion([FromBody]RetentionReceiptDTO _RetentionReceiptP)
        {
            RetentionReceipt _RetentionReceipt = _RetentionReceiptP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/RetentionReceipt/GetRetentionReceiptById/" + _RetentionReceipt.RetentionReceiptId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceipt = JsonConvert.DeserializeObject<RetentionReceiptDTO>(valorrespuesta);
                }

                if (_RetentionReceipt == null) { _RetentionReceipt = new Models.RetentionReceipt(); }

                if (_RetentionReceipt.RetentionReceiptId == 0)
                {
                    _RetentionReceiptP.FechaCreacion = DateTime.Now;
                    _RetentionReceiptP.FechaModificacion = DateTime.Now;
                    _RetentionReceiptP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _RetentionReceiptP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_RetentionReceiptP);
                }
                else
                {
                    _RetentionReceiptP.FechaCreacion = _RetentionReceipt.FechaCreacion;
                    _RetentionReceiptP.UsuarioCreacion = _RetentionReceipt.UsuarioCreacion;
                    _RetentionReceiptP.FechaModificacion = DateTime.Now;
                    _RetentionReceiptP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_RetentionReceipt.RetentionReceiptId, _RetentionReceiptP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_RetentionReceipt);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(RetentionReceipt _RetentionReceiptS)
        {
            RetentionReceipt _RetentionReceipt = _RetentionReceiptS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/RetentionReceipt/Insert", _RetentionReceiptS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceiptS = JsonConvert.DeserializeObject<RetentionReceipt>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _RetentionReceiptS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, RetentionReceipt _RetentionReceiptP)
        {
            RetentionReceipt _RetentionReceipt = _RetentionReceiptP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/RetentionReceipt/Update", _RetentionReceipt);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceipt = JsonConvert.DeserializeObject<RetentionReceipt>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _RetentionReceipt }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<RetentionReceipt>> Delete(Int64 Id, [FromBody]RetentionReceipt _RetentionReceiptP)
        {
            RetentionReceipt _RetentionReceipt = _RetentionReceiptP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/RetentionReceipt/Delete", _RetentionReceipt);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _RetentionReceipt = JsonConvert.DeserializeObject<RetentionReceipt>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _RetentionReceipt }, Total = 1 });
        }
    }
}
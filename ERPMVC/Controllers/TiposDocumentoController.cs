using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
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
    public class TiposDocumentoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public TiposDocumentoController(ILogger<TiposDocumentoController> logger, IOptions<MyConfig> _config)
        {
            config = _config;
            this._logger = logger;
        }

        public ActionResult TiposDocumento()
        {
            return View();
        }


        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetTiposDocumento([DataSourceRequest]DataSourceRequest request)
        {
            List<TiposDocumento> _clientes = new List<TiposDocumento>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TiposDocumento/GetTipoDocumento");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<TiposDocumento>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_clientes.ToDataSourceResult(request));

        }

        [HttpPost]
        public async Task<ActionResult<TiposDocumento>> SaveTiposDocumento([FromBody]TiposDocumentoDTO _TiposDocumentoS)
        {
            TiposDocumento _TiposDocumento = _TiposDocumentoS;
            try
            {
                //TiposDocumento _listTiposDocumento = new TiposDocumento();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TiposDocumento/GetTipoDocumentoById/" + _TiposDocumento.IdTipoDocumento);
                string valorrespuesta = "";
                _TiposDocumento.FechaModificacion = DateTime.Now;
                _TiposDocumento.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TiposDocumento = JsonConvert.DeserializeObject<TiposDocumentoDTO>(valorrespuesta);
                }

                if (_TiposDocumento == null) { _TiposDocumento = new Models.TiposDocumento(); }

                if (_TiposDocumentoS.IdTipoDocumento == 0)
                {
                    _TiposDocumento.FechaCreacion = DateTime.Now;
                    _TiposDocumento.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TiposDocumentoS);
                }
                else
                {
                    var updateresult = await Update(_TiposDocumento.IdTipoDocumento, _TiposDocumentoS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TiposDocumento);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddTiposDocumentos([FromBody]TiposDocumentoDTO _sarpara)
        {
            TiposDocumentoDTO _TiposDocumento = new TiposDocumentoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TiposDocumento/GetTipoDocumentoById/" + _sarpara.IdTipoDocumento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TiposDocumento = JsonConvert.DeserializeObject<TiposDocumentoDTO>(valorrespuesta);

                }

                if (_TiposDocumento == null)
                {
                    _TiposDocumento = new TiposDocumentoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TiposDocumento);

        }


        // POST: TiposDocumento/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(TiposDocumento _TiposDocumento)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TiposDocumento.UsuarioCreacion = HttpContext.Session.GetString("user");
                _TiposDocumento.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/TiposDocumento/Insert", _TiposDocumento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TiposDocumento = JsonConvert.DeserializeObject<TiposDocumento>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TiposDocumento }, Total = 1 });
        }

        [HttpPut("IdTipoDocumento")]
        public async Task<IActionResult> Update(Int64 IdTipoDocumento, TiposDocumento _TipoDocumento)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/TiposDocumento/Update", _TipoDocumento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoDocumento = JsonConvert.DeserializeObject<TiposDocumento>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoDocumento }, Total = 1 });
        }

        [HttpDelete ("IdTipoDocumento")]
        public async Task<ActionResult<TiposDocumento>> Delete(Int64 IdTipoDocumento, TiposDocumento _TiposDocumento)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/TiposDocumento/Delete", _TiposDocumento);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TiposDocumento = JsonConvert.DeserializeObject<TiposDocumento>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _TiposDocumento }, Total = 1 });
        }









    }
}
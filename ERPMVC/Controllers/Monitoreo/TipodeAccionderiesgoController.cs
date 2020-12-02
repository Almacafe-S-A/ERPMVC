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
using System.Security.Claims;


namespace ERPMVC.Controllers
{

    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

    public class TipodeAccionderiesgoController : Controller
    {

        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public TipodeAccionderiesgoController(ILogger<BankController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        
        [Authorize(Policy = "Monitoreo.Tipo de accion de riesgo")]
        public IActionResult TipodeAccionesRiesgos()
        {
            ViewData["permisos"] = _principal;
            return View();
        }
                      

        public async Task<ActionResult> pvwAddTipodeAccionesRiesgos([FromBody]TipodeAccionderiesgo _Data)
        {
            TipodeAccionderiesgoDTO _TipodeAccionderiesgo = new TipodeAccionderiesgoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipodeAccionderiesgo/GetTipodeAccionderiesgoId/" + _Data.TipodeAccionderiesgoId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipodeAccionderiesgo = JsonConvert.DeserializeObject<TipodeAccionderiesgoDTO>(valorrespuesta);

                }

                if (_TipodeAccionderiesgo == null)
                {
                    _TipodeAccionderiesgo = new TipodeAccionderiesgoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TipodeAccionderiesgo);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetTipodeAccionderiesgo([DataSourceRequest]DataSourceRequest request)
        {
            List<TipodeAccionderiesgo> _TipodeAccionderiesgo = new List<TipodeAccionderiesgo>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipodeAccionderiesgo/GetTipodeAccionderiesgo");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipodeAccionderiesgo = JsonConvert.DeserializeObject<List<TipodeAccionderiesgo>>(valorrespuesta);
                    _TipodeAccionderiesgo = _TipodeAccionderiesgo.OrderByDescending(e => e.TipodeAccionderiesgoId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _TipodeAccionderiesgo.ToDataSourceResult(request);

        }



        
        [HttpPost("[action]")]
        public async Task<ActionResult<TipodeAccionderiesgo>> SaveTipodeAccionderiesgo([FromBody]TipodeAccionderiesgoDTO _TipodeAccionderiesgoS)
        {
            TipodeAccionderiesgo _TipodeAccionderiesgo = _TipodeAccionderiesgoS;
            try
            {
               
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (_TipodeAccionderiesgo.TipodeAccionderiesgoId == 0)
                {
                    var result = await _client.GetAsync(baseadress + "api/TipodeAccionderiesgo/GetTipodeAccionderiesgobyTipoAccion/" + _TipodeAccionderiesgo.Tipodeaccion);           
                    string valorrespuesta = "";
                    _TipodeAccionderiesgo.FechaModificacion = DateTime.Now;
                    _TipodeAccionderiesgo.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _TipodeAccionderiesgo = JsonConvert.DeserializeObject<TipodeAccionderiesgo>(valorrespuesta);
                    }

                    if (_TipodeAccionderiesgo == null) { _TipodeAccionderiesgo = new Models.TipodeAccionderiesgo(); }

                    if (_TipodeAccionderiesgo.TipodeAccionderiesgoId > 0)
                    {
                        return await Task.Run(() => BadRequest($"Ya existe un Tipo de acción de riesgo registrado con ese nombre."));
                    }
                }

                if (_TipodeAccionderiesgoS.TipodeAccionderiesgoId == 0)
                {
                    _TipodeAccionderiesgoS.FechaCreacion = DateTime.Now;
                    _TipodeAccionderiesgoS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TipodeAccionderiesgoS);
                }
                else
                {
                    _TipodeAccionderiesgoS.UsuarioCreacion = _TipodeAccionderiesgo.UsuarioCreacion;
                    _TipodeAccionderiesgoS.FechaCreacion = _TipodeAccionderiesgo.FechaCreacion;
                    _TipodeAccionderiesgoS.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _TipodeAccionderiesgoS.FechaModificacion = DateTime.Now;
                    var updateresult = await Update(_TipodeAccionderiesgo.TipodeAccionderiesgoId, _TipodeAccionderiesgoS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TipodeAccionderiesgoS);
        }

        // POST: Bank/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TipodeAccionderiesgo>> Insert(TipodeAccionderiesgo _TipodeAccionderiesgo)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TipodeAccionderiesgo.UsuarioCreacion = HttpContext.Session.GetString("user");
                _TipodeAccionderiesgo.UsuarioModificacion = HttpContext.Session.GetString("user");
                _TipodeAccionderiesgo.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipodeAccionderiesgo/Insert", _TipodeAccionderiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipodeAccionderiesgo = JsonConvert.DeserializeObject<TipodeAccionderiesgo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_TipodeAccionderiesgo);
          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 id, TipodeAccionderiesgo _TipodeAccionderiesgo)
        {
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/TipodeAccionderiesgo/Update", _TipodeAccionderiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipodeAccionderiesgo = JsonConvert.DeserializeObject<TipodeAccionderiesgo>(valorrespuesta);
                }               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipodeAccionderiesgo }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<TipodeAccionderiesgo>> Delete(Int64 BankId, TipodeAccionderiesgo _TipodeAccionderiesgo)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/TipodeAccionderiesgo/Delete", _TipodeAccionderiesgo);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipodeAccionderiesgo = JsonConvert.DeserializeObject<TipodeAccionderiesgo>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _TipodeAccionderiesgo }, Total = 1 });
        }



    }
}
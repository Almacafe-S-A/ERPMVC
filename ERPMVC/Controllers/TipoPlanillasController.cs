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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class TipoPlanillasController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public TipoPlanillasController(ILogger<TipoPlanillasController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        public ActionResult TipoPlanillas()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoPlanillas> _TipoPlanillas = new List<TipoPlanillas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<List<TipoPlanillas>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TipoPlanillas.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> GetTipoPlanillas([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoPlanillas> _TipoPlanillas = new List<TipoPlanillas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<List<TipoPlanillas>>(valorrespuesta);
                    _TipoPlanillas = _TipoPlanillas.Where(q => q.Estado == "Activo").ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TipoPlanillas.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoPlanillas> _TipoPlanillas = new List<TipoPlanillas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetPlanilla");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<List<TipoPlanillas>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TipoPlanillas);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddTipoPlanillas([FromBody]TipoPlanillasDTO _sarpara)
        {
            TipoPlanillasDTO _TipoPlanillas = new TipoPlanillasDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillasById/" + _sarpara.IdTipoPlanilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<TipoPlanillasDTO>(valorrespuesta);

                }

                if (_TipoPlanillas == null)
                {
                    _TipoPlanillas = new TipoPlanillasDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TipoPlanillas);

        }

        
        [HttpPost]
        public async Task<ActionResult<TipoPlanillas>> SaveTipoPlanillas([FromBody]TipoPlanillasDTO _TipoPlanillasP)
        {

            TipoPlanillas _TipoPlanillas = _TipoPlanillasP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillasById/" + _TipoPlanillas.IdTipoPlanilla);
                string valorrespuesta = "";
                _TipoPlanillas.FechaModificacion = DateTime.Now;
                _TipoPlanillas.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<TipoPlanillasDTO>(valorrespuesta);
                }

                if (_TipoPlanillas == null) { _TipoPlanillas = new Models.TipoPlanillas(); }

                if (_TipoPlanillasP.IdTipoPlanilla == 0)
                {
                    _TipoPlanillas.FechaCreacion = DateTime.Now;
                    _TipoPlanillas.Usuariomodificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TipoPlanillasP);
                }
                else
                {
                    _TipoPlanillasP.Usuariocreacion = _TipoPlanillas.Usuariocreacion;
                    _TipoPlanillasP.FechaCreacion = _TipoPlanillas.FechaCreacion;
                    var updateresult = await Update(_TipoPlanillas.IdTipoPlanilla, _TipoPlanillasP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TipoPlanillas);
        }


        //--------------------------------------------------------------------------------------
        // POST: Planilla/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(TipoPlanillas _TipoPlanillasP)
        {
            TipoPlanillas _TipoPlanillas = _TipoPlanillasP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TipoPlanillas.Usuariocreacion = HttpContext.Session.GetString("user");
                _TipoPlanillas.Usuariomodificacion = HttpContext.Session.GetString("user");
                _TipoPlanillas.FechaCreacion = DateTime.Now;
                _TipoPlanillas.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoPlanillas/Insert", _TipoPlanillas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<TipoPlanillas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoPlanillas }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, TipoPlanillas _TipoPlanillasP)
        {
            TipoPlanillas _TipoPlanillas = _TipoPlanillasP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TipoPlanillas.FechaModificacion = DateTime.Now;
                _TipoPlanillas.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/TipoPlanillas/Update", _TipoPlanillas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<TipoPlanillas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoPlanillas }, Total = 1 });
        }


        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<TipoPlanillas>> Delete([FromBody]TipoPlanillas _TipoPlanillasP)
        {
            TipoPlanillas _TipoPlanillas = _TipoPlanillasP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoPlanillas/Delete", _TipoPlanillas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoPlanillas = JsonConvert.DeserializeObject<TipoPlanillas>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoPlanillas }, Total = 1 });
        }

    }
}
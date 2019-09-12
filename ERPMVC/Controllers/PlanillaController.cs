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
    public class PlanillaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PlanillaController(ILogger<PlanillaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Customer
        public ActionResult Planilla()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Planilla> _Planilla = new List<Planilla>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanilla");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<List<Planilla>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Planilla.ToDataSourceResult(request));

        }
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<Planilla> _Planilla = new List<Planilla>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanilla");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<List<Planilla>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Planilla);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPlanilla([FromBody]PlanillaDTO _sarpara)
        {
            PlanillaDTO _Planilla = new PlanillaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanillaById/" + _sarpara.IdPlanilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<PlanillaDTO>(valorrespuesta);

                }

                if (_Planilla == null)
                {
                    _Planilla = new PlanillaDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Planilla);

        }

        
        [HttpPost]
        public async Task<ActionResult<Planilla>> SavePlanilla([FromBody]PlanillaDTO _PlanillaP)
        {

            Planilla _Planilla = _PlanillaP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanillaById/" + _Planilla.IdPlanilla);
                string valorrespuesta = "";
                _Planilla.FechaModificacion = DateTime.Now;
                _Planilla.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<PlanillaDTO>(valorrespuesta);
                }

                if (_Planilla == null) { _Planilla = new Models.Planilla(); }

                if (_PlanillaP.IdPlanilla == 0)
                {
                    _Planilla.FechaCreacion = DateTime.Now;
                    _Planilla.Usuariomodificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PlanillaP);
                }
                else
                {
                    _PlanillaP.Usuariocreacion = _Planilla.Usuariocreacion;
                    _PlanillaP.FechaCreacion = _Planilla.FechaCreacion;
                    var updateresult = await Update(_Planilla.IdPlanilla, _PlanillaP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Planilla);
        }


        //--------------------------------------------------------------------------------------
        // POST: Planilla/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Planilla _PlanillaP)
        {
            Planilla _Planilla = _PlanillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Planilla.Usuariocreacion = HttpContext.Session.GetString("user");
                _Planilla.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Planilla.FechaCreacion = DateTime.Now;
                _Planilla.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Planilla/Insert", _Planilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Planilla }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, Planilla _PlanillaP)
        {
            Planilla _Planilla = _PlanillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Planilla.FechaModificacion = DateTime.Now;
                _Planilla.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Planilla/Update", _Planilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Planilla }, Total = 1 });
        }



    }
}
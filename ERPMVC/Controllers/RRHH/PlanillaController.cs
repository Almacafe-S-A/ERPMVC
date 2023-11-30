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
    public class PlanillaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public PlanillaController(ILogger<PlanillaController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: Customer
        //[Authorize(Policy = "RRHH.Parametros Tipo de Planilla")]
        public ActionResult TipoPlanillas()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            List<Planilla> planilla = new List<Planilla>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planillas/Get");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<List<Planilla>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return Json(planilla.ToDataSourceResult(request));

        }

        [HttpGet]
        public async Task<JsonResult> GetById([DataSourceRequest] DataSourceRequest request, int Id)
        {
            List<Planilla> planilla = new List<Planilla>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Planillas/GetById/{Id}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<List<Planilla>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return Json(planilla.ToDataSourceResult(request));

        }




        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddPlanilla([FromBody] Planilla _sarpara)
        {
            Planilla planilla = new Planilla();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillasById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);

                }

                if (planilla == null)
                {
                    planilla = new Planilla();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }



            return PartialView(planilla);

        }


        [HttpPost]
        public async Task<ActionResult<Planilla>> SaveTipoPlanillas([FromBody] Planilla planillaP)
        {

            Planilla planilla = planillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoPlanillas/GetTipoPlanillasById/" + planilla.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }

                if (planilla == null)
                    planilla = new Models.Planilla();

                ActionResult actionResult = null;
                if (planillaP.Id == 0)
                    actionResult = await Insert(planillaP);
                else
                    actionResult = await Update(planillaP);

                if (actionResult is BadRequestObjectResult)
                {
                    return BadRequest(((BadRequestObjectResult)actionResult).Value);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            return Json(planilla);
        }


        //--------------------------------------------------------------------------------------
        // POST: Planilla/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Planilla planillaP)
        {
            Planilla planilla = planillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoPlanillas/Insert", planilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { planilla }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Planilla planillaP)
        {
            Planilla planilla = planillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/TipoPlanillas/Update", planilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { planilla }, Total = 1 });
        }


        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<Planilla>> Delete([FromBody] Planilla planillaP)
        {
            Planilla planilla = planillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoPlanillas/Delete", planilla);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { planilla }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

    }
}
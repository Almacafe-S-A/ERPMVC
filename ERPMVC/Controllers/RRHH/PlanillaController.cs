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

        public ActionResult Planilla()
        {
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
                var result = await _client.GetAsync(baseadress + "api/Planilla/Get");
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
        public async Task<JsonResult> GetDetalleById([DataSourceRequest] DataSourceRequest request,Planilla planilla)
        {
            List<PlanillaDetalle> planillas = new List<PlanillaDetalle>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Planilla/GetDetalleById/{planilla.Id}/{planilla.TipoPlanillaId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planillas = JsonConvert.DeserializeObject<List<PlanillaDetalle>>(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return Json(planillas.ToDataSourceResult(request));

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
                var result = await _client.GetAsync(baseadress + $"api/Planilla/GetById/{Id}");
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
        public async Task<ActionResult> pvwPlanilla([FromBody] Planilla _sarpara)
        {
            Planilla planilla = new Planilla();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanillaById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    planilla = JsonConvert.DeserializeObject<Planilla>(valorrespuesta);

                }

                if (planilla == null)
                {
                    planilla = new Planilla {
                        FechaPlanilla = DateTime.Now,   
                        
                    };
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
        public async Task<ActionResult<Planilla>> SavePlanilla([FromBody] Planilla planillaP)
        {

            Planilla planilla = planillaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Planilla/GetPlanillaById/" + planilla.Id);
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


        public async Task<ActionResult<GoodsDeliveryAuthorization>> Aprobar([FromBody] Planilla planilla)
        {
            try
            {
                if (planilla == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Planilla/ChangeStatus/{planilla.Id}/{2}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo el documento!"));
                }

                return await Task.Run(() => Json(goodsDeliveryAuthorization));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


        }

        public async Task<ActionResult<GoodsDeliveryAuthorization>> Revisar([FromBody] Planilla planilla)
        {
            try
            {
                if (planilla == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Planilla/ChangeStatus/{planilla.Id}/{1}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo el documento!"));
                }

                return await Task.Run(() => Json(goodsDeliveryAuthorization));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


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
                var result = await _client.PostAsJsonAsync(baseadress + "api/Planilla/Insert", planilla);
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
                var result = await _client.PutAsJsonAsync(baseadress + "api/Planilla/Update", planilla);
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
                var result = await _client.PostAsJsonAsync(baseadress + "api/Planilla/Delete", planilla);
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
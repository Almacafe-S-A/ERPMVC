using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
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
    public class TipoDeduccionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;
        public TipoDeduccionController(IOptions<MyConfig> config, ILogger<TipoDeduccionController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        //[Authorize(Policy = "RRHH.Tipo Deduccion")]
        public ActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public ActionResult VistaDetalle()
        {
            ViewData["Editar"] = 1;
            return PartialView("pvwTipoDeduccion", new DeduccionDTO());
        }

        public async Task<ActionResult> EditarDeduccion(long DeductionId)
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Deduction/GetDeductionById/" + DeductionId);
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<DeduccionDTO>(contenido);
                ViewData["Editar"] = 1;
                return PartialView("pvwTipoDeduccion", resultado);
            }

            return BadRequest();
        }

        public async Task<ActionResult> VerDeduccion(long DeductionId)
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Deduction/GetDeductionById/" + DeductionId);
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<DeduccionDTO>(contenido);
                ViewData["Editar"] = 0;
                return PartialView("pvwTipoDeduccion", resultado);
            }

            return BadRequest();
        }

        public async Task<ActionResult<List<DeduccionDTO>>> GetDeducciones()
        {
            try
            {
                string direccionBase = config.Value.urlbase;
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var respuesta = await cliente.GetAsync(direccionBase + "api/Deduction/GetDeduction");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<DeduccionDTO>>(contenido);
                    return Ok(resultado);
                }
                else
                {
                    return Ok(new List<DeduccionDTO>());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                return BadRequest("Error al cargar tipos de deducciones");
            }
        }

        public async Task<ActionResult<List<DeduccionDTO>>> GetDeduccionesSinLey()
        {
            try
            {
                string direccionBase = config.Value.urlbase;
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var respuesta = await cliente.GetAsync(direccionBase + "api/Deduction/GetDeduction");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<DeduccionDTO>>(contenido);
                    resultado = resultado.Where(r => r.DeductionId > 4).ToList();
                    return Ok(resultado);
                }
                else
                {
                    return Ok(new List<DeduccionDTO>());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                return BadRequest("Error al cargar tipos de deducciones");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GuardarDeduccion([FromBody] DeduccionDTO deduccion)
        {
            if (ModelState.IsValid)
            {
                deduccion.DeductionType = deduccion.DeductionTypeId == 1 ? "Por Ley" : deduccion.DeductionTypeId == 2 ? "Eventual" : deduccion.DeductionTypeId == 3 ? "Colegiación" : "Fondo Pensión / AFP";

                HttpResponseMessage respuesta;
                if (deduccion.DeductionId == 0)
                {
                    respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Deduction/Insert", deduccion);
                }
                else
                {
                    respuesta = await Utils.HttpPutAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Deduction/Update", deduccion);
                }
                
                if (respuesta.IsSuccessStatusCode)
                {
                    ViewData["Editar"] = 1;
                    return PartialView("pvwTipoDeduccion", deduccion);
                }
                else
                {
                    return BadRequest();
                }
                
            }
            else
            {
                throw new Exception("Campos no validos en tipo de deducción");
            }
        }

        public async Task<ActionResult> GetTipoDeduccionPorNombre(string nombre)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/Deduction/GetDeductionByDescription/" + nombre);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<DeduccionDTO>(contenido);
                    return Ok(resultado);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar tipo deducción por nombre");
                return BadRequest();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
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
    public class LlegadaTardeController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;

        public LlegadaTardeController(IOptions<MyConfig> config, ILogger<LlegadaTardeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostFecha([FromForm] string Fecha, bool Todos)
        {
            try
            {
                DateTime fecha = DateTime.ParseExact(Fecha,"dd/MM/yyyy",CultureInfo.InvariantCulture);
                TempData["Fecha"] = fecha;
                TempData["Todos"] = Todos;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error con formato de fecha");
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetLlegadasTardeDisponibles(DateTime fecha, bool todos)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/LlegadasTarde/GetLlegadasTardesDisponibles/{fecha.ToString("yyyy-MM-dd")}/"+(todos?1:0));
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<LlegadaTarde>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar las llegadas tarde");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AprobarLlegadaTarde(long idLlegadaTarde)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/LlegadasTarde/AprobarLlegadaTarde/{idLlegadaTarde}",null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al aprobar la llegada tarde");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarLlegadaTarde(long idLlegadaTarde)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/LlegadasTarde/RechazarLlegadaTarde/{idLlegadaTarde}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al rechazar la llegada tarde");
                return BadRequest(ex.Message);
            }
        }

    }
}
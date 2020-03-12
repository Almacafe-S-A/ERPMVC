using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class InasistenciaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;
        public InasistenciaController(IOptions<MyConfig> config, ILogger<InasistenciaController> logger, IHttpContextAccessor httpContextAccessor)
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
        public async Task<IActionResult> PostFecha([FromForm] string Fecha)
        {
            try
            {
                DateTime fecha = DateTime.ParseExact(Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                TempData["Fecha"] = fecha;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error con formato de fecha");
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetInasistenciasFecha(DateTime fecha)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Inasistencia/GetInasistenciasFecha/{fecha.ToString("yyyy-MM-dd")}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<Inasistencia>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar las inasistencias");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AprobarInasistencia(long idInasistencia, long idTipo, string comentario)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Inasistencia/Aprobar/{idInasistencia}/{idTipo}/{(comentario??"")}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Inasistencia>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al aprobar las inasistencias");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarInasistencia(long idInasistencia, long idTipo, string comentario)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Inasistencia/Rechazar/{idInasistencia}/{comentario}/{idTipo}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Inasistencia>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al aprobar las inasistencias");
                return BadRequest(ex.Message);
            }
        }
    }
}
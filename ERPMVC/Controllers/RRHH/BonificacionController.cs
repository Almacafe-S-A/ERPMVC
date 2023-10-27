using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BonificacionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public BonificacionController(IOptions<MyConfig> config, ILogger<BonificacionController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        //[Authorize(Policy = "RRHH.Bonos y Deducciones.Bonificaciones")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostFiltro([FromForm] int Periodo, int Mes, bool Inactivo)
        {
            try
            {
                TempData["Periodo"] = Periodo;
                TempData["Mes"] = Mes;
                TempData["Inactivo"] = Inactivo;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error con formato de fecha");
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> GetBonificacionesMesPeriodo(int Periodo, int Mes, bool inactivos)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Bonificacion/GetBonificacionesMesPeriodo/{Periodo}/{Mes}/{inactivos}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<Bonificacion>>(contenido);
                    return Ok(resultado);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar las bonificaciones.");
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> Guardar(Bonificacion registro)
        {
            try
            {
                if (registro.Id == 0)
                {
                    registro.UsuarioCreacion = HttpContext.Session.GetString("user");
                    registro.UsuarioModificacion = registro.UsuarioCreacion;
                    registro.FechaCreacion = DateTime.Now;
                    registro.FechaModificacion = registro.FechaCreacion;
                }
                else
                {
                    registro.UsuarioModificacion = HttpContext.Session.GetString("user");
                    registro.FechaModificacion = DateTime.Now;
                }

                if (registro.EmpleadoId == 0)
                    throw new Exception("Debe seleccionar a un empleado.");

                if (registro.Monto <= 0)
                    throw new Exception("Monto de bonificación es invalido.");

                if (registro.TipoId == 0)
                    throw new Exception("Debe seleccionar un tipo de bonificación");

                if (registro.TipoId == 1)
                    throw new Exception(
                        "El bono educativo no puede ser otorgado de forma manual, debe utilizar la opción del sistema designada para este efecto.");

                registro.Estado = null;
                registro.Empleado = null;
                registro.Tipo = null;

                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/Bonificacion/Guardar", registro);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Bonificacion>(contenido);
                    registro.Id = resultado.Id;
                    return Ok(registro);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al guardar una bonificación");
                return BadRequest(ex.Message);
            }
        }
    }
}
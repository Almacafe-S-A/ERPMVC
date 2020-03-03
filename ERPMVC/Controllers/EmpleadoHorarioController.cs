using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
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
    public class EmpleadoHorarioController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public EmpleadoHorarioController(IOptions<MyConfig> config, ILogger<EmpleadoHorarioController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, EmpleadoHorario registro)
        {
            try
            {
                registro.Empleado = null;
                registro.HorarioEmpleado = null;
                registro.Estado = null;
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
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/EmpleadoHorario/Guardar", registro);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<EmpleadoHorario>(contenido);
                    registro.Id = resultado.Id;
                    return Ok(new []{resultado}.ToDataSourceResult(request));
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar horario de empleado");
                return BadRequest();
            }
        }

        public async Task<ActionResult> GetHorariosEmpleados()
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/EmpleadoHorario/GetHorariosEmpleados");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<EmpleadoHorario>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar los horarios del empleado");
                return BadRequest();
            }
        }
    }
}
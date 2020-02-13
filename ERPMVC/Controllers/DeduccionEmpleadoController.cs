using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class DeduccionEmpleadoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public DeduccionEmpleadoController(IOptions<MyConfig> config, ILogger<DeduccionEmpleadoController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditarDeducciones(long codigoEmpleado, string nombreEmpleado, double salarioEmpleado)
        {
            ViewData["Editar"] = 1;
            ViewData["CodigoEmpleado"] = codigoEmpleado;
            ViewData["NombreEmpleado"] = nombreEmpleado;
            ViewData["SalarioEmpleado"] = salarioEmpleado;
            return PartialView("pvwAgregarDeduccionEmpleado");
        }

        public ActionResult VerDeducciones(long codigoEmpleado, string nombreEmpleado, double salarioEmpleado)
        {
            ViewData["Editar"] = 0;
            ViewData["CodigoEmpleado"] = codigoEmpleado;
            ViewData["NombreEmpleado"] = nombreEmpleado;
            ViewData["SalarioEmpleado"] = salarioEmpleado;
            return PartialView("pvwAgregarDeduccionEmpleado");
        }

        public async Task<ActionResult<List<DeduccionesEmpleadoDTO>>> GetEmpleadosDeducciones()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/DeduccionEmpleado/GetDeduccionesEmpleados");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<DeduccionesEmpleadoDTO>>(contenido);
                return Ok(resultado);
            }

            return BadRequest();
        }

        public async Task<ActionResult> GetDeduccionesEmpleado(long empleadoId)
        //[DataSourceRequest] DataSourceRequest request, long empleadoId)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/DeduccionEmpleado/GetDeduccionesPorEmpleado/"+empleadoId);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<DeduccionEmpleado>>(contenido);
                    if (resultado.Count == 0)
                    {
                        return Ok();
                    }

                    return Ok(resultado);

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar deducciones del empleado");
                return BadRequest();
            }
        }

        public async Task<ActionResult> GuardarDeduccionEmpleado([DataSourceRequest] DataSourceRequest request, DeduccionEmpleado deduccionGuardar)
        {
            try
            {
                if (deduccionGuardar.Id == 0)
                {
                    deduccionGuardar.UsuarioCreacion = HttpContext.Session.GetString("user");
                    deduccionGuardar.UsuarioModificacion = deduccionGuardar.UsuarioCreacion;
                    deduccionGuardar.FechaCreacion = DateTime.Now;
                    deduccionGuardar.FechaModificacion = deduccionGuardar.FechaCreacion;
                }
                else
                {
                    deduccionGuardar.UsuarioModificacion = HttpContext.Session.GetString("user");
                    deduccionGuardar.FechaModificacion = DateTime.Now;
                }
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/DeduccionEmpleado/Guardar", deduccionGuardar);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<DeduccionEmpleado>(contenido);
                    return Json(new object());
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar deduccion empleado");
                return BadRequest();
            }
        }
    }
}

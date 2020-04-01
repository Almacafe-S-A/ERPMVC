using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ERPMVC.DTO;
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

        public ActionResult CalculoGeneralISR()
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


        public async Task<ActionResult> CalcularISR(long empleadoId)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/DeduccionEmpleado/GetISREmpleado/" + empleadoId);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<decimal>(contenido);
                    return Ok(resultado);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al calcular el ISR del empleado");
                return BadRequest();
            }
        }

        public async Task<ActionResult> GuardarDeduccionEmpleado([DataSourceRequest] DataSourceRequest request, DeduccionEmpleado deduccionGuardar)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                deduccionGuardar.Deduccion = null;
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
                        return Json(new[] { resultado }.ToDataSourceResult(request, ModelState));
                    }
               // }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar deduccion empleado");
                return BadRequest();
            }
        }


        public async Task<ActionResult> CalcularISRGeneral(int periodo, int mes)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/DeduccionEmpleado/CalcularISRGeneral/{periodo}/{mes}/{HttpContext.Session.GetString("user")}",null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex,$"Error al calcular el ISR para los empleados en periodo {periodo} y mes {mes}");
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> GetPagosISRPeriodo(int periodo, int mes)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/DeduccionEmpleado/GetPagosISRPeriodo/{periodo}/{mes}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<PagosISRDTO>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al cargar el ISR para los empleados en periodo {periodo} y mes {mes}");
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> CalculoISR(PeriodoDTO pPeriodo)
        {
            try
            {

                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/DeduccionEmpleado/CalcularISRGeneral/{pPeriodo.Periodo}/{pPeriodo.Mes}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<PagosISRDTO>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error al cargar el ISR para los empleados en periodo {pPeriodo.Periodo} y mes {pPeriodo.Mes}");
                return BadRequest(ex.Message);
            }
        }




    }
}

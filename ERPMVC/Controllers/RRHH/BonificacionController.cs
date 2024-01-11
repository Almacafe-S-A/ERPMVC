using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            Bonificacion bonificacion= new Bonificacion();
            bonificacion.Periodo = Utils.Periodo;
            bonificacion.PeriodoId = Utils.PeriodoActualId;
            bonificacion.Mes = DateTime.Now.Month;
            return View(bonificacion);
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

        public async Task<DataSourceResult> GetBonificacionesMesPeriodo([DataSourceRequest] DataSourceRequest request, BonificacionDTO bonificacion)
        {
            List<Bonificacion> bonificaciones = new List<Bonificacion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Bonificacion/GetBonificacionesMesPeriodo/{bonificacion.IdPeriodo}/{bonificacion.NoMes}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    bonificaciones = JsonConvert.DeserializeObject<List<Bonificacion>>(valorrespuesta);
                    bonificaciones = bonificaciones.OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return bonificaciones.ToDataSourceResult(request);
        }

        public async Task<ActionResult> GetPorEmpleado([DataSourceRequest] DataSourceRequest request, long empleadoId)
        //[DataSourceRequest] DataSourceRequest request, long empleadoId)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Bonificacion/GetBonificacionesEmpleado/{empleadoId}/{1}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<Bonificacion>>(contenido);
                    if (resultado.Count == 0)
                    {
                        return Ok();
                    }

                    return Ok(resultado.ToDataSourceResult(request));

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar deducciones del empleado");
                return BadRequest();
            }
        }


        public async Task<ActionResult> GetPorEmpleadoPlanilla([DataSourceRequest] DataSourceRequest request,int empleadoId, int mes, int periodoId, int planilladetalleId)
        //[DataSourceRequest] DataSourceRequest request, long empleadoId)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Bonificacion/GetEmpleadoPlanilla/{empleadoId}/{mes}/{periodoId}/{planilladetalleId}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<Bonificacion>>(contenido);
                    if (resultado.Count == 0)
                    {
                        return Ok();
                    }

                    return Ok(resultado.ToDataSourceResult(request));

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar deducciones del empleado");
                return BadRequest();
            }
        }

        public async Task<ActionResult> Guardar(Bonificacion registro,  int NoMes,  int IdPeriodo, string FechaBono)
        {
            try
            {
                registro.FechaBono = Utils.GetFecha(FechaBono);
                registro.PeriodoId = IdPeriodo;
                registro.Mes = NoMes; 
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

                //if (registro.Empleado.IdEmpleado == 0)
                //    throw new Exception("Debe seleccionar a un empleado.");

                if (registro.Tipo.Id == 0)
                    throw new Exception("Debe seleccionar un tipo de bonificación");

               
                if (registro.Quincena == 1)
                {
                    registro.NombreQuincena = "Primera";
                }
                if (registro.Quincena == 2)
                {
                    registro.NombreQuincena = "Segunda";
                }
                if (registro.Quincena == 3)
                {
                    registro.NombreQuincena = "Ambas";
                }
                //registro.FechaBono = new DateTime(Periodo, Mes, 1);
                //registro.EmpleadoId = registro.Empleado.IdEmpleado;
                registro.TipoId = registro.Tipo.Id;
               
                registro.Monto = registro.Cantidad * registro.Tipo.Valor;
                //registro.EstadoId = registro.Estado.IdEstado;
                //registro.EmpleadoNombre = registro.Empleado.NombreEmpleado;
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

        [HttpPost("[action]")]
        public async Task<ActionResult> AprobarBonificacion(long idBonificacion)
        {
            try
            {
                if (idBonificacion == 0)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Bonificacion/ChangeStatus/{idBonificacion}/{1}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo el documento!"));
                }

                return Ok();

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarBonificacion(long idBonificacion)
        {
            try
            {
                if (idBonificacion == 0)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Bonificacion/ChangeStatus/{idBonificacion}/{2}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Rechazo el documento!"));
                }

                return Ok();

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
        }
    }
}
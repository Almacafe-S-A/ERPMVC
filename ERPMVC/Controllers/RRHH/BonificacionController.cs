﻿using System;
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

        public async Task<DataSourceResult> GetBonificacionesMesPeriodo([DataSourceRequest] DataSourceRequest request, int Periodo, int Mes)
        {
            List<Bonificacion> bonificaciones = new List<Bonificacion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Bonificacion/GetBonificacionesMesPeriodo/{Periodo}/{Mes}");
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

        public async Task<ActionResult> Guardar(Bonificacion registro, int Periodo, int Mes)
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

                if (registro.Empleado.IdEmpleado == 0)
                    throw new Exception("Debe seleccionar a un empleado.");

                if (registro.Cantidad <= 0)
                    throw new Exception("Cantidad de bonificación es invalido.");

                if (registro.Tipo.Id == 0)
                    throw new Exception("Debe seleccionar un tipo de bonificación");

                if (registro.Tipo.Id == 1)
                    throw new Exception(
                        "El bono educativo no puede ser otorgado de forma manual, debe utilizar la opción del sistema designada para este efecto.");
                if (registro.NombreQuincena == "1")
                {
                    registro.NombreQuincena = "Primera";
                }
                if (registro.NombreQuincena == "2")
                {
                    registro.NombreQuincena = "Segunda";
                }
                if (registro.NombreQuincena == "3")
                {
                    registro.NombreQuincena = "Ambas";
                }
                registro.FechaBono = new DateTime(Periodo, Mes, 1);
                registro.EmpleadoId = registro.Empleado.IdEmpleado;
                registro.TipoId = registro.Tipo.Id;
                registro.Monto = registro.Cantidad * registro.Tipo.Valor;
                registro.EstadoId = registro.Estado.IdEstado;
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
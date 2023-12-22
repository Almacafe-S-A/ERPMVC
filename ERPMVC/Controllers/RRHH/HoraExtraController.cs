using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;

namespace ERPMVC.Controllers
{
    public class HoraExtraController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;
        public HoraExtraController(IOptions<MyConfig> config, ILogger<HoraExtraController> logger, IHttpContextAccessor httpContextAccessor)
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

    

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetHorasExtra([DataSourceRequest] DataSourceRequest request, DateTime Fecha, bool Todos)
        {
            List<HoraExtra> horasExtra = new List<HoraExtra>();
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/GetHorasExtrasFecha/{Fecha.ToString("yyyy-MM-dd")}/" + (Todos ? 1 : 0));
                string valorrespuesta = "";
                if (respuesta.IsSuccessStatusCode)
                {
                    valorrespuesta = await respuesta.Content.ReadAsStringAsync();
                    horasExtra = JsonConvert.DeserializeObject<List<HoraExtra>>(valorrespuesta);
                    horasExtra = horasExtra.OrderByDescending(x => x.Id).ToList();

                    // Calcular y asignar las horas extras correctamente
                    foreach (var horaExtra in horasExtra)
                    {
                        double horasExtras = horaExtra.Horas + (horaExtra.Minutos / 60.0);
                                horasExtras = Math.Round(horasExtras, 2, MidpointRounding.AwayFromZero);

                        TimeSpan totalhorasextras = new TimeSpan(horaExtra.Horas+ horaExtra.HoraAlumerzo, horaExtra.Minutos, 0);

                        
                        horaExtra.HorasExtras = Math.Round(totalhorasextras.TotalHours, 2, MidpointRounding.AwayFromZero)  ;

                        if (horaExtra.HoraEntrada == null || horaExtra.HoraSalida == null)
                        {
                            continue;
                        }
                        // Convertir las cadenas de tiempo a objetos DateTime
                        DateTime horaEntrada = DateTime.ParseExact(horaExtra.HoraEntrada, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime horaSalida =  DateTime.ParseExact(horaExtra.HoraSalida, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        if (horaSalida < horaEntrada)
                        {
                            // Si la hora de salida es menor, significa que el empleado ha salido al día siguiente
                            horaSalida = horaSalida.AddDays(1);
                        }
                        // Calcular la diferencia entre las horas
                        TimeSpan diferenciaHoras = horaSalida - horaEntrada;

                        // La diferencia estará representada como un TimeSpan, puedes obtener los valores específicos si lo necesitas
                        horaExtra.HorasExtrasBiometrico = diferenciaHoras.TotalHours;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar las horas extra");
                throw ex;
            }
            return horasExtra.ToDataSourceResult(request);
        }

        public async Task<DataSourceResult> GetHorasExtraDistribucion([DataSourceRequest] DataSourceRequest request,int IdHoraExtra)
        {
            List<HorasExtrasDistribucion> horasExtrasDistribucions = new List<HorasExtrasDistribucion>();
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/GetDistribucionByHoraExtraId/{IdHoraExtra}");
                string valorrespuesta = "";
                if (respuesta.IsSuccessStatusCode)
                {
                    valorrespuesta = await respuesta.Content.ReadAsStringAsync();
                    horasExtrasDistribucions = JsonConvert.DeserializeObject<List<HorasExtrasDistribucion>>(valorrespuesta);
                    horasExtrasDistribucions = horasExtrasDistribucions.OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar la distribucion hora extra");
                throw ex;
            }
            return horasExtrasDistribucions.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Revisar(long idHoraExtra)
        {
            try
            {
                if (idHoraExtra == 0)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/HorasExtra/ChangeStatus/{idHoraExtra}/{1}");
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
        public async Task<ActionResult> Aprobar(long idHoraExtra)
        {
            try
            {
                if (idHoraExtra == 0)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/HorasExtra/ChangeStatus/{idHoraExtra}/{2}");
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
        public async Task<ActionResult> Rechazar(long idHoraExtra)
        {
            try
            {
                if (idHoraExtra == 0)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/HorasExtra/ChangeStatus/{idHoraExtra}/{3}");
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
        public async Task<ActionResult> AprobarHorasExtra(long idHoraExtra)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/AprobarHoraExtra/{idHoraExtra}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al aprobar la Hora Extra");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarHoraExtra(long idHoraExtra)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/RechazarHoraExtra/{idHoraExtra}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al rechazar la Hora Extra");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<HoraExtra>> Update(HoraExtra _horaextra)
        {
            try
            {
                // TODO: Add insert logic here
                string baseAddress = config.Value.urlbase;
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await httpClient.PutAsJsonAsync(baseAddress + "api/HorasExtra/Update", _horaextra);
                string valorRespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    _horaextra = JsonConvert.DeserializeObject<HoraExtra>(valorRespuesta);
                }
                else
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    return BadRequest(valorRespuesta);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrió un error: {ex.ToString()}");
                return await Task.Run(() => BadRequest($"Ocurrió un error: {ex.Message}"));
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _horaextra }, Total = 1 });
        }

        public IActionResult fechaexcel(string contentType, string base64, string fileName, string fechaSeleccionada)
        {
            if (DateTime.TryParseExact(fechaSeleccionada, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                fileName = "HoraExtraReport_" + fecha.ToString("dd/M/yyyy") + ".xlsx";

                var fileContents = Convert.FromBase64String(base64);
                return File(fileContents, contentType, fileName);
            }
            else
            {
                return BadRequest("Fecha seleccionada no válida");
            }
        }

    }
}
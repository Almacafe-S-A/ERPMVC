using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Security.Claims;
using Kendo.Mvc.UI;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class BiometricoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        private readonly ClaimsPrincipal _principal;
        public BiometricoController(IOptions<MyConfig> config, ILogger<BiometricoController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this.logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        //[Authorize(Policy = "RRHH.Asistencia.Cargar Archivo Biometrico")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        public IActionResult VerDetalle(long idBiometrico)
        {
            ViewData["IdBiometrico"] = idBiometrico;
            return PartialView("pvwDetalle");
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetBiometricos([DataSourceRequest] DataSourceRequest request)
        {
            List<Biometrico> biometricos = new List<Biometrico>();

            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/Biometrico/GetBiometricos");
                string valorrespuesta = "";
                if (respuesta.IsSuccessStatusCode)
                {
                    valorrespuesta = await respuesta.Content.ReadAsStringAsync();
                    biometricos = JsonConvert.DeserializeObject<List<Biometrico>>(valorrespuesta);
                    biometricos = biometricos.OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar los archivos biometricos");
                throw ex;
            }
            return biometricos.ToDataSourceResult(request);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetDetalleBiometrico(long IdBiometrico)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Biometrico/GetDetalleBiometrico/{IdBiometrico}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<DetalleBiometrico>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar el detalle del archivo biometrico ");
                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> GuardarBiometrico([FromForm]BiometricoPost registro)
        {
            try
            {
                if (registro.Archivo == null)
                {
                    throw new Exception("No hay archivo que procesar");
                }

                if (registro.Archivo.Count() == 0)
                {
                    throw new Exception("No hay archivo que procesar");
                }

                var archivo = registro.Archivo.First();
                var metaArchivo = ContentDispositionHeaderValue.Parse(archivo.ContentDisposition);
                var nombreArchivo = Path.GetFileName(metaArchivo.FileName.ToString().Trim('"'));

                if (!nombreArchivo.Contains(".xlsx"))
                {
                    throw new Exception("No es un archivo de Excel");
                }

                XSSFWorkbook libro = new XSSFWorkbook(archivo.OpenReadStream());
                var hoja = libro.GetSheetAt(0);
                var titulos = hoja.GetRow(0);
                //validar titulos
                if (!(titulos.GetCell(0).StringCellValue.ToUpper().Equals("CODIGO") &&
                      titulos.GetCell(1).StringCellValue.ToUpper().Equals("FECHA") &&
                      titulos.GetCell(2).StringCellValue.ToUpper().Equals("HORA") &&
                      titulos.GetCell(3).StringCellValue.ToUpper().Equals("TIPO")))
                {
                    libro.Close();
                    throw new Exception("Titulos de hoja de excel no son validos");
                }

                Biometrico biometrico = new Biometrico()
                                        {
                                            Id = 0,
                                            Fecha = registro.Fecha,
                                            IdEstado = 60,
                                            Detalle = new List<DetalleBiometrico>(),
                                            Observacion = registro.Observacion
                                        };


                for (int fila = 1; fila <= hoja.LastRowNum; fila++)
                {
                    var filaRegistro = hoja.GetRow(fila);

                    var IdBiometrico = Utils.GetNumeroXLS(filaRegistro.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK));
                    var fecha = Utils.GetFechaXLS(filaRegistro.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK));
                    var hora = Utils.GetHoraXLS(filaRegistro.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK));
                    var tipo = filaRegistro.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();

                    if (fecha.Equals(DateTime.MinValue)) {
                        TempData["Errores"] = "Formato de Fecha No Valido";
                        return RedirectToAction("Index");
                        continue;

                    }

                    if (hora.Equals(DateTime.MinValue)) { 
                        TempData["Errores"] = "Formato de Fecha No Valido";
                        return RedirectToAction("Index");
                        continue;
                    }

                    if (IdBiometrico == null) { 
                        TempData["Errores"] = "Formato de Fecha No Valido";
                        return RedirectToAction("Index");
                        continue; 
                    }

                    var fechaHora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, 0);

                    DetalleBiometrico detalle = new DetalleBiometrico()
                                                {
                                                    Encabezado = biometrico,
                                                    FechaHora = fechaHora,
                                                    IdBiometrico = (long)IdBiometrico.Value,
                                                    Tipo = tipo
                                                };
                    biometrico.Detalle.Add(detalle);
                }
                libro.Close();

                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/Biometrico/Guardar", biometrico);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else {
                   TempData["Errores"] = await respuesta.Content.ReadAsStringAsync();

                }

                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al guardar el registro biometrico");
                TempData["Errores"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AprobarBiometrico(long IdBiometrico)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Biometrico/AprobarBiometrico/{IdBiometrico}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return BadRequest(contenido);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Ocurrio un erro al aprobar el archivo biometrico.");
                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarBiometrico(long IdBiometrico)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/Biometrico/RechazarBiometrico/{IdBiometrico}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return BadRequest(contenido);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrio un erro al rechazar el archivo biometrico.");
                return BadRequest(ex);
            }
        }
    }
}
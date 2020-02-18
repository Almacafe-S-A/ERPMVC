using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    public class BiometricoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public BiometricoController(IOptions<MyConfig> config, ILogger<BiometricoController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
                      titulos.GetCell(1).StringCellValue.ToUpper().Equals("FECHA_HORA") &&
                      titulos.GetCell(2).StringCellValue.ToUpper().Equals("TIPO")))
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
                    
                    var IdBiometrico = filaRegistro.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                    var fechaHora = filaRegistro.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                    var tipo = filaRegistro.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
                    DetalleBiometrico detalle = new DetalleBiometrico()
                                                {
                                                    Encabezado = biometrico,
                                                    FechaHora = DateTime.ParseExact(fechaHora,"dd/MM/yyyy hh:mm tt",null),
                                                    IdBiometrico = long.Parse(IdBiometrico),
                                                    Tipo = tipo
                                                };
                    biometrico.Detalle.Add(detalle);
                }
                libro.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al guardar el registro biometrico");
                return BadRequest(ex);
            }
        }
    }
}
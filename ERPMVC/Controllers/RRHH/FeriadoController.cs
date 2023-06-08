using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class FeriadoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public FeriadoController(IOptions<MyConfig> config, ILogger<FeriadoController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Feriado()
        {
            return View();
        }
        
        public async Task<ActionResult> GetFeriados()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Feriado/GetFeriados");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Feriado>>(contenido);
                return Ok(resultado);
            }
            return BadRequest();
        }


        [HttpGet]
        public async Task<DataSourceResult> GetFeriadosByPeriodo([DataSourceRequest] DataSourceRequest request, int PeriodoId)
        {
            List<Feriado> _Feriado = new List<Feriado>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Feriado/GetFeriadosByPeriodo/{PeriodoId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Feriado = JsonConvert.DeserializeObject<List<Feriado>>(valorrespuesta);
                    _Feriado = _Feriado.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _Feriado.ToDataSourceResult(request);

        }

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, Feriado feriado)
        {
            try
            {

                    if (feriado.Id == 0)
                    {
                        feriado.UsuarioCreacion = HttpContext.Session.GetString("user");
                        feriado.UsuarioModificacion = feriado.UsuarioCreacion;
                        feriado.FechaCreacion = DateTime.Now;
                        feriado.FechaModificacion = feriado.FechaCreacion;
                    }
                    else
                    {
                        feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                        feriado.FechaModificacion = DateTime.Now;
                    }
                    var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Feriado/Guardar", feriado);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<Feriado>(contenido);
                        feriado.Id = resultado.Id;
                        return Json(new[] { resultado }.ToDataSourceResult(request, ModelState));
                    }
                    else
                    {
                        return BadRequest(respuesta.Content.ReadAsStringAsync());
                    }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al guardar feriado.");
                return BadRequest();
            }
        }

        public async Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, Feriado feriado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (feriado.Id == 0)
                    {
                        feriado.UsuarioCreacion = HttpContext.Session.GetString("user");
                        feriado.UsuarioModificacion = feriado.UsuarioCreacion;
                        feriado.FechaCreacion = DateTime.Now;
                        feriado.FechaModificacion = feriado.FechaCreacion;
                    }
                    else
                    {
                        feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                        feriado.FechaModificacion = DateTime.Now;
                    }
                    var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Feriado/Delete", feriado);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<Feriado>(contenido);
                        feriado.Id = resultado.Id;
                        return Json(new[] { resultado }.ToDataSourceResult(request, ModelState));
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar configuración del ISR");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Feriado>> Update(FeriadoDTO _feriado)
        {
            try
            {
                // TODO: Add insert logic here
                string baseAddress = config.Value.urlbase;
                HttpClient httpClient = new HttpClient();
                _feriado.FechaCreacion = DateTime.Now;
                _feriado.UsuarioCreacion = HttpContext.Session.GetString("user");
                _feriado.FechaModificacion = DateTime.Now;
                _feriado.UsuarioModificacion = HttpContext.Session.GetString("user");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await httpClient.PutAsJsonAsync(baseAddress + "api/Feriado/Update", _feriado);
                string valorRespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    _feriado = JsonConvert.DeserializeObject<FeriadoDTO>(valorRespuesta);
                }
                else
                {
                    valorRespuesta = await result.Content.ReadAsStringAsync();
                    return BadRequest("Este feriado ya ha sido modificado para el periodo seleccionado");
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrió un error: {ex.ToString()}");
                return await Task.Run(() => BadRequest($"Ocurrió un error: {ex.Message}"));
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _feriado }, Total = 1 });
        }

    }
}
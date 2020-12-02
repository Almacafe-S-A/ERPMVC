using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, Feriado feriado)
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
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar configuración del ISR");
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
    }
}
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
    public class ISRController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public ISRController(IOptions<MyConfig> config, ILogger<ISRController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetConfiguracion()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/ISR/GetISRConfiguracion");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<ISR>>(contenido);
                return Ok(resultado);
            }
            
            return BadRequest();
        }

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, ISR configuracion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (configuracion.Id == 0)
                    {
                        configuracion.UsuarioCreacion = HttpContext.Session.GetString("user");
                        configuracion.UsuarioModificacion = configuracion.UsuarioCreacion;
                        configuracion.FechaCreacion = DateTime.Now;
                        configuracion.FechaModificacion = configuracion.FechaCreacion;
                    }
                    else
                    {
                        configuracion.UsuarioModificacion = HttpContext.Session.GetString("user");
                        configuracion.FechaModificacion = DateTime.Now;
                    }
                    var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/ISR/Guardar", configuracion);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<ISR>(contenido);
                        configuracion.Id = resultado.Id;
                        return Json(new[]{resultado}.ToDataSourceResult(request,ModelState));
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

        public async Task<ActionResult> Borrar([DataSourceRequest] DataSourceRequest request, ISR configuracion)
        {
            try
            {
                if (configuracion.Id == 0)
                {
                    return Ok();
                }

                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/ISR/Borrar/"+configuracion.Id, null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Json(new object());
                }


                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al borrar configuración del ISR");
                return BadRequest();
            }
        }
    }
}
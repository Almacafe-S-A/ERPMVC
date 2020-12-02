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
    public class HorarioController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public HorarioController(IOptions<MyConfig> config, ILogger<HorarioController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetHorarios()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/Horario/GetHorarios");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Horario>>(contenido);
                return Ok(resultado);
            }
            return BadRequest();
        }

        public async Task<ActionResult> Guardar([DataSourceRequest] DataSourceRequest request, Horario horario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (horario.Id == 0)
                    {
                        horario.UsuarioCreacion = HttpContext.Session.GetString("user");
                        horario.UsuarioModificacion = horario.UsuarioCreacion;
                        horario.FechaCreacion = DateTime.Now;
                        horario.FechaModificacion = horario.FechaCreacion;
                    }
                    else
                    {
                        horario.UsuarioModificacion = HttpContext.Session.GetString("user");
                        horario.FechaModificacion = DateTime.Now;
                    }
                    var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                        config.Value.urlbase + "api/Horario/Guardar", horario);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var contenido = await respuesta.Content.ReadAsStringAsync();
                        var resultado = JsonConvert.DeserializeObject<Feriado>(contenido);
                        horario.Id = resultado.Id;
                        return Json(new[] { resultado }.ToDataSourceResult(request, ModelState));
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar horario");
                return BadRequest();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    public class EmpleadoBiometricoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;
        public EmpleadoBiometricoController(IOptions<MyConfig> config, ILogger<EmpleadoBiometricoController> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<ActionResult> GetAsignacionesBiometrico()
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/EmpleadoBiometrico/GetAsignacionesBiometrico");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<EmpleadoBiometrico>>(contenido);
                    return Ok(resultado);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar asignaciones biometricas.");
                return BadRequest(ex);
            }
        }

        public async Task<ActionResult> Guardar(EmpleadoBiometrico asignacion)
        {
            try
            {
                if (!asignacion.BiometricoId.HasValue)
                    return BadRequest();

                if (asignacion.BiometricoId.Value == 0)
                    return BadRequest();

                asignacion.UsuarioModificacion = HttpContext.Session.GetString("user");

                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/EmpleadoBiometrico/Guardar", asignacion);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<EmpleadoBiometrico>(contenido);
                    return Ok(resultado);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al guardar asignaciones biometricas.");
                return BadRequest(ex);
            }
        }
    }
}
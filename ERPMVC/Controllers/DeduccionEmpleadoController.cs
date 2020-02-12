using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
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
    public class DeduccionEmpleadoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public DeduccionEmpleadoController(IOptions<MyConfig> config, ILogger<DeduccionEmpleadoController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<List<DeduccionesEmpleadoDTO>>> GetEmpleadosDeducciones()
        {
            var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                config.Value.urlbase + "api/DeduccionEmpleado/GetDeduccionesEmpleados");
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<DeduccionesEmpleadoDTO>>(contenido);
                return Ok(resultado);
            }

            return BadRequest();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
using ERPMVC.Helpers;
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
    public class TipoDeduccionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public TipoDeduccionController(IOptions<MyConfig> config, ILogger<TipoDeduccionController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VistaDetalle()
        {
            return PartialView("pvwTipoDeduccion");
        }

        public async Task<ActionResult<DeduccionDTO>> GetDeducciones()
        {
            try
            {
                string direccionBase = config.Value.urlbase;
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var respuesta = await cliente.GetAsync(direccionBase + "api/Deduction/GetDeduction");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<DeduccionDTO>>(contenido);
                    return Ok(resultado);
                }
                else
                {
                    return Ok(new List<DeduccionDTO>());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                return BadRequest("Error al cargar tipos de deducciones");
            }
        }
    }
}
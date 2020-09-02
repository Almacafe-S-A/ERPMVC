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
    public class CategoriaPlanillaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public CategoriaPlanillaController(IOptions<MyConfig> config, ILogger<CategoriaPlanillaController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<ActionResult> GetCategorias()
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/CategoriaPlanilla/GetCategorias");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<CategoriaPlanilla>>(contenido);
                    return Ok(resultado);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al cargar las categorias de planillas");
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult> GetCategoriasDS([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/CategoriaPlanilla/GetCategorias");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<CategoriaPlanilla>>(contenido);
                    return Ok(resultado.ToDataSourceResult(request));
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar las categorias de planillas");
                return BadRequest(ex.Message);
            }
        }
    }
}
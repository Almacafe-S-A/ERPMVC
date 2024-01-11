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
    public class TipoBonificacionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;

        public TipoBonificacionController(IOptions<MyConfig> config, ILogger<TipoBonificacionController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetTiposBonificaciones()
        {
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/TipoBonificacion/GetTiposBonificacion");
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<TipoBonificacion>>(contenido);
                    return Ok(resultado);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrio un error al cargar los tipos de bonificaciones.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetTiposBonificacionesActivos([DataSourceRequest] DataSourceRequest request)
        {
            List<TipoBonificacion> _tipoBonificacion = new List<TipoBonificacion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoBonificacion/GetTiposBonificacion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _tipoBonificacion = JsonConvert.DeserializeObject<List<TipoBonificacion>>(valorrespuesta).Where(w => w.EstadoId == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }
            return await Task.Run(() => _tipoBonificacion.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Guardar(TipoBonificacion registro)
        {
            try
            {
                if (registro.Id == 0)
                {
                    registro.UsuarioCreacion = HttpContext.Session.GetString("user");
                    registro.UsuarioModificacion = registro.UsuarioCreacion;
                    registro.FechaCreacion = DateTime.Now;
                    registro.FechaModificacion = registro.FechaCreacion;
                }
                else
                {
                    registro.UsuarioModificacion = HttpContext.Session.GetString("user");
                    registro.FechaModificacion = DateTime.Now;
                }

                if (string.IsNullOrEmpty(registro.Nombre))
                {
                    throw new Exception("El nombre es obligatorio.");
                }
                registro.Estado = null;
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + "api/TipoBonificacion/Guardar", registro);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ISR>(contenido);
                    registro.Id = resultado.Id;
                    return Ok(registro);
                }

                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error al guardar un registro de tipo de bonificacion");
                return BadRequest(ex.Message);
            }
        }
    }
}
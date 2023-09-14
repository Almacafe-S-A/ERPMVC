using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
    public class HoraExtraController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger logger;
        private readonly ClaimsPrincipal _principal;
        public HoraExtraController(IOptions<MyConfig> config, ILogger<HoraExtraController> logger, IHttpContextAccessor httpContextAccessor)
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

    

[HttpGet("[action]")]
        public async Task<DataSourceResult> GetHorasExtra([DataSourceRequest] DataSourceRequest request, DateTime Fecha, bool Todos)
        {
            List<HoraExtra> horasExtra = new List<HoraExtra>();
            try
            {
                var respuesta = await Utils.HttpGetAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/GetHorasExtrasFecha/{Fecha.ToString("yyyy-MM-dd")}/" + (Todos ? 1 : 0));
                string valorrespuesta = "";
                if (respuesta.IsSuccessStatusCode)
                {
                    valorrespuesta = await respuesta.Content.ReadAsStringAsync();
                    horasExtra = JsonConvert.DeserializeObject<List<HoraExtra>>(valorrespuesta);
                    horasExtra = horasExtra.OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar las horas extra");
                throw ex;
            }
            return horasExtra.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AprobarHorasExtra(long idHoraExtra)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/AprobarHoraExtra/{idHoraExtra}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al aprobar la Hora Extra");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RechazarHoraExtra(long idHoraExtra)
        {
            try
            {
                var respuesta = await Utils.HttpPostAsync(HttpContext.Session.GetString("token"),
                    config.Value.urlbase + $"api/HorasExtra/RechazarHoraExtra/{idHoraExtra}", null);
                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest(await respuesta.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al rechazar la Hora Extra");
                return BadRequest(ex.Message);
            }
        }
    }
}
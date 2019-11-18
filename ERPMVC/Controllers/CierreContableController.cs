using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CierreContableController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CierreContableController(ILogger<CierreContableController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        public ActionResult CierreContable()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> GetEjecutarCierreContable([FromBody]BitacoraCierreProcesos _Cierrep)
        {
            BitacoraCierreProcesos _Cierre = new BitacoraCierreProcesos();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CierreContable/EjecutarCierreContable", _Cierrep);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cierre = JsonConvert.DeserializeObject<BitacoraCierreProcesos>(valorrespuesta);
                }
                if (_Cierre == null)
                {
                    _Cierre = new BitacoraCierreProcesos();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_Cierre);
        }

    }
}
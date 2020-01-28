﻿using System;
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


        public async Task<IActionResult> CierreContable()
        {
            string baseadress = config.Value.urlbase;
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var resultadoCierre = await cliente.GetAsync(baseadress + "api/CierreContable/UltimoCierre");
            string ultimoCierre = await resultadoCierre.Content.ReadAsStringAsync();
            BitacoraCierreContable cierre = JsonConvert.DeserializeObject<BitacoraCierreContable>(ultimoCierre);
            BitacoraCierreContable NuevoCierre = new BitacoraCierreContable();
            NuevoCierre.FechaCierre = cierre.FechaCierre.AddDays(1);
            return View(NuevoCierre);
        }


        [HttpPost]
        public async Task<ActionResult> GetEjecutarCierreContable([FromBody]BitacoraCierreContable _Cierrep)
        {
            if (_Cierrep.FechaCierre > DateTime.Now)
            {
                return await Task.Run(() => BadRequest($"La fecha no puede ser mayor a la fecha actual"));
            }
            BitacoraCierreContable _Cierre = new BitacoraCierreContable();
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
                    _Cierre = JsonConvert.DeserializeObject<BitacoraCierreContable>(valorrespuesta);
                    ERPMVC.Helpers.Utils.Cerrado = true;
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    return await Task.Run(() => BadRequest($"Error: {error},No se puede aplicar este cierre."));
                }
                if (_Cierre == null)
                {
                    _Cierre = new Models.BitacoraCierreContable();
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
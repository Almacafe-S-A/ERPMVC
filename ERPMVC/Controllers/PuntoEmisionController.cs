﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class PuntoEmisionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public PuntoEmisionController(ILogger<PuntoEmisionController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: PuntoEmision
        public ActionResult Index()
        {
            return View();
        }

        // GET: PuntoEmision
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PuntoEmision> _PuntoEmision = new List<PuntoEmision>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PuntoEmision/GetPuntoEmision");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmision = JsonConvert.DeserializeObject<List<PuntoEmision>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PuntoEmision.ToDataSourceResult(request);

        }

        // POST: PuntoEmision/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(PuntoEmision _PuntoEmisionp)
        {
            PuntoEmision _PuntoEmision = _PuntoEmisionp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PuntoEmision.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PuntoEmision.UsuarioModificacion = HttpContext.Session.GetString("user");
                _PuntoEmision.FechaCreacion = DateTime.Now;
                _PuntoEmision.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/PuntoEmision/Insert", _PuntoEmision);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmision = JsonConvert.DeserializeObject<PuntoEmision>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmision }, Total = 1 });
        }


        // POST: PuntoEmision/Update
        [HttpPost]
        public async Task<IActionResult> Update(PuntoEmision _PuntoEmisionp)
        {
            PuntoEmision _PuntoEmision = _PuntoEmisionp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PuntoEmision.FechaModificacion = DateTime.Now;
                _PuntoEmision.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/PuntoEmision/Update", _PuntoEmision);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmision = JsonConvert.DeserializeObject<PuntoEmision>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmision }, Total = 1 });
        }



        // GET: PuntoEmision/Delete
        [HttpDelete("{IdPuntoEmision}")]
        public async Task<ActionResult<PuntoEmision>> Delete(Int64 IdPuntoEmision, PuntoEmision _PuntoEmisionp)
        {
            PuntoEmision _PuntoEmision = _PuntoEmisionp;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/PuntoEmision/Delete", _PuntoEmision);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PuntoEmision = JsonConvert.DeserializeObject<PuntoEmision>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _PuntoEmision }, Total = 1 });
        }


    }
}
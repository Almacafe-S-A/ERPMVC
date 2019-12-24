﻿using System;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class GarantiaBancariaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GarantiaBancariaController(ILogger<HomeController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        //--------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GarantiaBancaria> _GarantiaBancaria = new List<GarantiaBancaria>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GarantiaBancaria/GetGarantiaBancaria");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancaria = JsonConvert.DeserializeObject<List<GarantiaBancaria>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => _GarantiaBancaria.ToDataSourceResult(request));
        }

        //--------------------------------------------------------------------------------------

        public async Task<ActionResult> pvwAddGarantiaBancaria([FromBody]GarantiaBancariaDTO _sarpara)
        {
            GarantiaBancariaDTO _GarantiaBancaria = new GarantiaBancariaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GarantiaBancaria/GetGarantiaBancariaById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancaria = JsonConvert.DeserializeObject<GarantiaBancariaDTO>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_GarantiaBancaria);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<GarantiaBancariaDTO>> SaveGarantiaBancaria([FromBody]GarantiaBancariaDTO _GarantiaBancariaP)
        {
            GarantiaBancaria _GarantiaBancaria = _GarantiaBancariaP;
            try
            {
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GarantiaBancaria/GetGarantiaBancariaById/" + _GarantiaBancaria.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancaria = JsonConvert.DeserializeObject<GarantiaBancariaDTO>(valorrespuesta);
                }

                if (_GarantiaBancaria == null) { _GarantiaBancaria = new Models.GarantiaBancaria(); }

                if (_GarantiaBancariaP.Id == 0)
                {
                    _GarantiaBancariaP.FechaCreacion = DateTime.Now;
                    _GarantiaBancariaP.FechaModificacion = DateTime.Now;
                    _GarantiaBancariaP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _GarantiaBancariaP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GarantiaBancariaP);
                }
                else
                {
                    _GarantiaBancariaP.FechaCreacion = _GarantiaBancaria.FechaCreacion;
                    _GarantiaBancariaP.UsuarioCreacion = _GarantiaBancaria.UsuarioCreacion;
                    _GarantiaBancariaP.FechaModificacion = DateTime.Now;
                    _GarantiaBancariaP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    var updateresult = await Update(_GarantiaBancaria.Id, _GarantiaBancariaP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_GarantiaBancaria);
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult> Insert(GarantiaBancaria _GarantiaBancariaS)
        {
            GarantiaBancaria _GarantiaBancaria = _GarantiaBancariaS;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/GarantiaBancaria/Insert", _GarantiaBancariaS);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancariaS = JsonConvert.DeserializeObject<GarantiaBancaria>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _GarantiaBancariaS }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, GarantiaBancaria _GarantiaBancariaP)
        {
            GarantiaBancaria _GarantiaBancaria = _GarantiaBancariaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/GarantiaBancaria/Update", _GarantiaBancaria);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancaria = JsonConvert.DeserializeObject<GarantiaBancaria>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _GarantiaBancaria }, Total = 1 });
        }

        //--------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<GarantiaBancaria>> Delete(Int64 Id, [FromBody]GarantiaBancaria _GarantiaBancariaP)
        {
            GarantiaBancaria _GarantiaBancaria = _GarantiaBancariaP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/GarantiaBancaria/Delete", _GarantiaBancaria);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GarantiaBancaria = JsonConvert.DeserializeObject<GarantiaBancaria>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _GarantiaBancaria }, Total = 1 });
        }
    }
}
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
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class TipoContratoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public TipoContratoController(ILogger<TipoContratoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        // GET: Customer
        //[Authorize(Policy = "RRHH.Parametros Tipo Contrato")]
        public ActionResult TipoContrato()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoContrato> _cais = new List<TipoContrato>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContrato");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<TipoContrato>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_cais.ToDataSourceResult(request));

        }
        
        
        [HttpGet]
        public async Task<JsonResult> GetActivos([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoContrato> _cais = new List<TipoContrato>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContrato");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _cais = JsonConvert.DeserializeObject<List<TipoContrato>>(valorrespuesta).Where(w => w.IdEstado==1).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_cais.ToDataSourceResult(request));
        }


        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<JsonResult> GetBOX([DataSourceRequest]DataSourceRequest request)
        {
            List<TipoContrato> _TipoContrato = new List<TipoContrato>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContrato");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<List<TipoContrato>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TipoContrato);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddTipoContrato([FromBody]TipoContratoDTO _sarpara)
        {
            TipoContratoDTO _TipoContrato = new TipoContratoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContratoById/" + _sarpara.IdTipoContrato);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<TipoContratoDTO>(valorrespuesta);

                }

                if (_TipoContrato == null)
                {
                    _TipoContrato = new TipoContratoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TipoContrato);

        }


        [HttpPost]
        public async Task<ActionResult<TipoContrato>> SaveTipoContrato([FromBody]TipoContratoDTO _TipoContratoP)
        {

            TipoContrato _TipoContrato = _TipoContratoP;
            try
            {
                TipoContrato _listTipoContrato = new TipoContrato();
                // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContratoByName/" + _TipoContrato.NombreTipoContrato);
                //var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContratoById/" + _TipoContrato.IdTipoContrato);
                string valorrespuesta1 = "";
                _TipoContrato.FechaModificacion = DateTime.Now;
                _TipoContrato.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result1.IsSuccessStatusCode)
                {
                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<TipoContratoDTO>(valorrespuesta1);
                }

                if (_TipoContrato == null) { _TipoContrato = new Models.TipoContrato(); }

                if (_TipoContrato.IdTipoContrato > 0)
                {
                    if (_TipoContrato.IdTipoContrato != _TipoContratoP.IdTipoContrato)
                    return await Task.Run(() => BadRequest($"Ya existe un Contrato registrado con ese nombre."));
                }
                

                if (_TipoContratoP.IdTipoContrato == 0)
                {
                    _TipoContrato.FechaCreacion = DateTime.Now;
                    _TipoContrato.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TipoContratoP);
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/TipoContrato/GetTipoContratoById/" + _TipoContrato.IdTipoContrato);
                    _TipoContratoP.Usuariocreacion = _TipoContrato.Usuariocreacion;
                    _TipoContratoP.FechaCreacion = _TipoContrato.FechaCreacion;
                    var updateresult = await Update(_TipoContrato.IdTipoContrato, _TipoContratoP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TipoContrato);
        }


        //--------------------------------------------------------------------------------------
        // POST: TipoContrato/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(TipoContrato _TipoContratoP)
        {
            TipoContrato _TipoContrato = _TipoContratoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TipoContrato.Usuariocreacion = HttpContext.Session.GetString("user");
                _TipoContrato.Usuariomodificacion = HttpContext.Session.GetString("user");
                _TipoContrato.FechaCreacion = DateTime.Now;
                _TipoContrato.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoContrato/Insert", _TipoContrato);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<TipoContrato>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoContrato }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Int64 Id, TipoContrato _TipoContratoP)
        {
            TipoContrato _TipoContrato = _TipoContratoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TipoContrato.FechaModificacion = DateTime.Now;
                _TipoContrato.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/TipoContrato/Update", _TipoContrato);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<TipoContrato>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoContrato }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<TipoContrato>> Delete(Int64 Id, TipoContrato _TipoContratoP)
        {
            TipoContrato _TipoContrato = _TipoContratoP;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/TipoContrato/DeleteTipoContrato", _TipoContrato);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TipoContrato = JsonConvert.DeserializeObject<TipoContrato>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TipoContrato }, Total = 1 });
        }






    }
}
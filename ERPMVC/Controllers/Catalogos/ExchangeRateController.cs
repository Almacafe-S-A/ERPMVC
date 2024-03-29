﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
    public class ExchangeRateController : Controller
    {
        private readonly IOptions<MyConfig> config;
       // private readonly IMapper mapper;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public ExchangeRateController(ILogger<ExchangeRateController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }
        // GET: ExchangeRate
        [Authorize(Policy = "Configuracion.Tasa de Cambio")]
        public ActionResult ExchangeRate()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Configuracion.Tasa de Cambio.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Configuracion.Tasa de Cambio.Editar", "true");
            return View();
        }


       

        [HttpPost("[action]")]
        public async Task<JsonResult> GetExchangeRateByDateNCurrency([FromBody] ExchangeRate exchangeRate)
        {
            ExchangeRate _ExchangeRate = new ExchangeRate();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/ExchangeRate/GetExchangeRateByDateNCurrency",exchangeRate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_ExchangeRate);
        }



        [HttpGet("[action]")]
        public async Task<JsonResult> GetExchangeRate([DataSourceRequest] DataSourceRequest request)
        {
            List<ExchangeRate> _ExchangeRate = new List<ExchangeRate>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRate");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<List<ExchangeRate>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_ExchangeRate.ToDataSourceResult(request));
        }

        public async Task<ActionResult<ExchangeRate>> SaveExchangeRate([FromBody]ExchangeRateDTO _ExchangeRateP)
        {
            ExchangeRate _ExchangeRate = _ExchangeRateP;
            if (_ExchangeRateP.DayofRate.Date>DateTime.Now)
            {
                return await Task.Run(() => BadRequest($"La fecha no puede ser Mayor a la Actual"));
            }
            try
            {
                ExchangeRate _listAccount = new ExchangeRate();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                // var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRateById/" + _ExchangeRate.ExchangeRateId);
                if (_ExchangeRate.ExchangeRateId == 0)
                {
                    _ExchangeRate.CreatedUser = HttpContext.Session.GetString("user");
                    _ExchangeRate.ModifiedUser = HttpContext.Session.GetString("user");
                    var resultdate = await _client.PostAsJsonAsync(baseadress + "api/ExchangeRate/GetExchangeRateByFecha" , _ExchangeRate);
                    string valorrespuesta = "";
                    //PRUEBA
                    decimal valor = Convert.ToDecimal(_ExchangeRate.ExchangeRateValue);
                    decimal temp = decimal.Round(valor, 4, MidpointRounding.AwayFromZero);
                    temp = decimal.Parse(temp.ToString("N4"));
                    //string a = String.Format("{0:F2}", temp);                  
                    //temp = decimal.Parse(a); 
                    decimal variable1 = Convert.ToDecimal(temp, System.Globalization.CultureInfo.InvariantCulture);
                    _ExchangeRate.ExchangeRateValue = Convert.ToDecimal(variable1);
                    _ExchangeRate.ModifiedDate = DateTime.Now;
                    _ExchangeRate.ModifiedUser = HttpContext.Session.GetString("user");
                    if (resultdate.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (resultdate.Content.ReadAsStringAsync());
                        _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);

                        if (_ExchangeRate == null)
                        {
                            _ExchangeRate = new Models.ExchangeRate();
                        }

                        if (_ExchangeRate.ExchangeRateId > 0)
                        {
                            return await Task.Run(() => BadRequest($"Ya existe una tasa registrada el día de hoy para esta moneda."));
                        }
                    }
                   
                }

                ///
                if (_ExchangeRate.ExchangeRateId == 0)
                {
                    _ExchangeRate.DayofRate = DateTime.Now;
                    _ExchangeRate.CreatedDate = DateTime.Now;
                    _ExchangeRate.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ExchangeRateP);
                    var value = (insertresult.Result as ObjectResult).Value;
                    ExchangeRate resultado = ((ExchangeRate)(value));
                    if (resultado.ExchangeRateId <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se guardo la."));
                    }
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRateById/" + _ExchangeRate.ExchangeRateId);
                    string valorrespuesta = "";

                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);

                        if (_ExchangeRate == null)
                        {
                            _ExchangeRate = new Models.ExchangeRate();
                        }

                       
                    }

                    _ExchangeRateP.CreatedUser = _ExchangeRate.CreatedUser;
                    _ExchangeRateP.CreatedDate = _ExchangeRate.CreatedDate;
                    _ExchangeRateP.ModifiedUser = HttpContext.Session.GetString("user");
                    _ExchangeRateP.ModifiedDate = DateTime.Now;

                    var updateresult = await Update(_ExchangeRate.ExchangeRateId, _ExchangeRateP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ExchangeRateP);
        }



        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddExchangeRate([FromBody]ExchangeRateDTO _sarpara)
        {
            ExchangeRateDTO _ExchangeRate = new ExchangeRateDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();               
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRateById/" + _sarpara.ExchangeRateId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRateDTO>(valorrespuesta);
                }
              if (_ExchangeRate == null)
                {
                    _ExchangeRate = new ExchangeRateDTO();
                    {
                        //NUEVO
                        _ExchangeRate.DayofRate = DateTime.Now;
                        ///
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => PartialView(_ExchangeRate));
            //return PartialView(_ExchangeRate);
        }


        //
        //[HttpPost("[action]")]
        //public async Task<ActionResult> pvwAddExchangeRateDate([FromBody]ExchangeRateDTO _sarpara)
        //{
        //    ExchangeRateDTO _ExchangeRate = new ExchangeRateDTO();
        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/ExchangeRate/GetExchangeRateById/" + _sarpara.ExchangeRateId);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRateDTO>(valorrespuesta);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        throw ex;
        //    }
        //    return PartialView(_ExchangeRate);
        //}
        //

        // POST: ExchangeRate/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<ExchangeRate>> Insert(ExchangeRate _ExchangeRate)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ExchangeRate.CreatedUser = HttpContext.Session.GetString("user");
                _ExchangeRate.CreatedDate = DateTime.Now;
                _ExchangeRate.ModifiedUser = HttpContext.Session.GetString("user");
                _ExchangeRate.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/ExchangeRate/Insert", _ExchangeRate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }


            return Ok(_ExchangeRate);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _ExchangeRate }, Total = 1 });
        }

        [HttpPut("ExchangeRateId")]
        public async Task<IActionResult> Update(Int64 ExchangeRateId, ExchangeRate _ExchangeRate)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ExchangeRate/Update", _ExchangeRate);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ExchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ExchangeRate }, Total = 1 });
        }

    }
}
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
    public class CertificadoLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CertificadoLineController(ILogger<CertificadoLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwCertificadoLine(Int64 Id = 0)
        {
            CertificadoLine _CertificadoLine = new CertificadoLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoLine/GetCertificadoLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);

                }

                if (_CertificadoLine == null)
                {
                    _CertificadoLine = new CertificadoLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CertificadoLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CertificadoLine> _CertificadoLine = new List<CertificadoLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoLine/GetCertificadoLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoLine = JsonConvert.DeserializeObject<List<CertificadoLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CertificadoLine.ToDataSourceResult(request);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrderLine([DataSourceRequest]DataSourceRequest request, CertificadoLine _CertificadoLine)
        {
            List<CertificadoLine> _CertificadoLinelist = new List<CertificadoLine>();

            try
            {

                if (HttpContext.Session.Get("listadoproductoscertificadodeposito") == null
                    || HttpContext.Session.GetString("listadoproductoscertificadodeposito") == "")
                {
                    if (_CertificadoLine.SubProductId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_CertificadoLinelist).ToString();
                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", serialzado);
                    }
                }
                else
                {
                    _CertificadoLinelist = JsonConvert.DeserializeObject<List<CertificadoLine>>(HttpContext.Session.GetString("listadoproductoscertificadodeposito"));
                }
                if (_CertificadoLine.IdCD > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                  //  _client.DefaultRequestHeaders.Add("SalesOrderId", _CertificadoLine.IdCD.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CertificadoLine/");
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _CertificadoLinelist = JsonConvert.DeserializeObject<List<CertificadoLine>>(valorrespuesta);
                    }
                }
                else
                {
                    if (_CertificadoLine.SubProductId > 0)
                    {
                        _CertificadoLinelist.Add(_CertificadoLine);
                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", JsonConvert.SerializeObject(_CertificadoLinelist).ToString());
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CertificadoLinelist.ToDataSourceResult(request);
        }






        [HttpPost("[action]")]
        public async Task<ActionResult<CertificadoLine>> SaveCertificadoLine([FromBody]CertificadoLine _CertificadoLine)
        {

            try
            {
                CertificadoLine _listCertificadoLine = new CertificadoLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoLine/GetCertificadoLineById/" + _CertificadoLine.CertificadoLineId);
                string valorrespuesta = "";
               // _CertificadoLine.FechaModificacion = DateTime.Now;
                //_CertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);
                }

                if (_listCertificadoLine.CertificadoLineId == 0)
                {
                   // _CertificadoLine.FechaCreacion = DateTime.Now;
                    //_CertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CertificadoLine);
                }
                else
                {
                    var updateresult = await Update(_CertificadoLine.CertificadoLineId, _CertificadoLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CertificadoLine);
        }




        // POST: CertificadoLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CertificadoLine>> Insert(CertificadoLine _CertificadoLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _CertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_CertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoLine/Insert", _CertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CertificadoLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CertificadoLine>> Update(Int64 id, CertificadoLine _CertificadoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CertificadoLine/Update", _CertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CertificadoLine>> Delete([FromBody]CertificadoLine _CertificadoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoLine/Delete", _CertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoLine }, Total = 1 });
        }





    }
}
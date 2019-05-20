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
    public class GoodsReceivedLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsReceivedLineController(ILogger<GoodsReceivedLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwGoodsReceivedLine(Int64 Id = 0)
        {
            GoodsReceivedLine _GoodsReceivedLine = new GoodsReceivedLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceivedLine/GetGoodsReceivedLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);

                }

                if (_GoodsReceivedLine == null)
                {
                    _GoodsReceivedLine = new GoodsReceivedLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsReceivedLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsReceivedLine> _GoodsReceivedLine = new List<GoodsReceivedLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceivedLine/GetGoodsReceivedLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceivedLine.ToDataSourceResult(request);

        }


        public async Task<ActionResult<GoodsReceivedLine>> SaveGoodsReceivedLine([FromBody]GoodsReceivedLine _GoodsReceivedLine)
        {

            try
            {
                GoodsReceivedLine _listGoodsReceivedLine = new GoodsReceivedLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceivedLine/GetGoodsReceivedLineById/" + _GoodsReceivedLine.GoodsReceiveLinedId);
                string valorrespuesta = "";
                _GoodsReceivedLine.FechaModificacion = DateTime.Now;
                _GoodsReceivedLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                }

                if (_listGoodsReceivedLine.GoodsReceiveLinedId == 0)
                {
                    _GoodsReceivedLine.FechaCreacion = DateTime.Now;
                    _GoodsReceivedLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsReceivedLine);
                }
                else
                {
                    var updateresult = await Update(_GoodsReceivedLine.GoodsReceiveLinedId, _GoodsReceivedLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsReceivedLine);
        }

        // POST: GoodsReceivedLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsReceivedLine>> Insert(GoodsReceivedLine _GoodsReceivedLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsReceivedLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsReceivedLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceivedLine/Insert", _GoodsReceivedLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceivedLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsReceivedLine>> Update(Int64 id, GoodsReceivedLine _GoodsReceivedLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsReceivedLine/Update", _GoodsReceivedLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceivedLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsReceivedLine>> Delete([FromBody]GoodsReceivedLine _GoodsReceivedLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceivedLine/Delete", _GoodsReceivedLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceivedLine }, Total = 1 });
        }





    }
}
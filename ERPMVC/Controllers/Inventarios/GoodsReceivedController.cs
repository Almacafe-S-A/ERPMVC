﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class GoodsReceivedController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public GoodsReceivedController(ILogger<GoodsReceivedController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Recibo de Mercaderia")]
        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsReceived([FromBody]GoodsReceivedDTO _GoodsReceivedDTO)
        {
            GoodsReceivedDTO _GoodsReceived = new GoodsReceivedDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceivedDTO.GoodsReceivedId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceivedDTO>(valorrespuesta);

                }

                if (_GoodsReceived == null)
                {
                    _GoodsReceived = new GoodsReceivedDTO {  DocumentDate=DateTime.Now, ExpirationDate=DateTime.Now,OrderDate=DateTime.Now,editar=1
                    ,
                        BranchId = Convert.ToInt64(HttpContext.Session.GetString("BranchId"))
                    };
                }
                else
                {
                    _GoodsReceived.editar = 0;
                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsReceived);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsReceived> _GoodsReceived = new List<GoodsReceived>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceived");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceived = _GoodsReceived.OrderByDescending(q => q.GoodsReceivedId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }

       // [HttpGet("[controller]/[action]/{id}")]
        public ActionResult SFGoodsReceived(Int32 id)
        {

            GoodsReceivedDTO _GoodsReceivedDTO = new GoodsReceivedDTO { GoodsReceivedId = id, }; //token = HttpContext.Session.GetString("token") };

            return View(_GoodsReceivedDTO);
        }


        //public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request, Int64 CustomerId)
        //{
        //    var res = await GetGoodsReceived(CustomerId);
        //    // var res = await GetGoodsReceived();
        //    return Json(res.ToDataSourceResult(request));
        //}

        //public async Task<ActionResult> Orders_ValueMapper(GoodsReceivedParams _goodsparm)
        ////public async Task<ActionResult> Orders_ValueMapper(Int64[] values,Int64 CustomerId)
        //{
        //    var indices = new List<Int64>();

        //    if (_goodsparm.values != null && _goodsparm.values.Any())
        //    {
        //        var index = 0;

        //        foreach (var order in await GetGoodsReceived(_goodsparm.CustomerId))
        //        {
        //            if (_goodsparm.values.Contains(order.GoodsReceivedId))
        //            {
        //                indices.Add(index);
        //            }

        //            index += 1;
        //        }
        //    }

        //    return Json(indices);
        //}

        //private async Task<List<GoodsReceived>> GetGoodsReceived(Int64 CustomerId)
        //{
        //    List<GoodsReceived> _ControlPallets = new List<GoodsReceived>();

        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
        //        var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedNoSelected");
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _ControlPallets = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
        //            _ControlPallets = (from c in _ControlPallets
        //                                    .Where(q => q.CustomerId == CustomerId)
        //                               select new GoodsReceived
        //                               {
        //                                   GoodsReceivedId = c.GoodsReceivedId,
        //                                   ProductName = "Nombre:" + c.ProductName + "|| Fecha: " + c.DocumentDate + " || Total:" + c.WarehouseName,
        //                                   DocumentDate = c.DocumentDate,

        //                               }
        //                              ).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    // return Json(_CustomerConditions.ToDataSourceResult(request));
        //    return _ControlPallets;
        //}


        /// <summary>
        /// Posible
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>


        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request,GoodsReceived _gr)
        {
           
            var res = await GetGoodsReceived(_gr);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values, GoodsReceived _gr)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetGoodsReceived(_gr))
                {
                    if (values.Contains(order.GoodsReceivedId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        [HttpGet("[controller]/[action]")]
        private async Task<List<GoodsReceived>> GetGoodsReceived( GoodsReceived _gr)
        {
            List<GoodsReceived> _GoodsReceiveds = new List<GoodsReceived>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedNoSelected");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceiveds = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _GoodsReceiveds = (from c in _GoodsReceiveds
                                       .Where(q => q.CustomerId == _gr.CustomerId)
                                        .Where(q => q.ProductId == _gr.ProductId)
                                       select new GoodsReceived
                                       {
                                           GoodsReceivedId = c.GoodsReceivedId,
                                           ProductName = " No. Documento:" + c.GoodsReceivedId + " || Cliente:" + c.CustomerName+ " || Tipo de servicio:" + c.ProductName +" || Fecha: " + c.DocumentDate ,
                                           //DocumentDate = c.DocumentDate,
                                           CustomerId = c.CustomerId,
                                       }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _GoodsReceiveds;
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<List<GoodsReceivedLine>>> AgruparRecibos([FromBody]GoodsReceivedParams _params)
        {
            List<GoodsReceivedLine> _GoodsReceived = new List<GoodsReceivedLine>();
            if(_params!=null)
            if (_params.RecibosSeleccionados != null)
            {
            
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/AgruparRecibos", _params.RecibosSeleccionados);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(valorrespuesta);
                      
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {

            }

            return Json(_GoodsReceived);
        }


      




        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> GetGoodsReceivedById([DataSourceRequest]DataSourceRequest request,[FromBody] GoodsReceived _GoodsReceivedId)
        {
            GoodsReceived _GoodsReceived = new GoodsReceived();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceivedId.GoodsReceivedId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_GoodsReceived);

        }


        [HttpPost("[controller]/[action]")]
       public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]GoodsReceivedDTO _GoodsReceived)
          // public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]dynamic _GoodsReceived)
        {
            try
            {
                if (_GoodsReceived != null)
                {

                    
                    GoodsReceived _listGoodsReceived = new GoodsReceived();
                   // _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(_GoodsReceived.ToString());
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceived.GoodsReceivedId);
                    string valorrespuesta = "";
                    _GoodsReceived.FechaModificacion = DateTime.Now;
                    _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                    }

                    if (_listGoodsReceived == null)
                    {
                        _listGoodsReceived = new GoodsReceived();
                    }

                    if (_listGoodsReceived.GoodsReceivedId == 0)
                    {
                        _GoodsReceived.FechaCreacion = DateTime.Now;
                        _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_GoodsReceived);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _GoodsReceived = ((GoodsReceivedDTO)(value));
                        if (_GoodsReceived.GoodsReceivedId == 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero el documento!"));
                        }
                        
                        return Ok(_GoodsReceived);
                    }
                    else
                    {
                        //var updateresult = await Update(_GoodsReceived.GoodsReceivedId, _GoodsReceived);
                    }

                   
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return BadRequest("No llego correctamente el modelo!");
        }

        // POST: GoodsReceived/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsReceived>> Insert(GoodsReceivedDTO _GoodsReceived)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Insert", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceivedDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_GoodsReceived);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsReceived>> Update(Int64 id, GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsReceived/Update", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsReceived>> Delete([FromBody]GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Delete", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }





    }

    public class GoodsReceivedParams
    {
        public Int64[] values { get; set; }

        public Int64 CustomerId { get; set; } 

        public List<Int64> RecibosSeleccionados { get; set; }

    }

    


}
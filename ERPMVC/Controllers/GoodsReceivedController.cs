using System;
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
    public class GoodsReceivedController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsReceivedController(ILogger<GoodsReceivedController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
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
                    _GoodsReceived = new GoodsReceivedDTO {  DocumentDate=DateTime.Now,OrderDate=DateTime.Now,editar=1 };
                }
                else
                {
                    _GoodsReceived.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsReceived);

        }


        [HttpGet]
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

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }


        //public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request,Int64 CustomerId)
        //{
        //    var res = await GetGoodsReceived(CustomerId);
        //   // var res = await GetGoodsReceived();
        //    return Json(res.ToDataSourceResult(request));
        //}

        //public async Task<ActionResult> Orders_ValueMapper(GoodsReceivedParams _goods)
        //{
        //    var indices = new List<Int64>();

        //    if (_goods.values != null && _goods.values.Any())
        //    {
        //        var index = 0;

        //        foreach (var order in await GetGoodsReceived(_goods.CustomerId))
        //        {
        //            if (_goods.values.Contains(order.GoodsReceivedId))
        //            {
        //                indices.Add(index);
        //            }

        //            index += 1;
        //        }
        //    }

        //    return Json(indices);
        //}

        //public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        //{
        //    var indices = new List<Int64>();

        //    if (values != null && values.Any())
        //    {
        //        var index = 0;

        //        foreach (var order in await GetGoodsReceived(_goods.CustomerId))
        //        {
        //            if (values.Contains(order.GoodsReceivedId))
        //            {
        //                indices.Add(index);
        //            }

        //            index += 1;
        //        }
        //    }

        //    return Json(indices);
        //}

        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request, Int64 CustomerId)
        {
            //var res = await GetGoodsReceived(CustomerId);
            var res = await GetGoodsReceived();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await  GetGoodsReceived())//GetGoodsReceived(_goods.CustomerId))
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
               
        
        private async Task<List<GoodsReceived>> GetGoodsReceived()
        {
            List<GoodsReceived> _ControlPallets = new List<GoodsReceived>();

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
                    _ControlPallets = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);
                    _ControlPallets = (from c in _ControlPallets
                                      // .Where(q => q.CustomerId == CustomerId)
                                       select new GoodsReceived
                                       {
                                           GoodsReceivedId = c.GoodsReceivedId,
                                           ProductName = "Nombre:" + c.ProductName + "|| Fecha: " + c.DocumentDate + " || Total:" + c.WarehouseName,
                                           DocumentDate = c.DocumentDate,
                                       }
                                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return Json(_CustomerConditions.ToDataSourceResult(request));
            return _ControlPallets;
        }


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
        //                                   // .Where(q => q.CustomerId == CustomerId)
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
        //                               .Where(q=>q.CustomerId==CustomerId)
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



        [HttpPost("[action]")]
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


        [HttpPost("[action]")]
       public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]GoodsReceived _GoodsReceived)
           //public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]dynamic _GoodsReceived)
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
                        _GoodsReceived = ((GoodsReceived)(value));

                        return _GoodsReceived;
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
        public async Task<ActionResult<GoodsReceived>> Insert(GoodsReceived _GoodsReceived)
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
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
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

    }


}
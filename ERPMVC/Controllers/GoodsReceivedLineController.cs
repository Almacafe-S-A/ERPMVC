using System;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class GoodsReceivedLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsReceivedLineController(ILogger<GoodsReceivedLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[Controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwGoodsReceivedMant([FromBody]GoodsReceivedLine _GoodsReceivedLinep)
        {
            GoodsReceivedLine _GoodsReceivedLine = new GoodsReceivedLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceivedLine/GetGoodsReceivedLineById/" + _GoodsReceivedLinep.GoodsReceiveLinedId);
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



            return PartialView("~/Views/GoodsReceived/pvwGoodsReceivedDetailMant.cshtml", _GoodsReceivedLine);

        }


        [HttpGet("[Controller]/[action]")]
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

        [HttpPost("[controller]/[action]")]
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
                //_GoodsReceivedLine.FechaModificacion = DateTime.Now;
                _GoodsReceivedLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                }

                if (_listGoodsReceivedLine.GoodsReceiveLinedId == 0)
                {
                   // _GoodsReceivedLine.FechaCreacion = DateTime.Now;
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


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetGoodsReceivedLineByGoodsReceivedId([DataSourceRequest]DataSourceRequest request, GoodsReceivedLine _GoodsReceivedLinep)
        {
            List<GoodsReceivedLine> _GoodsReceivedLine = new List<GoodsReceivedLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosgoodsreceived") == null
                   || HttpContext.Session.GetString("listadoproductosgoodsreceived") == "")
                {
                    if (_GoodsReceivedLinep.GoodsReceivedId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_GoodsReceivedLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosgoodsreceived", serialzado);
                    }
                }
                else
                {
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(HttpContext.Session.GetString("listadoproductosgoodsreceived"));
                }


                if (_GoodsReceivedLinep.GoodsReceivedId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/GoodsReceivedLine/GetGoodsReceivedLineByGoodsReceivedId/" + _GoodsReceivedLinep.GoodsReceivedId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(valorrespuesta);
                    }
                }
                else
                {

                    List<GoodsReceivedLine> _existelinea = new List<GoodsReceivedLine>();
                    if (HttpContext.Session.GetString("listadoproductosgoodsreceived") != "")
                    {
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(HttpContext.Session.GetString("listadoproductosgoodsreceived"));
                        _existelinea = _GoodsReceivedLine.Where(q => q.GoodsReceiveLinedId == _GoodsReceivedLinep.GoodsReceiveLinedId).ToList();
                    }


                    if (_GoodsReceivedLinep.GoodsReceiveLinedId > 0 && _existelinea.Count == 0)
                    {
                        _GoodsReceivedLine.Add(_GoodsReceivedLinep);
                        HttpContext.Session.SetString("listadoproductosgoodsreceived", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());
                    }
                    else
                    {
                        var obj = _GoodsReceivedLine.Where(x => x.GoodsReceiveLinedId == _GoodsReceivedLinep.GoodsReceiveLinedId).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Description = _GoodsReceivedLinep.Description;
                            obj.Price = _GoodsReceivedLinep.Price;
                            obj.Quantity = _GoodsReceivedLinep.Quantity;
                            obj.QuantitySacos = _GoodsReceivedLinep.QuantitySacos;
                            obj.SubProductId = _GoodsReceivedLinep.SubProductId;
                            obj.SubProductName = _GoodsReceivedLinep.SubProductName;
                            obj.Total = _GoodsReceivedLinep.Total;
                            obj.UnitOfMeasureId = _GoodsReceivedLinep.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _GoodsReceivedLinep.UnitOfMeasureName;
                            obj.WareHouseId = _GoodsReceivedLinep.WareHouseId;
                            obj.WareHouseName = _GoodsReceivedLinep.WareHouseName;
                            obj.ControlPalletsId = _GoodsReceivedLinep.ControlPalletsId;

                        }

                        HttpContext.Session.SetString("listadoproductosgoodsreceived", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());

                    }

                }

             

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceivedLine.ToDataSourceResult(request);

        }



        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<GoodsReceivedLine>> Update(Int64 id, [FromBody]GoodsReceivedLine _GoodsReceivedLine)
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

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsReceivedLine>> Delete([FromBody]GoodsReceivedLine _GoodsReceivedLine)
        {
            try
            {

                List<GoodsReceivedLine> _GoodsReceivedLineLIST =
                    JsonConvert.DeserializeObject<List<GoodsReceivedLine>>(HttpContext.Session.GetString("listadoproductosgoodsreceived"));

                if (_GoodsReceivedLineLIST != null)
                {
                    _GoodsReceivedLineLIST = _GoodsReceivedLineLIST
                           .Where(q => q.GoodsReceiveLinedId != _GoodsReceivedLine.GoodsReceiveLinedId)
                           .Where(q => q.Quantity != _GoodsReceivedLine.Quantity)
                           .Where(q=>q.QuantitySacos!=_GoodsReceivedLine.QuantitySacos)
                           .Where(q => q.Total != _GoodsReceivedLine.Total)
                           .Where(q => q.Price != _GoodsReceivedLine.Price)
                          // .Where(q => q.UnitOfMeasureId != _GoodsReceivedLine.UnitOfMeasureId)
                           .ToList();
                        
                    HttpContext.Session.SetString("listadoproductosgoodsreceived", JsonConvert.SerializeObject(_GoodsReceivedLineLIST));
                }
                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                //var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceivedLine/Delete", _GoodsReceivedLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _GoodsReceivedLine = JsonConvert.DeserializeObject<GoodsReceivedLine>(valorrespuesta);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return await Task.Run(() => new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceivedLine }, Total = 1 }));
        }





    }
}
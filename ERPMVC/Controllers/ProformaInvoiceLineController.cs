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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProformaInvoiceLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProformaInvoiceLineController(ILogger<ProformaInvoiceLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwProformaInvoiceLine([FromBody]ProformaInvoiceLineDTO _salesorderline)
        {
            ProformaInvoiceLineDTO _ProformaInvoiceLine = new ProformaInvoiceLineDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineById/" + _salesorderline.ProformaLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLineDTO>(valorrespuesta);

                }

                if (_ProformaInvoiceLine == null)
                {
                    _ProformaInvoiceLine = new ProformaInvoiceLineDTO { IdCD = _salesorderline.IdCD };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return PartialView("~/Views/ProformaInvoice/pvwProformaInvoiceDetailMant.cshtml", _ProformaInvoiceLine);
            //  return PartialView(_ProformaInvoiceLine);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> SetLinesInSession([FromBody]ProformaInvoiceLine _ProformaInvoiceLine)
        {

            try
            {

                List<ProformaInvoiceLine> _GoodsReceivedLine = new List<ProformaInvoiceLine>();
                _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));

                if (_GoodsReceivedLine == null) { _GoodsReceivedLine = new List<ProformaInvoiceLine>(); }
                _GoodsReceivedLine.Add(_ProformaInvoiceLine);
                //  GoodsDeliveryAuthorizationLine _listGoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
                // string serialzado = JsonConvert.SerializeObject(_GoodsDeliveryAuthorizationLine).ToString();
                HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_ProformaInvoiceLine));
        }



        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetProformaInvoiceLineByProformaInvoiceId([DataSourceRequest]DataSourceRequest request, ProformaInvoiceLine _ProformaInvoiceLinep)
        {
            List<ProformaInvoiceLine> _GoodsReceivedLine = new List<ProformaInvoiceLine>();
            try
            {
                
                if (HttpContext.Session.Get("listadoproductosproformainvoice") == null
                   || HttpContext.Session.GetString("listadoproductosproformainvoice") == "")
                {
                    if (_ProformaInvoiceLinep.ProformaInvoiceId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_ProformaInvoiceLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosproformainvoice", serialzado);
                    }
                }
                else
                { 
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));
                }
                if (_ProformaInvoiceLinep.ProformaInvoiceId > 0 && _ProformaInvoiceLinep.ProformaLineId > 0)
                {
                    ProformaInvoice _ProformaInvoiceP = new ProformaInvoice();

                    //_ProformaInvoiceLinep.ProformaLineId = 0;
                    var updateresult = await Update(_ProformaInvoiceLinep.ProformaLineId, _ProformaInvoiceLinep);
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    _ProformaInvoiceP.UsuarioModificacion = HttpContext.Session.GetString("user");
                    _ProformaInvoiceP.FechaModificacion = DateTime.Now;
                    var result1 = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/Update/" + _ProformaInvoiceLinep);
                    string valorrespuesta1 = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(valorrespuesta1);  
                    }

                }


                if (_ProformaInvoiceLinep.ProformaInvoiceId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineByProformaInvoiceId/" + _ProformaInvoiceLinep.ProformaInvoiceId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());
                    }
                }
                else
                {

                    List<ProformaInvoiceLine> _existelinea = new List<ProformaInvoiceLine>();
                    if (HttpContext.Session.GetString("listadoproductosproformainvoice") != "" && HttpContext.Session.GetString("listadoproductosproformainvoice") != null)
                    {
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));
                        _existelinea = _GoodsReceivedLine.Where(q => q.ProformaLineId == _ProformaInvoiceLinep.ProformaLineId).ToList();
                    }

                    if (_ProformaInvoiceLinep.ProformaLineId > 0 && _existelinea.Count == 0)
                    {
                        _GoodsReceivedLine.Add(_ProformaInvoiceLinep);
                        HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());
                    }
                    else
                    {

                        var obj = _GoodsReceivedLine.FirstOrDefault(x => x.ProformaLineId == _ProformaInvoiceLinep.ProformaLineId);
                        if (obj != null)
                        {
                            obj.Description = _ProformaInvoiceLinep.Description;
                            obj.Price = _ProformaInvoiceLinep.Price;
                            obj.Quantity = _ProformaInvoiceLinep.Quantity;
                            obj.Amount = _ProformaInvoiceLinep.Amount;
                            obj.SubProductId = _ProformaInvoiceLinep.SubProductId;
                            obj.SubProductName = _ProformaInvoiceLinep.SubProductName;
                            obj.SubTotal = _ProformaInvoiceLinep.SubTotal;
                            obj.TaxAmount = _ProformaInvoiceLinep.TaxAmount;
                            obj.TaxId = _ProformaInvoiceLinep.TaxId;
                            obj.TaxCode = _ProformaInvoiceLinep.TaxCode;
                            obj.TaxPercentage = _ProformaInvoiceLinep.TaxPercentage;
                            obj.UnitOfMeasureId = _ProformaInvoiceLinep.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _ProformaInvoiceLinep.UnitOfMeasureName;
                            obj.WareHouseId = _ProformaInvoiceLinep.WareHouseId;
                            obj.CostCenterId = _ProformaInvoiceLinep.CostCenterId;
                            obj.DiscountAmount = _ProformaInvoiceLinep.DiscountAmount;
                            obj.DiscountPercentage = _ProformaInvoiceLinep.DiscountPercentage;
                            obj.Total = _ProformaInvoiceLinep.Total;

                        }

                        HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());

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




        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProformaInvoiceLine> _ProformaInvoiceLine = new List<ProformaInvoiceLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProformaInvoiceLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProformaInvoiceLine>> SaveProformaInvoiceLine([FromBody]ProformaInvoiceLine _ProformaInvoiceLine)
        {

            try
            {
                ProformaInvoiceLine _listProformaInvoiceLine = new ProformaInvoiceLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineById/" + _ProformaInvoiceLine.ProformaLineId);
                string valorrespuesta = "";
                // _ProformaInvoiceLine.FechaModificacion = DateTime.Now;
                //_ProformaInvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

                if (_listProformaInvoiceLine.ProformaLineId == 0)
                {
                    //  _ProformaInvoiceLine.FechaCreacion = DateTime.Now;
                    // _ProformaInvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProformaInvoiceLine);
                }
                else
                {
                    var updateresult = await Update(_ProformaInvoiceLine.ProformaLineId, _ProformaInvoiceLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProformaInvoiceLine);
        }

        // POST: ProformaInvoiceLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoiceLine>> Insert(ProformaInvoiceLine _ProformaInvoiceLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //_ProformaInvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_ProformaInvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Insert", _ProformaInvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ProformaInvoiceLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProformaInvoiceLine>> Update(Int64 id, ProformaInvoiceLine _ProformaInvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Update", _ProformaInvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<ProformaInvoiceLine>> Delete([FromBody]ProformaInvoiceLine _ProformaInvoiceLine)
        {
            List<ProformaInvoiceLine> _ProformaInvoiceLIST = new List<ProformaInvoiceLine>();
            try
            {
                _ProformaInvoiceLIST = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));

                if (_ProformaInvoiceLIST != null)
                {
                    var item = _ProformaInvoiceLIST.Find(c => c.ProformaLineId == _ProformaInvoiceLine.ProformaLineId);
                    _ProformaInvoiceLIST.Remove(item);
                    HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_ProformaInvoiceLIST));
                }

                //  List<ProformaInvoiceLine> _salesorderLIST =
                //JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));

                //  if (_salesorderLIST != null)
                //  {
                //      _salesorderLIST = _salesorderLIST
                //              .Where(q => q.ProformaLineId == _ProformaInvoiceLine.ProformaLineId)
                //             .Where(q => q.Quantity != _ProformaInvoiceLine.Quantity)
                //             .Where(q => q.Amount != _ProformaInvoiceLine.Amount)
                //             .Where(q => q.Total != _ProformaInvoiceLine.Total)
                //             .Where(q => q.Price != _ProformaInvoiceLine.Price)
                //             .Where(q => q.SubProductId != _ProformaInvoiceLine.SubProductId)
                //            .ToList();

                //      HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_salesorderLIST));
                //  }

                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                //var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Delete", _ProformaInvoiceLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(()=> Ok(_ProformaInvoiceLine));
           //return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }





    }
}
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
    public class DebitNoteLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public DebitNoteLineController(ILogger<DebitNoteLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwDebitNoteLine([FromBody] DebitNoteLine _DebitNoteLinep)
        {
            DebitNoteLine _DebitNoteLine = new DebitNoteLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLineById/" + _DebitNoteLinep.DebitNoteLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);

                }

                if (_DebitNoteLine == null)
                {
                    _DebitNoteLine = new DebitNoteLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return PartialView("~/Views/DebitNote/pvwDebitNoteDetailMant.cshtml", _DebitNoteLine);

            //return PartialView(_DebitNoteLine);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> SetLinesInSession([FromBody]DebitNoteLine _DebitNoteLine)
        {

            try
            {

                List<DebitNoteLine> _GoodsReceivedLine = new List<DebitNoteLine>();
                _GoodsReceivedLine = JsonConvert.DeserializeObject<List<DebitNoteLine>>(HttpContext.Session.GetString("listadoproductosdebitnote"));

                if (_GoodsReceivedLine == null) { _GoodsReceivedLine = new List<DebitNoteLine>(); }
                _GoodsReceivedLine.Add(_DebitNoteLine);
                HttpContext.Session.SetString("listadoproductosdebitnote", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_DebitNoteLine));
        }



        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetDebitNoteLineByDebitNoteId([DataSourceRequest]DataSourceRequest request, DebitNoteLine _DebitNoteLinep)
        {
            List<DebitNoteLine> __DebitNoteLineList = new List<DebitNoteLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosdebitnote") == null
                   || HttpContext.Session.GetString("listadoproductosdebitnote") == "")
                {
                    if (_DebitNoteLinep.DebitNoteId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_DebitNoteLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosdebitnote", serialzado);
                    }
                }
                else
                {
                    var result = HttpContext.Session.GetString("listadoproductosdebitnote");
                    try
                    {
                        __DebitNoteLineList = JsonConvert.DeserializeObject<List<DebitNoteLine>>(HttpContext.Session.GetString("listadoproductosdebitnote"));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    }

                }


                if (_DebitNoteLinep.DebitNoteId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLineByDebitNoteId/" + _DebitNoteLinep.DebitNoteId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        __DebitNoteLineList = JsonConvert.DeserializeObject<List<DebitNoteLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosdebitnote", JsonConvert.SerializeObject(__DebitNoteLineList).ToString());
                    }
                }
                else
                {

                    List<DebitNoteLine> _existelinea = new List<DebitNoteLine>();
                    if (HttpContext.Session.GetString("listadoproductosdebitnote") != "" && HttpContext.Session.GetString("listadoproductosdebitnote") != null)
                    {
                        __DebitNoteLineList = JsonConvert.DeserializeObject<List<DebitNoteLine>>(HttpContext.Session.GetString("listadoproductosdebitnote"));
                        _existelinea = __DebitNoteLineList.Where(q => q.DebitNoteLineId == _DebitNoteLinep.DebitNoteLineId).ToList();
                    }

                    if (_DebitNoteLinep.DebitNoteLineId > 0 && _existelinea.Count == 0)
                    {
                        __DebitNoteLineList.Add(_DebitNoteLinep);
                        HttpContext.Session.SetString("listadoproductosdebitnote", JsonConvert.SerializeObject(__DebitNoteLineList).ToString());
                    }
                    else
                    {

                        var obj = __DebitNoteLineList.FirstOrDefault(x => x.DebitNoteLineId == _DebitNoteLinep.DebitNoteLineId);
                        if (obj != null)
                        {
                            obj.Description = _DebitNoteLinep.Description;
                            obj.Price = _DebitNoteLinep.Price;
                            obj.Quantity = _DebitNoteLinep.Quantity;
                            obj.Amount = _DebitNoteLinep.Amount;
                            obj.SubProductId = _DebitNoteLinep.SubProductId;
                            obj.SubProductName = _DebitNoteLinep.SubProductName;
                            obj.AccountId = _DebitNoteLinep.AccountId;
                            obj.AccountName = _DebitNoteLinep.AccountName;
                            obj.SubTotal = _DebitNoteLinep.SubTotal;
                            obj.TaxAmount = _DebitNoteLinep.TaxAmount;
                            obj.TaxId = _DebitNoteLinep.TaxId;
                            obj.TaxCode = _DebitNoteLinep.TaxCode;
                            obj.TaxPercentage = _DebitNoteLinep.TaxPercentage;
                            obj.UnitOfMeasureId = _DebitNoteLinep.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _DebitNoteLinep.UnitOfMeasureName;
                            obj.WareHouseId = _DebitNoteLinep.WareHouseId;
                            obj.CostCenterId = _DebitNoteLinep.CostCenterId;
                            obj.DiscountAmount = _DebitNoteLinep.DiscountAmount;
                            obj.DiscountPercentage = _DebitNoteLinep.DiscountPercentage;
                            obj.Total = _DebitNoteLinep.Total;

                        }

                        HttpContext.Session.SetString("listadoproductosdebitnote", JsonConvert.SerializeObject(__DebitNoteLineList).ToString());

                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return __DebitNoteLineList.ToDataSourceResult(request);

        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<DebitNoteLine> _DebitNoteLine = new List<DebitNoteLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<List<DebitNoteLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _DebitNoteLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DebitNoteLine>> SaveDebitNoteLine([FromBody]DebitNoteLine _DebitNoteLine)
        {

            try
            {
                DebitNoteLine _listDebitNoteLine = new DebitNoteLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/DebitNoteLine/GetDebitNoteLineById/" + _DebitNoteLine.DebitNoteLineId);
                string valorrespuesta = "";
               // _DebitNoteLine.FechaModificacion = DateTime.Now;
               // _DebitNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listDebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

                if (_listDebitNoteLine == null) { _listDebitNoteLine = new DebitNoteLine(); }

                if (_listDebitNoteLine.DebitNoteLineId == 0)
                {
                   // _DebitNoteLine.FechaCreacion = DateTime.Now;
                   // _DebitNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_DebitNoteLine);
                }
                else
                {
                    var updateresult = await Update(_DebitNoteLine.DebitNoteLineId, _DebitNoteLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_DebitNoteLine);
        }

        // POST: DebitNoteLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<DebitNoteLine>> Insert(DebitNoteLine _DebitNoteLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _DebitNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _DebitNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNoteLine/Insert", _DebitNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_DebitNoteLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DebitNoteLine>> Update(Int64 id, DebitNoteLine _DebitNoteLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/DebitNoteLine/Update", _DebitNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=>BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_DebitNoteLine);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }

        //[HttpDelete("DebitNoteLineId")]
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<DebitNoteLine>> Delete([FromBody]DebitNoteLine _DebitNoteLine)
        {
            List<DebitNoteLine> _debitnoteLIST = new List<DebitNoteLine>();
            try {
                _debitnoteLIST=JsonConvert.DeserializeObject<List<DebitNoteLine>>(HttpContext.Session.GetString("listadoproductosdebitnote"));

                if (_debitnoteLIST != null)
                {
                    //_debitnoteLIST = _debitnoteLIST
                    //      .Where(q => q.DebitNoteLineId != _DebitNoteLine.DebitNoteLineId)
                    //       .Where(q => q.Quantity != _DebitNoteLine.Quantity)
                    //       .Where(q => q.Amount != _DebitNoteLine.Amount)
                    //       .Where(q => q.Total != _DebitNoteLine.Total)
                    //       .Where(q => q.Price != _DebitNoteLine.Price)
                    //        .Where(q => q.AccountName != _DebitNoteLine.AccountName)
                    //    .Where(q => q.Description != _DebitNoteLine.Description)
                    //      .ToList();

                    var item = _debitnoteLIST.Find(c => c.DebitNoteLineId == _DebitNoteLine.DebitNoteLineId);
                    _debitnoteLIST.Remove(item);

                    HttpContext.Session.SetString("listadoproductosdebitnote", JsonConvert.SerializeObject(_debitnoteLIST));
                }
                //string baseadress = config.Value.urlbase;
                //    HttpClient _client = new HttpClient();
                //    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                //    var result = await _client.PostAsJsonAsync(baseadress + "api/DebitNoteLine/Delete", _DebitNoteLine);
                //    string valorrespuesta = "";
                //    if (result.IsSuccessStatusCode)
                //    {
                //        valorrespuesta = await (result.Content.ReadAsStringAsync());
                //        _DebitNoteLine = JsonConvert.DeserializeObject<DebitNoteLine>(valorrespuesta);
                //    }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }


            return await Task.Run(() => Ok(_DebitNoteLine));
            //return new ObjectResult(new DataSourceResult { Data = new[] { _DebitNoteLine }, Total = 1 });
        }





    }
}
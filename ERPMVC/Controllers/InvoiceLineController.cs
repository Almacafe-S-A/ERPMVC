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
    public class InvoiceLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InvoiceLineController(ILogger<InvoiceLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwInvoiceLine([FromBody]InvoiceLine _InvoiceLinep)
        {
            InvoiceLine _InvoiceLine = new InvoiceLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoiceLine/GetInvoiceLineById/" + _InvoiceLinep.InvoiceLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InvoiceLine>(valorrespuesta);

                }

                if (_InvoiceLine == null)
                {
                    _InvoiceLine = new InvoiceLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InvoiceLine);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetInvoiceLineByInvoiceId([DataSourceRequest]DataSourceRequest request, InvoiceLine _InvoiceLinep)
        {
            List<InvoiceLine> __InvoiceLineList = new List<InvoiceLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosinvoice") == null
                   || HttpContext.Session.GetString("listadoproductosinvoice") == "")
                {
                    if (_InvoiceLinep.InvoiceId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_InvoiceLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosinvoice", serialzado);
                    }
                }
                else
                {
                    var result = HttpContext.Session.GetString("listadoproductosinvoice");
                    try
                    {
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InvoiceLine>>(HttpContext.Session.GetString("listadoproductosinvoice"));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    }

                }


                if (_InvoiceLinep.InvoiceId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/InvoiceLine/GetInvoiceLineByInvoiceId/" + _InvoiceLinep.InvoiceId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InvoiceLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosinvoice", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                }
                else
                {

                    List<InvoiceLine> _existelinea = new List<InvoiceLine>();
                    if (HttpContext.Session.GetString("listadoproductosinvoice") != "" && HttpContext.Session.GetString("listadoproductosinvoice") != null)
                    {
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InvoiceLine>>(HttpContext.Session.GetString("listadoproductosinvoice"));
                        _existelinea = __InvoiceLineList.Where(q => q.InvoiceLineId == _InvoiceLinep.InvoiceLineId).ToList();
                    }

                    if (_InvoiceLinep.InvoiceLineId > 0 && _existelinea.Count == 0)
                    {
                        __InvoiceLineList.Add(_InvoiceLinep);
                        HttpContext.Session.SetString("listadoproductosinvoice", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                    else
                    {

                        var obj = __InvoiceLineList.FirstOrDefault(x => x.InvoiceLineId == _InvoiceLinep.InvoiceLineId);
                        if (obj != null)
                        {
                            obj.Description = _InvoiceLinep.Description;
                            obj.Price = _InvoiceLinep.Price;
                            obj.Quantity = _InvoiceLinep.Quantity;
                            obj.Amount = _InvoiceLinep.Amount;
                            obj.SubProductId = _InvoiceLinep.SubProductId;
                            obj.SubProductName = _InvoiceLinep.SubProductName;
                            obj.SubTotal = _InvoiceLinep.SubTotal;
                            obj.TaxAmount = _InvoiceLinep.TaxAmount;
                            obj.TaxCode = _InvoiceLinep.TaxCode;
                            obj.TaxPercentage = _InvoiceLinep.TaxPercentage;
                            obj.UnitOfMeasureId = _InvoiceLinep.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _InvoiceLinep.UnitOfMeasureName;
                            obj.WareHouseId = _InvoiceLinep.WareHouseId;
                            obj.CostCenterId = _InvoiceLinep.CostCenterId;
                            obj.DiscountAmount = _InvoiceLinep.DiscountAmount;
                            obj.DiscountPercentage = _InvoiceLinep.DiscountPercentage;

                        }

                        HttpContext.Session.SetString("listadoproductosinvoice", JsonConvert.SerializeObject(__InvoiceLineList).ToString());

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return __InvoiceLineList.ToDataSourceResult(request);

        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InvoiceLine> _InvoiceLine = new List<InvoiceLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoiceLine/GetInvoiceLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<List<InvoiceLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InvoiceLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InvoiceLine>> SaveInvoiceLine([FromBody]InvoiceLine _InvoiceLine)
        {

            try
            {
                InvoiceLine _listInvoiceLine = new InvoiceLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoiceLine/GetInvoiceLineById/" + _InvoiceLine.InvoiceLineId);
                string valorrespuesta = "";
               // _InvoiceLine.FechaModificacion = DateTime.Now;
                //_InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInvoiceLine = JsonConvert.DeserializeObject<InvoiceLine>(valorrespuesta);
                }

                if (_listInvoiceLine.InvoiceLineId == 0)
                {
                   // _InvoiceLine.FechaCreacion = DateTime.Now;
                  //  _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InvoiceLine);
                }
                else
                {
                    var updateresult = await Update(_InvoiceLine.InvoiceLineId, _InvoiceLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InvoiceLine);
        }

        // POST: InvoiceLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InvoiceLine>> Insert(InvoiceLine _InvoiceLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InvoiceLine/Insert", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_InvoiceLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceLine>> Update(Int64 id, InvoiceLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InvoiceLine/Update", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InvoiceLine>> Delete([FromBody]InvoiceLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InvoiceLine/Delete", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }





    }
}
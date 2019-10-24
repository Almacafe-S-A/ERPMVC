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
    public class VendorInvoiceLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public VendorInvoiceLineController(ILogger<VendorInvoiceLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwVendorInvoiceLine([FromBody]VendorInvoiceLine _InvoiceLinep)
        {
            VendorInvoiceLine _InvoiceLine = new VendorInvoiceLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoiceLine/GetVendorInvoiceLineById/" + _InvoiceLinep.VendorInvoiceLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<VendorInvoiceLine>(valorrespuesta);

                }

                if (_InvoiceLine == null)
                {
                    _InvoiceLine = new VendorInvoiceLine();
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
        public async Task<DataSourceResult> GetVendorInvoiceLineByInvoiceId([DataSourceRequest]DataSourceRequest request, VendorInvoiceLine _InvoiceLinep)
        {
            List<VendorInvoiceLine> __InvoiceLineList = new List<VendorInvoiceLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosinvoice") == null
                   || HttpContext.Session.GetString("listadoproductosinvoice") == "")
                {
                    if (_InvoiceLinep.VendorInvoiceId > 0)
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
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<VendorInvoiceLine>>(HttpContext.Session.GetString("listadoproductosinvoice"));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    }

                }


                if (_InvoiceLinep.VendorInvoiceId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/VendorInvoiceLine/GetVendorInvoiceLineByInvoiceId/" + _InvoiceLinep.VendorInvoiceId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<VendorInvoiceLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosinvoice", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                }
                else
                {

                    List<VendorInvoiceLine> _existelinea = new List<VendorInvoiceLine>();
                    if (HttpContext.Session.GetString("listadoproductosinvoice") != "" && HttpContext.Session.GetString("listadoproductosinvoice") != null)
                    {
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<VendorInvoiceLine>>(HttpContext.Session.GetString("listadoproductosinvoice"));
                        _existelinea = __InvoiceLineList.Where(q => q.contadorid == _InvoiceLinep.contadorid).ToList();
                    }

                    if (_InvoiceLinep.contadorid > 0 && _existelinea.Count == 0)
                    {
                        __InvoiceLineList.Add(_InvoiceLinep);
                        HttpContext.Session.SetString("listadoproductosinvoice", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                    else
                    {

                        var obj = __InvoiceLineList.FirstOrDefault(x => x.contadorid == _InvoiceLinep.contadorid);
                        if (obj != null)
                        {
                            obj.contadorid = _InvoiceLinep.contadorid;
                            obj.Description = _InvoiceLinep.Description;
                            obj.Price = _InvoiceLinep.Price;
                            obj.Quantity = _InvoiceLinep.Quantity;
                            obj.Amount = _InvoiceLinep.Amount;
                            obj.ProductId = _InvoiceLinep.ProductId;
                            obj.SubTotal = _InvoiceLinep.SubTotal;
                            obj.TaxAmount = _InvoiceLinep.TaxAmount;
                            obj.TaxCode = _InvoiceLinep.TaxCode;
                            obj.TaxPercentage = _InvoiceLinep.TaxPercentage;
                            obj.UnitOfMeasureId = _InvoiceLinep.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _InvoiceLinep.UnitOfMeasureName;
                            obj.DiscountAmount = _InvoiceLinep.DiscountAmount;
                            obj.DiscountPercentage = _InvoiceLinep.DiscountPercentage;
                            obj.Total = _InvoiceLinep.Total;

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
            List<VendorInvoiceLine> _InvoiceLine = new List<VendorInvoiceLine>();
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
                    _InvoiceLine = JsonConvert.DeserializeObject<List<VendorInvoiceLine>>(valorrespuesta);

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
        public async Task<ActionResult<VendorInvoiceLine>> SaveVendorInvoiceLine([FromBody]VendorInvoiceLine _InvoiceLine)
        {

            try
            {
                VendorInvoiceLine _listInvoiceLine = new VendorInvoiceLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/VendorInvoiceLine/GetInvoiceLineById/" + _InvoiceLine.VendorInvoiceLineId);
                string valorrespuesta = "";
               // _InvoiceLine.FechaModificacion = DateTime.Now;
                //_InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInvoiceLine = JsonConvert.DeserializeObject<VendorInvoiceLine>(valorrespuesta);
                }

                if (_listInvoiceLine.VendorInvoiceLineId == 0)
                {
                   // _InvoiceLine.FechaCreacion = DateTime.Now;
                  //  _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InvoiceLine);
                }
                else
                {
                    var updateresult = await Update(_InvoiceLine.VendorInvoiceLineId, _InvoiceLine);
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
        public async Task<ActionResult<VendorInvoiceLine>> Insert(VendorInvoiceLine _InvoiceLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorInvoiceLine/Insert", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<VendorInvoiceLine>(valorrespuesta);
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
        public async Task<ActionResult<VendorInvoiceLine>> Update(Int64 id, VendorInvoiceLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/VendorInvoiceLine/Update", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<VendorInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

        [HttpDelete("VendorInvoiceLineId")]
        public async Task<ActionResult<VendorInvoiceLine>> Delete([FromBody]VendorInvoiceLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/VendorInvoiceLine/Delete", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<VendorInvoiceLine>(valorrespuesta);
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
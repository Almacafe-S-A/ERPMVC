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
    public class InvoiceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InvoiceController(ILogger<InvoiceController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwInvoice([FromBody]Invoice _Invoicep)
        {
            InvoiceDTO _Invoice = new InvoiceDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetInvoiceById/" + _Invoicep.InvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<InvoiceDTO>(valorrespuesta);
                   
                }

                if (_Invoice == null)
                {
                    _Invoice = new InvoiceDTO { OrderDate = DateTime.Now, DeliveryDate = DateTime.Now, ExpirationDate = DateTime.Now.AddDays(30), editar = 1 };
                }
                else
                {
                    _Invoice.NumeroDEIString = $"{_Invoice.Sucursal}-{_Invoice.Caja}-01-{_Invoice.NumeroDEI.ToString().PadLeft(8, '0')} ";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Invoice);

        }



    

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Invoice> _Invoice = new List<Invoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetInvoice");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<List<Invoice>>(valorrespuesta);
                    _Invoice = _Invoice.OrderByDescending(q => q.InvoiceId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Invoice.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult> GetInvoiceById([FromBody]Invoice _Invoicep)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            Invoice _Invoice = new Invoice();
            try
            {

                //GoodsDelivered _GoodsDeliveredp = JsonConvert.DeserializeObject<GoodsDelivered>(dto);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Invoice/GetInvoiceLineById", _Invoicep);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);

                }

                if (_Invoice == null)
                {
                    _Invoice = new Invoice();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Invoice);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<InvoiceDTO>> SaveInvoice([FromBody]InvoiceDTO _Invoice)
        {

            try
            {
                if (_Invoice != null)
                {
                    Invoice _listInvoice = new Invoice();
                    string baseadress = config.Value.urlbase;


                    if (_Invoice.Total <= 0 || _Invoice.SubTotal <= 0)
                    {
                        return await Task.Run(() => BadRequest($"No se esta calculando correctamente los totales!"));
                    }


                    foreach (var item in _Invoice.InvoiceLine)
                    {
                        if (item.UnitOfMeasureId == 0)
                        {
                            return await Task.Run(() => BadRequest($"Ingrese una unidad de medida valido!"));
                        }

                        if (item.Total == 0)
                        {
                            return await Task.Run(() => BadRequest($"El documento no se ha calculado correctamente!"));
                        }

                        if (item.TaxCode == "")
                        {
                            return await Task.Run(() => BadRequest($"Debe llevar un código de impuesto!, el producto :{item.SubProductName}"));
                        }

                    }

                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/Invoice/GetInvoiceById/" + _Invoice.InvoiceId);
                    string valorrespuesta = "";
                    _Invoice.FechaModificacion = DateTime.Now;
                    _Invoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listInvoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);
                    }

                    if (_listInvoice == null) { _listInvoice = new Invoice(); }

                    if (_listInvoice.InvoiceId == 0)
                    {
                        _Invoice.FechaCreacion = DateTime.Now;
                        _Invoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_Invoice);
                        var value = (insertresult.Result as ObjectResult).Value;

                        InvoiceDTO resultado = ((InvoiceDTO)(value));
                        if (resultado.InvoiceId <= 0)
                        {
                            return await Task.Run(() => BadRequest("No se genero la factura!"));
                        }
                        else
                        {
                            _Invoice.NumeroDEIString = $"{resultado.Sucursal}-{resultado.Caja}-01-{resultado.NumeroDEI.ToString().PadLeft(8, '0')} ";
                        }

                    }
                    else
                    {
                        var updateresult = await Update(_Invoice.InvoiceId, _Invoice);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));
            
                throw ex;
            }

            return Json(_Invoice);
        }

        // POST: Invoice/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Invoice>> Insert(InvoiceDTO _Invoice)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Invoice.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Invoice.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Invoice/Insert", _Invoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<InvoiceDTO>(valorrespuesta);
                }
                else
                {
                    string d =  await (result.Content.ReadAsStringAsync());
                    throw  new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw (ex);
                // return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_Invoice);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Invoice }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Invoice>> Update(Int64 id, Invoice _Invoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Invoice/Update", _Invoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_Invoice));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _Invoice }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Invoice>> Delete([FromBody]Invoice _Invoice)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Invoice/Delete", _Invoice);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_Invoice));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _Invoice }, Total = 1 });
        }



        [HttpGet]
        public async Task<ActionResult> SFInvoice(Int32 id)
        {
            try
            {
                InvoiceDTO _invoicedto = new InvoiceDTO { InvoiceId = id, };
                return await Task.Run(()=> View(_invoicedto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }      
           
        }

        public async Task<IActionResult> SFLibroVentas()
        {
            return await Task.Run(() => View());

        }




    }
}
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
    public class InvoiceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public InvoiceController(ILogger<InvoiceController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Cuentas por Cobrar.Factura")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
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
                    _Invoice = new InvoiceDTO
                    { OrderDate = DateTime.Now,
                        DeliveryDate = DateTime.Now,
                        ExpirationDate = DateTime.Now.AddDays(30),
                        BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId")),
                        editar = 1,
                        DiasVencimiento =30
                        
                    };

                }
                ViewData["permisos"] = _principal;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Invoice);

        }

        [HttpGet]
        public ActionResult SFReporteInvoice()
        {
            return View();
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


        //[HttpGet]
        public async Task<DataSourceResult> GetFacturasByCustomer([DataSourceRequest] DataSourceRequest request, int CustomerId)
        {
            List<Invoice> _Invoice = new List<Invoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetFacturasByCustomer/"+CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<List<Invoice>>(valorrespuesta);
                    _Invoice = _Invoice.OrderByDescending(q => q.InvoiceId).ToList();
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _Invoice.ToDataSourceResult(request);

        }
        



        public async Task<DataSourceResult> GetFacturasPendientesPagoByCustomer([DataSourceRequest] DataSourceRequest request, int CustomerId)
        {
            List<DocumentoDTO> _Invoice = new List<DocumentoDTO>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetFacturasPendientesPagoByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<List<DocumentoDTO>>(valorrespuesta);
                    _Invoice = (from c in _Invoice
                                select new DocumentoDTO{
                                    DocumentoId = (int)c.DocumentoId,
                                     CustomerId= (int)c.CustomerId, 
                                     DocumentType = c.DocumentType,
                                     DocumentTypeId= c.DocumentTypeId,
                                     Identificador = new Identificador {
                                        Id = c.DocumentoId,
                                        Tipo = c.DocumentTypeId,
                                     },

                                    NumeroDEI = $"{c.NumeroDEI} - {c.ProductName} - {(c.Saldo + c.SaldoImpuesto).ToString("C2")}",

                                }).OrderByDescending(q => q.DocumentoId).ToList();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _Invoice.ToDataSourceResult(request);

        }
        


        public async Task<DataSourceResult> GetFacturasPendientesVigentesByCustomer([DataSourceRequest] DataSourceRequest request, int CustomerId)
        {
            List<Invoice> _Invoice = new List<Invoice>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetFacturasPendientesVigentesByCustomer/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<List<Invoice>>(valorrespuesta);
                    _Invoice = (from c in _Invoice
                                select new Invoice
                                {
                                    InvoiceId = (int)c.InvoiceId,
                                    CustomerId = (int)c.CustomerId,
                                    // = c.DocumentType,
                                    //DocumentTypeId = c.DocumentTypeId,
                                    

                                    NumeroDEI = $"{c.NumeroDEI} - {c.ProductName} - {(c.Saldo + c.SaldoImpuesto).ToString("C2")}",

                                }).OrderByDescending(q => q.NumeroDEI).ToList();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }

            DataSourceResult dataSource= new DataSourceResult();
            dataSource = _Invoice.ToDataSourceResult(request);
            dataSource.Data = _Invoice;
            return dataSource;

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


        public async Task<ActionResult<GoodsDeliveryAuthorization>> Aprobar([FromBody] Invoice invoice)
        {
            try
            {
                if (invoice == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Invoice/ChangeInvoiceStatus/{invoice.InvoiceId}/{2}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo el documento!"));
                }

                return await Task.Run(() => Json(goodsDeliveryAuthorization));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


        }

        public async Task<ActionResult<GoodsDeliveryAuthorization>> Revisar([FromBody] Invoice invoice)
        {
            try
            {
                if (invoice == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Invoice/ChangeInvoiceStatus/{invoice.InvoiceId}/{1}");
                string valorrespuesta = "";
                if (!result.IsSuccessStatusCode)
                {
                    return await Task.Run(() => BadRequest("No se Aprobo el documento!"));
                }

                return await Task.Run(() => Json(goodsDeliveryAuthorization));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


        }



        public async Task<ActionResult> GenerarFactura([FromBody] Invoice invoice)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            Invoice _Invoice = new Invoice();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Invoice/GenerarFactura/{invoice.InvoiceId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);

                }
                else
                {
                    throw new Exception(await (result.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest(ex.Message);
            }

            return Json(_Invoice);
        }

        public async Task<ActionResult> AnularFactura([FromBody] Invoice invoice)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            Invoice _Invoice = new Invoice();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Invoice/AnularFactura/{invoice.InvoiceId}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);

                }
                else
                {
                    throw new Exception(await (result.Content.ReadAsStringAsync()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                return BadRequest(ex.Message);
            }

            return Json(_Invoice);
        }



        [HttpPost("[action]")]
        public async Task<ActionResult<InvoiceDTO>> SaveInvoice([FromBody]InvoiceDTO _Invoice)
        {

            try
            {
                if (_Invoice == null)
                {
                    return BadRequest("No llego correctamente el modelo");
                }

                Invoice _listInvoice = new Invoice();
                string baseadress = config.Value.urlbase;
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
                    if ((insertresult.Result is BadRequestObjectResult))
                    {
                        return await Task.Run(() => BadRequest(insertresult.Result));
                    }

                }
                else
                {
                    var updateresult = await Update(_Invoice.InvoiceId, _Invoice);
                    if ((updateresult.Result is BadRequestObjectResult))
                    {
                        return await Task.Run(() => BadRequest(updateresult.Result));
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"No se genero la factura : {ex.ToString()}"));
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
                    //throw  new Exception(d);
                    return await Task.Run(() => BadRequest($"{d}"));
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
                string baseadress = config.Value.urlbase;
                Invoice _Invoice = new Invoice();
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Invoice/GetInvoiceById/" + id);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Invoice = JsonConvert.DeserializeObject<Invoice>(valorrespuesta);
                    if (_Invoice.Impreso == null)
                    {
                        _Invoice.Impreso = "0";
                    }
                    else if (_Invoice.Impreso == "0")
                    {
                        _Invoice.Impreso = "1";
                    }

                    var updateresult = await Update(_Invoice.InvoiceId, _Invoice);
                    
                }

                

                



                return await Task.Run(()=> View(_invoicedto));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }      
           
        }

        [HttpGet]
        public async Task<ActionResult> SFInvoiceProforma(Int32 id)
        {
            try
            {
                InvoiceDTO _invoicedto = new InvoiceDTO { InvoiceId = id, };
                return await Task.Run(() => View(_invoicedto));
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



        public async Task<ActionResult> InvoiceCustomer()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => PartialView());
        }



        [HttpGet]
        public async Task<DataSourceResult> GetByCustomer([DataSourceRequest]DataSourceRequest request, Int32 CustomerId)
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
                    _Invoice = _Invoice.Where(x => x.CustomerId ==  CustomerId).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Invoice.ToDataSourceResult(request);

        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
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
    public class InvoicePaymentsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public InvoicePaymentsController(ILogger<InvoicePaymentsController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Cuentas por Cobrar.Recibos de Pago")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> pvwInvoicePayments([FromBody]InvoicePayments _InvoicePaymentsp)
        {
            InvoicePayments _InvoicePayments = new InvoicePayments();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoicePayments/GetInvoicePaymentsById/" + _InvoicePaymentsp.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);
                   
                }

                if (_InvoicePayments == null)
                {
                    _InvoicePayments = new InvoicePayments
                    {   FechaPago = DateTime.Now,
                        MontoAdeudado = 0,
                        MontoAdeudaPrevio = 0,
                        MontoPagado = 0,
                        InvoicePaymentsLines = new List<InvoicePaymentsLine>()
                        
                    };

                }
                ViewData["permisos"] = _principal;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_InvoicePayments);

        }


        public async Task<ActionResult> AnularRecibo([FromBody] InvoicePayments invoice)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            Invoice _Invoice = new Invoice();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/InvoicePayments/AnularRecibo/{invoice.Id}");
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




        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InvoicePayments> _InvoicePayments = new List<InvoicePayments>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoicePayments/GetInvoicePayments");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<List<InvoicePayments>>(valorrespuesta);
                    _InvoicePayments = _InvoicePayments.OrderByDescending(q => q.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InvoicePayments.ToDataSourceResult(request);

        }


        //[HttpGet]
        public async Task<DataSourceResult> GetPagosByCustomer([DataSourceRequest] DataSourceRequest request, int CustomerId)
        {
            List<InvoicePayments> _InvoicePayments = new List<InvoicePayments>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoicePayments/GetPagosByCustomer/"+CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<List<InvoicePayments>>(valorrespuesta);
                    _InvoicePayments = _InvoicePayments.OrderByDescending(q => q.Id).ToList();
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


            return _InvoicePayments.ToDataSourceResult(request);

        }




        public async Task<DataSourceResult> GetInvoicePaymentsLineByInvoiceId([DataSourceRequest] DataSourceRequest request, InvoicePaymentsDTO invoicePayments)
        {
            List<InvoicePaymentsLine> _InvoicePayments = new List<InvoicePaymentsLine>();
            try
            {
                if (invoicePayments  == null)
                {
                    return null ;
                }
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                HttpResponseMessage result;
                if (invoicePayments.Id == 0)
                {
                     result = await _client.PostAsJsonAsync(baseadress + $"api/InvoicePayments/GetDetalletInvoiceById/",invoicePayments.Facturas);
                }
                else
                {
                    result = await _client.GetAsync(baseadress + $"api/InvoicePayments/GetDetalletInvoicePaymentById/{invoicePayments.Id}");
                }
                
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<List<InvoicePaymentsLine>>(valorrespuesta);
                    _InvoicePayments = _InvoicePayments.OrderByDescending(q => q.Id).ToList();
                }
                else
                {
                    return _InvoicePayments.ToDataSourceResult(request);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: {ex.ToString()}");
                throw ex;
            }


            return _InvoicePayments.ToDataSourceResult(request);

        }
        


        [HttpPost]
        public async Task<ActionResult> GetInvoicePaymentsById([FromBody]InvoicePayments _InvoicePaymentsp)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            InvoicePayments _InvoicePayments = new InvoicePayments();
            try
            {

                //GoodsDelivered _GoodsDeliveredp = JsonConvert.DeserializeObject<GoodsDelivered>(dto);
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/InvoicePayments/GetInvoicePaymentsLineById", _InvoicePaymentsp);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);

                }

                if (_InvoicePayments == null)
                {
                    _InvoicePayments = new InvoicePayments();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InvoicePayments);
        }
        


        public async Task<ActionResult<GoodsDeliveryAuthorization>> Aprobar([FromBody] InvoicePayments InvoicePayments)
        {
            try
            {
                if (InvoicePayments == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/InvoicePayments/ChangeInvoicePaymentsStatus/{InvoicePayments.Id}/{2}");
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

        public async Task<ActionResult<GoodsDeliveryAuthorization>> Revisar([FromBody] InvoicePayments InvoicePayments)
        {
            try
            {
                if (InvoicePayments == null)
                {
                    return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
                }

                GoodsDeliveryAuthorization goodsDeliveryAuthorization = new GoodsDeliveryAuthorization();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/InvoicePayments/ChangeInvoicePaymentsStatus/{InvoicePayments.Id}/{1}");
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



        public async Task<ActionResult> GenerarFactura([FromBody] InvoicePayments InvoicePayments)
        //public async Task<ActionResult> GetGoodsDeliveredById([FromBody]dynamic dto)
        {
            InvoicePayments _InvoicePayments = new InvoicePayments();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/InvoicePayments/GenerarFactura/{InvoicePayments.Id}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);

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

            return Json(_InvoicePayments);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<InvoicePayments>> SaveInvoicePayments([FromBody]InvoicePayments _InvoicePayments)
        {

            try
            {
                if (_InvoicePayments == null)
                {
                    return BadRequest("No llego correctamente el modelo");
                }

                InvoicePayments _listInvoicePayments = new InvoicePayments();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoicePayments/GetInvoicePaymentsById/" + _InvoicePayments.Id);
                string valorrespuesta = "";
                _InvoicePayments.FechaModificacion = DateTime.Now;
                _InvoicePayments.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);
                }

                if (_listInvoicePayments == null) { _listInvoicePayments = new InvoicePayments(); }

                if (_listInvoicePayments.Id == 0)
                {
                    _InvoicePayments.FechaCreacion = DateTime.Now;
                    _InvoicePayments.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InvoicePayments);
                    if ((insertresult.Result is BadRequestObjectResult))
                    {
                        return await Task.Run(() => BadRequest(insertresult.Result));
                    }

                }
                else
                {
                    var updateresult = await Update(_InvoicePayments.Id, _InvoicePayments);
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

            return Json(_InvoicePayments);
        }

        // POST: InvoicePayments/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InvoicePayments>> Insert(InvoicePayments _InvoicePayments)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _InvoicePayments.UsuarioCreacion = HttpContext.Session.GetString("user");
                _InvoicePayments.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InvoicePayments/Insert", _InvoicePayments);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);
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
            return Ok(_InvoicePayments);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InvoicePayments }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoicePayments>> Update(Int64 id, InvoicePayments _InvoicePayments)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InvoicePayments/Update", _InvoicePayments);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_InvoicePayments));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _InvoicePayments }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InvoicePayments>> Delete([FromBody]InvoicePayments _InvoicePayments)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InvoicePayments/Delete", _InvoicePayments);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<InvoicePayments>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(()=> BadRequest($"Ocurrio un error: {ex.Message}"));
            }


            return await Task.Run(() => Ok(_InvoicePayments));
           // return new ObjectResult(new DataSourceResult { Data = new[] { _InvoicePayments }, Total = 1 });
        }



        [HttpGet]
        public async Task<ActionResult> SFInvoicePayments(Int32 id)
        {
            try
            {
                InvoicePayments _InvoicePayments = new InvoicePayments { Id = id, };
                return await Task.Run(()=> View(_InvoicePayments));
            }
            catch (Exception)
            {

                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }      
           
        }

        [HttpGet]
        public async Task<ActionResult> SFInvoicePaymentsProforma(Int32 id)
        {
            try
            {
                InvoicePayments _InvoicePayments = new InvoicePayments { Id = id, };
                return await Task.Run(() => View(_InvoicePayments));
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



        public async Task<ActionResult> InvoicePaymentsCustomer()
        {
            ViewData["permisos"] = _principal;
            return await Task.Run(() => PartialView());
        }



        [HttpGet]
        public async Task<DataSourceResult> GetByCustomer([DataSourceRequest]DataSourceRequest request, Int32 CustomerId)
        {
            List<InvoicePayments> _InvoicePayments = new List<InvoicePayments>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InvoicePayments/GetInvoicePayments");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoicePayments = JsonConvert.DeserializeObject<List<InvoicePayments>>(valorrespuesta);
                    _InvoicePayments = _InvoicePayments.OrderByDescending(q => q.Id).ToList();
                    _InvoicePayments = _InvoicePayments.Where(x => x.CustomerId ==  CustomerId).ToList();

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InvoicePayments.ToDataSourceResult(request);

        }




    }
}
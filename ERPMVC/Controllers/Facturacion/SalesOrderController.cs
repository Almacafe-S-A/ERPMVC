﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ERPMVC.DTO;
using ERPMVC.Helpers;
using ERPMVC.Models;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentEmail.Razor;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static ERPMVC.Helpers.ViewRenderService;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class SalesOrderController : Controller
    {
        //private readonly ApplicationDbContext _context;
        //  private readonly ILogger _logger;
        private readonly IOptions<MyConfig> _config;
        private readonly IMapper mapper;
        private readonly ILogger _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly ViewRender view;
        private readonly ClaimsPrincipal _principal;

        //public SalesOrderController(ILogger<SalesOrderController> logger,IOptions<MyConfig> config)
        public SalesOrderController(ILogger<SalesOrderController> logger, IOptions<MyConfig> config
            , IMapper mapper, IViewRenderService viewRenderService
            , ViewRender view, IHttpContextAccessor httpContextAccessor
            )
        {
            _principal = httpContextAccessor.HttpContext.User;
            this.mapper = mapper;
            this._logger = logger;
            this._config = config;
            this._viewRenderService = viewRenderService;
            this.view = view;
        }

        [Authorize(Policy = "Clientes.Cotizaciones")]
        [CustomAuthorization]
        public IActionResult Index()
        {
            // SalesOrderDTO _dto = new SalesOrderDTO();

            try
            {
                ViewData["permisoAprobar"] = _principal.HasClaim("Clientes.Cotizaciones", "true");
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwSalesOrder([FromBody]SalesOrderDTO _salesorder)
        {
            SalesOrderDTO _salesorderf = new SalesOrderDTO();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();


                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetById/" + _salesorder.SalesOrderId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _salesorderf = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

                if (_salesorderf == null)
                {
                    _salesorderf = new SalesOrderDTO
                    {
                        ExpirationDate = DateTime.Now.AddDays(30),
                       
                        OrderDate = DateTime.Now,                       
                        editar = _salesorder.editar,
                        IdEstado = 8 ,
                        SalesOrderId = _salesorder.SalesOrderId,
                        BranchId = Convert.ToInt32(HttpContext.Session.GetString("BranchId"))
                    };
                }
                _salesorderf.editar = _salesorder.editar;
                ViewData["permisos"] = _principal;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return View(_salesorderf);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrder([DataSourceRequest]DataSourceRequest request)
        {
            List<SalesOrder> _SalesOrders = new List<SalesOrder>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrder");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(valorrespuesta);
                    _SalesOrders = _SalesOrders.OrderByDescending(q => q.SalesOrderId).ToList();
                    DateTime fecAcutal = DateTime.Now;

                    List<SalesOrder> ListaEstado = new List<SalesOrder>();

                    foreach (var itemss in _SalesOrders)
                    {

                        if (itemss.ExpirationDate < fecAcutal)
                        {
                            itemss.Estado = "Vencida";
                        }
                        else
                        {

                        }
                        ListaEstado.Add(itemss);

                    }
                    _SalesOrders = ListaEstado;
                }
                //else if(result.StatusCode== 401)
                //{

                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _SalesOrders.ToDataSourceResult(request);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrderByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<SalesOrder> _SalesOrders = new List<SalesOrder>();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderByCustomerId/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }


            return _SalesOrders.ToDataSourceResult(request);
        }

        public async Task<ActionResult> GetSalesOrderById(Int64 Id)
        {
            SalesOrder _ControlPallets = new SalesOrder();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPallets = JsonConvert.DeserializeObject<SalesOrder>(valorrespuesta);

                }

                if (_ControlPallets == null)
                {
                    _ControlPallets = new SalesOrder();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPallets);
        }



        public async Task<ActionResult> EnviarCotizacionA([DataSourceRequest]DataSourceRequest request, SalesOrderDTO _SalesOrderDTO)
        {

            try
            {
                SalesOrderDTO _salesorderf = new SalesOrderDTO();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetById/" + _SalesOrderDTO.SalesOrderId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _salesorderf = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

                _salesorderf.IdEstado = _SalesOrderDTO.IdEstado;

                var resultado = await Update(_SalesOrderDTO.SalesOrderId, _salesorderf);
                var value = (resultado.Result as ObjectResult).Value;
                SalesOrder _result = (SalesOrder)value;

                switch (_salesorderf.IdEstado)
                {
                    //Factura proforma
                    case 3:

                        break;
                    //Factura fiscal
                    case 4:
                    case 6:
                        break;


                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return RedirectToAction("", "");
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<SalesOrderDTO>> Anular([FromBody]SalesOrderDTO _SalesOrder)
        {
            SalesOrderDTO _so = new SalesOrderDTO();
            if (_SalesOrder != null)
            {
                try
                {
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _SalesOrder.SalesOrderId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                        _so.IdEstado = 7;
                        _so.Estado = "Rechazado";
                        _so.Observacion = _SalesOrder.Observacion;
                        var resultsalesorder = await Update(_so.SalesOrderId, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        SalesOrder resultado = ((SalesOrder)(value));
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
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }

            return await Task.Run(() => Ok(_so));
        }
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<SalesOrderDTO>> Vencida([FromBody]SalesOrderDTO _SalesOrder)
        {
            SalesOrderDTO _so = new SalesOrderDTO();
            if (_SalesOrder != null)
            {
                try
                {
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _SalesOrder.SalesOrderId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);


                        //  _SalesOrder.UsuarioCreacion = HttpContext.Session.GetString("user");
                        // _SalesOrder.UsuarioModificacion = HttpContext.Session.GetString("user");
                        _so.IdEstado = 62;
                        _so.Estado = "Vencida";
                        var resultsalesorder = await Update(_so.SalesOrderId, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        SalesOrder resultado = ((SalesOrder)(value));
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
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }

            return await Task.Run(() => Ok(_so));
        }
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<SalesOrderDTO>> Aprobar([FromBody]SalesOrderDTO _SalesOrder)
        {
            SalesOrderDTO _so = new SalesOrderDTO();
            if (_SalesOrder != null)
            {
                try
                {
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _SalesOrder.SalesOrderId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);


                        //  _SalesOrder.UsuarioCreacion = HttpContext.Session.GetString("user");
                        // _SalesOrder.UsuarioModificacion = HttpContext.Session.GetString("user");
                        _so.IdEstado = 6;
                        _so.Estado = "Aprobado";
                        var resultsalesorder = await Update(_so.SalesOrderId, _so);
                        if (resultsalesorder.Result is BadRequestObjectResult)
                        {
                            return BadRequest(((BadRequestObjectResult)resultsalesorder.Result).Value);
                        }

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        SalesOrder resultado = ((SalesOrder)(value));
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
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }

            return await Task.Run(() => Ok(_so));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrder>> SaveSalesOrder([FromBody]SalesOrderDTO _SalesOrder)
        {
            if (_SalesOrder == null)
            {
                Console.WriteLine("No llego Correctamente el Modelo");
                return await Task.Run(() => BadRequest("Error al guardar"));
            }
          
            SalesOrder _SalesOrdermodel = new SalesOrder();
                try
                {
                    _SalesOrder.CustomerId = _SalesOrder.CustomerId == null ? 0 : _SalesOrder.CustomerId;
                    _SalesOrdermodel = mapper.Map<SalesOrderDTO, SalesOrder>(_SalesOrder);
                    if (_SalesOrder.SalesOrderId == 0)
                    {
                        _SalesOrder.UsuarioCreacion = HttpContext.Session.GetString("user");
                        _SalesOrder.UsuarioModificacion = HttpContext.Session.GetString("user");
                        var resultsalesorder = await Insert(_SalesOrder);
                        if (resultsalesorder.Result is BadRequestObjectResult)
                        {
                            return BadRequest(((BadRequestObjectResult)resultsalesorder.Result).Value);
                        }
                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        SalesOrder resultado = ((SalesOrder)(value));
                    
                    if (resultado.SalesOrderId > 0)
                        {

                            if (_SalesOrder.IdEstado == 7 || _SalesOrder.IdEstado==5)
                            {

                                string baseadress = _config.Value.urlbase;
                                HttpClient _client = new HttpClient();
                                Alert _alert = new Alert
                                {
                                    AlertName = "Sancionados",
                                    DescriptionAlert = "Listados sancionados,Nombre de persona que intenta ingresar al sistema " +
                                    " de planilla se encuentra en lista OFAC,  Informacion Mediatica, ONU. ",                                      
                                   
                                    UsuarioCreacion = _SalesOrder.UsuarioCreacion,
                                    UsuarioModificacion = _SalesOrder.UsuarioModificacion,
                                    Code   = "PERSON002",
                                    Type = _SalesOrder.listadossancionados,
                                    DocumentId = resultado.SalesOrderId,
                                    DocumentName = "COT",                                     
                                    FechaCreacion = DateTime.Now,
                                    FechaModificacion = DateTime.Now
                                   
                                };
                                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                                var result = await _client.PostAsJsonAsync(baseadress + "api/Alert/Insert", _alert);
                                string valorrespuesta = "";
                                if (result.IsSuccessStatusCode)
                                {
                                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                                    _alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                                }

                                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                                SmtpClient smtp = new SmtpClient();

                                var client = new SmtpClient();
                                client.UseDefaultCredentials = false;
                                client.Credentials = new NetworkCredential(_config.Value.emailsender, _config.Value.passwordsmtp);
                                client.Host = _config.Value.smtp;
                                client.Port = Convert.ToInt32(_config.Value.port);
                                client.EnableSsl = true;
                                Email.DefaultSender = new SmtpSender(client);                             


                                string completepath = Directory.GetCurrentDirectory() + "/Views/Shared/Page1.cshtml";
                                string url = this.Request.Scheme + "://" + this.Request.Host.Value + Url.Action("Index", "SalesOrder");
                                Email.DefaultRenderer = new RazorRenderer();
                                //var template = "Dear @Model.Name, You are totally @Model.Compliment. Ya que el nombre se encontro en los listados";
                                var email = Email
                                    .From(_config.Value.emailsender)
                                     .To("ccastillo@dev-agiles.com")
                                     .Subject($"La cotización {resultado.SalesOrderId} fue {_SalesOrder.Estado} " +
                                              $"por {_SalesOrder.UsuarioCreacion} !")
                                            .Body(url)
                                     .UsingTemplateFromFile($"{completepath}", new
                                     {
                                         Title = "Cotización a aprobar",
                                         DocumentId = resultado.SalesOrderId.ToString(),
                                         url = url,
                                         Fecha = DateTime.Now.ToString("dd/MM/yyyy") + " © Desarrollos Agiles.Todos los derechos reservados.",
                                     })
                                
                                    .SendAsync();



                                //await email.SendAsync();
                            }


                        }
                        else
                        {
                            return await Task.Run(() => BadRequest("No se genero la cotización!"));
                        }

                        return resultsalesorder;
                    }
                    else
                    {
                            var resultsalesorder =  await Update(_SalesOrder.SalesOrderId, _SalesOrder);
                            if (resultsalesorder.Result is BadRequestObjectResult)
                            {
                                return BadRequest(((BadRequestObjectResult)resultsalesorder.Result).Value);
                            }
                            return Ok(resultsalesorder);
                            
                }
                }

                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    //throw ex;
                }
            return Ok();
            
        }



        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContract>> GenerarContrato([FromBody]SalesOrderDTO _SalesOrderp)
        //  public async Task<ActionResult<SalesOrder>> GenerarContrato([FromBody]dynamic dto)
        {

            //     _SalesOrder = JsonConvert.DeserializeObject<SalesOrderDTO>(dto.ToString());           
            //SalesOrder _SalesOrdermodel = new SalesOrder();
            CustomerContract customerContract = new CustomerContract();
            if (_SalesOrderp != null)
            {
                try
                {

                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CustomerContract/GenerarContrato/" + _SalesOrderp.SalesOrderId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        customerContract = JsonConvert.DeserializeObject<CustomerContract>(valorrespuesta);
                        return await Task.Run(() => Json(customerContract));
                    }
                    else
                    {
                        return BadRequest();
                    }

                    CustomerContract _customercontract = new CustomerContract();
                    _customercontract.SalesOrderId = _SalesOrderp.SalesOrderId;              
                  


               


                    return await Task.Run(() => Json(customerContract));
                }

                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    return await Task.Run(() => BadRequest($"{ex.ToString()}!"));
                }

            }
            else
            {
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }
        }



        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrder>> Insert(SalesOrder _SalesOrder)
        {
            try
            {
                if (_SalesOrder.SalesOrderId == 0)
                {
                    // TODO: Add insert logic here
                    string baseadress = _config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _SalesOrder.FechaCreacion = DateTime.Now;
                    _SalesOrder.UsuarioCreacion = HttpContext.Session.GetString("user");
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Insert", _SalesOrder);

                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _SalesOrder = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);

                    }
                    else
                    {
                        string request = await result.Content.ReadAsStringAsync();
                        return BadRequest(request);
                    }


                }
                else
                {
                    return BadRequest("Ya existe!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_SalesOrder);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _role }, Total = 1 });
        }


        [HttpPut("[action]")]
        public async Task<ActionResult<SalesOrder>> Update(Int64 Id, SalesOrderDTO _SalesOrderLine)
        {
            try
            {

                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Update", _SalesOrderLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrderLine = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                    return Ok(_SalesOrderLine);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    return BadRequest(valorrespuesta);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            // return new ObjectResult(new DataSourceResult { Data = new[] { _SalesOrderLine }, Total = 1 });
           
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrderDTO>> Delete([FromBody]SalesOrderDTO _rol)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Delete", _rol);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _rol = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _rol }, Total = 1 });
        }



        // [HttpGet("{SalesOrderId}")]
        //public  ActionResult AR(Int32 SalesOrderId)
        // {

        //     SalesOrderDTO _salesorderdto = new SalesOrderDTO { SalesOrderId = SalesOrderId, token = HttpContext.Session.GetString("token") };

        //     return View(_salesorderdto);
        // }


        // [HttpGet("[controller]/[action]/{SalesOrderId}")]
        //    [HttpGet("{SalesOrderId}")]
        [HttpGet]
        public async Task<ActionResult> SFCotizacion(Int32 id)
        {
            SalesOrder _salesorderf = new SalesOrder();
            string baseadress = _config.Value.urlbase;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetById/" + id);
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await(result.Content.ReadAsStringAsync());
                _salesorderf = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);
            }
            //SalesOrderDTO _salesorderdto = new SalesOrderDTO { SalesOrderId = id, }; //token = HttpContext.Session.GetString("token") };

            return View(_salesorderf);
        }


        public async Task<ActionResult> Virtualization_ReadByCustomer([DataSourceRequest] DataSourceRequest request, Customer _customer)
        {
            var res = await GetSalesOrderByCustomer(_customer);
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapperByCustomer(Int64[] values,Customer customer)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetSalesOrderByCustomer(customer))
                {
                    if (values.Contains(order.SalesOrderId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<SalesOrder>> GetSalesOrderByCustomer(Customer _customer)
        {
            List<SalesOrder> _SalesOrder = new List<SalesOrder>();

            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderByCustomerId/"+ _customer.CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrder = JsonConvert.DeserializeObject<List<SalesOrder>>(valorrespuesta);
                    _SalesOrder = (from c in _SalesOrder
                                   .Where(q=>q.CustomerId == _customer.CustomerId)
                                   select new SalesOrder
                                   {
                                       RTN = c.RTN,
                                       SalesOrderId = c.SalesOrderId,
                                       SalesOrderName = "Id:" + c.SalesOrderId + "|| Nombre:" + c.SalesOrderName + "|| Fecha:" + c.OrderDate ,
                                       OrderDate = c.OrderDate,
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _SalesOrder;
        }




        public async Task<ActionResult> Virtualization_Read([DataSourceRequest] DataSourceRequest request )
        {
            var res = await GetSalesOrder();
            return Json(res.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Orders_ValueMapper(Int64[] values)
        {
            var indices = new List<Int64>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in await GetSalesOrder())
                {
                    if (values.Contains(order.SalesOrderId))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices);
        }

        private async Task<List<SalesOrder>> GetSalesOrder()
        {
            List<SalesOrder> _SalesOrder = new List<SalesOrder>();

            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrder");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SalesOrder = JsonConvert.DeserializeObject<List<SalesOrder>>(valorrespuesta);
                    _SalesOrder = (from c in _SalesOrder
                                   select new SalesOrder
                                   {
                                       RTN = c.RTN,
                                       SalesOrderId = c.SalesOrderId,
                                       SalesOrderName = "Id:" + c.SalesOrderId + "|| Nombre:" + c.SalesOrderName + "|| Fecha:" + c.OrderDate ,
                                       OrderDate = c.OrderDate,
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _SalesOrder;
        }





    }
}
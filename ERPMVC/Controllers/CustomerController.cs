using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ERPMVC.DTO;

namespace ERPMVC.Controllers
{

    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CustomerController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;

        public CustomerController(ILogger<CustomerController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        // GET: Customer
        [Authorize(Policy = "Clientes.Datos de Clientes")]
        public async Task<ActionResult> Index()
        {
            ViewData["CustomerType"] = await CustomerTypeList();

            return await Task.Run(() => View());
        }




        public async Task<ActionResult> SalesOrderCustomer()
        {
            return await Task.Run(() => PartialView());
        }

        public ActionResult CustomerProduct()
        {
            return PartialView();
        }


        public async Task<ActionResult> CertificadoDepositoCustomer()
        {
            return await Task.Run(() => PartialView());
        }

        public async Task<ActionResult> ProformaInvoiceCustomer()
        {
            return await Task.Run(() => PartialView());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetCustomerById(Int64 CustomerId)
        {
            CustomerDTO _customers = new CustomerDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + CustomerId);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<CustomerDTO>(valorrespuesta);

                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/ProformaInvoice/GetLastProformaInvoice/" + CustomerId);
                    if(result.IsSuccessStatusCode)
                    {
                        valorrespuesta = "";
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        ProformaInvoice _proforma = JsonConvert.DeserializeObject<ProformaInvoice>(valorrespuesta);
                        if (_proforma !=null)
                        {
                            _customers.EndDate = DateTime.Now;
                            _customers.StartDate = _proforma.OrderDate;
                        }
                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_customers));
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<Customer>> SaveCustomer([FromBody]Customer _Customer)
        {

            try
            {
                Customer _listCustomer = new Customer();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + _Customer.CustomerId);
                string valorrespuesta = "";
                _Customer.FechaModificacion = DateTime.Now;
                _Customer.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);
                }

                if (_listCustomer.CustomerId == 0)
                {
                    _Customer.FechaCreacion = DateTime.Now;
                    _Customer.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Post(_Customer);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _Customer = ((Customer)(value));
                    if (_Customer.CustomerId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }
                }
                else
                {
                    _Customer.UsuarioCreacion = _listCustomer.UsuarioCreacion == "" ? HttpContext.Session.GetString("user") : _listCustomer.UsuarioCreacion;
                    _Customer.FechaCreacion = _listCustomer.FechaCreacion==null?DateTime.Now: _listCustomer.FechaCreacion;

                    var updateresult = await Put(_Customer,1);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Customer);
        }


        //[Authorize(Policy = "Cliente")]
        // GET: Customer/Details/5
        public async Task<ActionResult> Details(Int64 CustomerId)
        {
            Customer _customers = new Customer();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Customer>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_customers));
        }

        // GET: Customer/Create
        public async Task<ActionResult> Create()
        {
            return await Task.Run(() => View());
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Customer> _customers = new List<Customer>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Customer>>(valorrespuesta);
                    _customers = _customers.OrderByDescending(q => q.CustomerId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => _customers.ToDataSourceResult(request));

        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetUsuario([DataSourceRequest]DataSourceRequest request)
        {
            List<Customer> _customers = new List<Customer>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Customer>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => _customers.ToDataSourceResult(request));

        }


        [HttpPost("[action]")]
        public async Task<ActionResult<SalesOrder>> InsertCustomerFromSalesOrder([FromBody]SalesOrderDTO _SalesOrder)
        {
            SalesOrderDTO _so = new SalesOrderDTO();
            Customer _customer = new Customer();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SalesOrder/GetSalesOrderById/" + _SalesOrder.SalesOrderId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _so = JsonConvert.DeserializeObject<SalesOrderDTO>(valorrespuesta);

                    _customer.CustomerName = _so.SalesOrderName;
                    _customer.RTN = _so.RTN;
                    _customer.Phone = _so.Tefono;
                    _customer.Identidad = _so.RTN;
                    _customer.Email = _so.Correo;
                    //_customer. = _so.SalesOrderName;

                    string json = JsonConvert.SerializeObject(_customer);
                    var resultsalesorder = await Post(_customer);
                    var value = (resultsalesorder.Result as ObjectResult).Value;
                    _customer = ((Customer)(value));


                    _so.CustomerId = _customer.CustomerId;
                    _so.CustomerName = _customer.CustomerName;
                    _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.PostAsJsonAsync(baseadress + "api/SalesOrder/Update", _so);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {

                    }
                    else
                    {
                        return await Task.Run(() => BadRequest($"La actualización de la cotización no se hizo correctamente "));

                    }

                }



            }
            catch (Exception ex)
            {
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
                throw ex;
            }

            return await Task.Run(() => Ok(_customer));
        }


        // POST: Customer/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<Customer>> Post(Customer _customer)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _customer.UsuarioCreacion = HttpContext.Session.GetString("user");
                _customer.UsuarioModificacion = HttpContext.Session.GetString("user");

                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerByRTN/"+ _customer.RTN);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                   
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    Customer _customerq = JsonConvert.DeserializeObject<Customer>(valorrespuesta);
                    if(_customerq==null)
                    {
                        _customerq = new Customer();

                    }


                    if (_customerq.CustomerId > 0)
                    {
                        string error = await result.Content.ReadAsStringAsync();
                        return this.Json(new DataSourceResult
                        {
                            Errors = $"El RTN del cliente ya existe!"
                        });
                    }
                    else
                    {
                        result = await _client.PostAsJsonAsync(baseadress + "api/Customer/Insert", _customer);
                        valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _customer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);

                        }
                        else
                        {
                            return BadRequest($"Ocurrio un error: {result.Content.ReadAsStringAsync()}");
                        }

                    }
                    // return await Task.Run(() => BadRequest("El RTN del cliente ya existe"));
                }
               

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return await Task.Run(() => Ok(_customer));
            // return await Task.Run(() => new ObjectResult(new DataSourceResult { Data = new[] { _customer }, Total = 1 }));
        }

        [HttpPost]
        public async Task<IActionResult> Put(Customer _customer,int fromsave=0)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                Customer _listCustomer = new Customer();
                var result = await _client.GetAsync(baseadress + "api/Customer/GetCustomerById/" + _customer.CustomerId);
                string valorrespuesta = "";

                _customer.FechaModificacion = DateTime.Now;
                _customer.UsuarioModificacion = HttpContext.Session.GetString("user");

                if (fromsave == 0)
                {
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listCustomer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);
                    }

                    _customer.City = _listCustomer.City;
                    _customer.CityId = _listCustomer.CityId;
                    _customer.Address = _listCustomer.Address;
                    _customer.ClienteRecoger = _listCustomer.ClienteRecoger;
                    _customer.CustomerRefNumber = _listCustomer.CustomerRefNumber;
                    _customer.Email = _listCustomer.Email;
                    _customer.EnviarlaMensajero = _listCustomer.EnviarlaMensajero;
                    _customer.IdentidadApoderado = _listCustomer.IdentidadApoderado;
                    _customer.MontoActivos = _listCustomer.MontoActivos;
                    _customer.MontoIngresosAnuales = _listCustomer.MontoIngresosAnuales;
                    _customer.NombreApoderado = _listCustomer.NombreApoderado;
                    _customer.PerteneceEmpresa = _listCustomer.PerteneceEmpresa;
                    _customer.Proveedor1 = _listCustomer.Proveedor1;
                    _customer.Proveedor2 = _listCustomer.Proveedor2;
                    _customer.ZipCode = _listCustomer.ZipCode;
                    _customer.State = _listCustomer.State;
                    _customer.StateId = _listCustomer.StateId;
                    _customer.IdEstado = _listCustomer.IdEstado;
                    _customer.Estado = _listCustomer.Estado;
                    _customer.GrupoEconomico = _listCustomer.GrupoEconomico;
                    _customer.Phone = _listCustomer.Phone;
                    _customer.CountryId = _listCustomer.CountryId;
                    _customer.CountryName = _listCustomer.CountryName;
                }

                _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                result = await _client.PostAsJsonAsync(baseadress + "api/Customer/Update", _customer);
                 valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customer = JsonConvert.DeserializeObject<Customer>(valorrespuesta);

                    _customer.CityId = _customer.CityId == null ? 0 : _customer.CityId;
                    _customer.CountryId = _customer.CountryId == null ? 0 : _customer.CountryId;
                    _customer.StateId = _customer.StateId == null ? 0 : _customer.StateId;
                    _customer.IdEstado = _customer.IdEstado == null ? 0 : _customer.IdEstado;
                    _customer.CustomerTypeId = _customer.CustomerTypeId == null ? 0 : _customer.CustomerTypeId;

                }
                else
                {
                    return BadRequest($"Ocurrio un error: {result.Content.ReadAsStringAsync()}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return await Task.Run(() => new ObjectResult(new DataSourceResult { Data = new[] { _customer }, Total = 1 }));
        }

        // GET: Customer/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Task.Run(() => View());
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return View();
            }
        }

        [HttpDelete]
        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return View();
            }
        }


        async Task<IEnumerable<CustomerType>> CustomerTypeList()
        {
            IEnumerable<CustomerType> CustomerTypeList = null;
            try
            {
                string baseadress = config.Value.urlbase;
               
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerType/Get");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    CustomerTypeList = JsonConvert.DeserializeObject<IEnumerable<CustomerType>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            ViewData["CustomerType"] = CustomerTypeList;
            return CustomerTypeList;

        }


    }
}
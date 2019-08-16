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
    public class CustomerAuthorizedSignatureController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerAuthorizedSignatureController(ILogger<CustomerAuthorizedSignatureController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            // CustomerAuthorizedSignature _customer = new CustomerAuthorizedSignature();
            //_customer.CustomerId = CustomerId;


            return PartialView();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerAuthorizedSignature([FromBody]CustomerAuthorizedSignature _CustomerAuthorizedSignaturep)
        {
            CustomerAuthorizedSignature _CustomerAuthorizedSignature = new CustomerAuthorizedSignature();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerAuthorizedSignature/GetCustomerAuthorizedSignatureById/"
                                + _CustomerAuthorizedSignaturep.CustomerAuthorizedSignatureId);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<CustomerAuthorizedSignature>(valorrespuesta);

                }

                if (_CustomerAuthorizedSignature == null)
                {
                    _CustomerAuthorizedSignature = new CustomerAuthorizedSignature {  CustomerId = _CustomerAuthorizedSignaturep.CustomerId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerAuthorizedSignature);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerAuthorizedSignature> _CustomerAuthorizedSignature = new List<CustomerAuthorizedSignature>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerAuthorizedSignature/GetCustomerAuthorizedSignature");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<List<CustomerAuthorizedSignature>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerAuthorizedSignature.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetSignatureByCustomerId([DataSourceRequest]DataSourceRequest request,Int64 CustomerId)
        {
            List<CustomerAuthorizedSignature> _CustomerAuthorizedSignature = new List<CustomerAuthorizedSignature>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerAuthorizedSignature/GetCustomerAuthorizedSignatureByCustomerId/"+CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<List<CustomerAuthorizedSignature>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerAuthorizedSignature.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerAuthorizedSignature>> SaveCustomerAuthorizedSignature([FromBody]CustomerAuthorizedSignature _CustomerAuthorizedSignature)
        {

            try
            {
                CustomerAuthorizedSignature _listCustomerAuthorizedSignature = new CustomerAuthorizedSignature();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerAuthorizedSignature/GetCustomerAuthorizedSignatureById/" + _CustomerAuthorizedSignature.CustomerAuthorizedSignatureId);
                string valorrespuesta = "";
                _CustomerAuthorizedSignature.FechaModificacion = DateTime.Now;
                _CustomerAuthorizedSignature.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerAuthorizedSignature = JsonConvert.DeserializeObject<CustomerAuthorizedSignature>(valorrespuesta);
                }

                if (_listCustomerAuthorizedSignature == null) { _listCustomerAuthorizedSignature = new CustomerAuthorizedSignature(); }

                if (_listCustomerAuthorizedSignature.CustomerAuthorizedSignatureId == 0)
                {
                    _CustomerAuthorizedSignature.FechaCreacion = DateTime.Now;
                    _CustomerAuthorizedSignature.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerAuthorizedSignature);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _listCustomerAuthorizedSignature = ((CustomerAuthorizedSignature)(value));
                    if (_listCustomerAuthorizedSignature.CustomerAuthorizedSignatureId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }
                    else
                    {
                        Alert _alert = new Alert
                        {
                            AlertName = "Sancionados",
                            DescriptionAlert = "Listados sancionados,Nombre de persona que intenta ingresar al sistema " +
                                 " de planilla se encuentra en lista OFAC,  Informacion Mediatica, ONU. ",

                            UsuarioCreacion = _CustomerAuthorizedSignature.UsuarioCreacion,
                            UsuarioModificacion = _CustomerAuthorizedSignature.UsuarioModificacion,
                            Code = "PERSON002",
                            Type = _CustomerAuthorizedSignature.Listados,
                            DocumentId = _listCustomerAuthorizedSignature.CustomerAuthorizedSignatureId,
                            DocumentName = "FIRMAAUTORIZADA",
                            FechaCreacion = DateTime.Now,
                            FechaModificacion = DateTime.Now

                        };

                        _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        result = await _client.PostAsJsonAsync(baseadress + "api/Alert/Insert", _alert);
                        valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _alert = JsonConvert.DeserializeObject<Alert>(valorrespuesta);
                        }
                    }

                }
                else
                {
                    var updateresult = await Update(_CustomerAuthorizedSignature.CustomerAuthorizedSignatureId, _CustomerAuthorizedSignature);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerAuthorizedSignature);
        }

        // POST: CustomerAuthorizedSignature/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerAuthorizedSignature>> Insert(CustomerAuthorizedSignature _CustomerAuthorizedSignature)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerAuthorizedSignature.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerAuthorizedSignature.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerAuthorizedSignature/Insert", _CustomerAuthorizedSignature);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<CustomerAuthorizedSignature>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerAuthorizedSignature);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerAuthorizedSignature }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerAuthorizedSignature>> Update(Int64 id, CustomerAuthorizedSignature _CustomerAuthorizedSignature)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerAuthorizedSignature/Update", _CustomerAuthorizedSignature);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<CustomerAuthorizedSignature>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerAuthorizedSignature }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerAuthorizedSignature>> Delete([FromBody]CustomerAuthorizedSignature _CustomerAuthorizedSignature)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerAuthorizedSignature/Delete", _CustomerAuthorizedSignature);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerAuthorizedSignature = JsonConvert.DeserializeObject<CustomerAuthorizedSignature>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerAuthorizedSignature }, Total = 1 });
        }





    }
}
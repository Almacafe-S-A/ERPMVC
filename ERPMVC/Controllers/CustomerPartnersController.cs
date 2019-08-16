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
    public class CustomerPartnersController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerPartnersController(ILogger<CustomerPartnersController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerPartner([FromBody]CustomerPartners _CustomerPartnersp)
        {
            CustomerPartners _CustomerPartners = new CustomerPartners();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPartners/GetCustomerPartnersById/" + _CustomerPartnersp.PartnerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<CustomerPartners>(valorrespuesta);

                }

                if (_CustomerPartners == null)
                {
                    _CustomerPartners = new CustomerPartners {  CustomerId = _CustomerPartnersp.CustomerId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerPartners);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerPartners> _CustomerPartners = new List<CustomerPartners>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPartners/GetCustomerPartners");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<List<CustomerPartners>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerPartners.ToDataSourceResult(request);

        }

        [HttpGet]
        public async Task<DataSourceResult> GetCustomerPartnersByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerPartners> _CustomerPartners = new List<CustomerPartners>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPartners/GetCustomerPartnersCustomerId/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<List<CustomerPartners>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerPartners.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerPartners>> SaveCustomerPartners([FromBody]CustomerPartners _CustomerPartners)
        {

            try
            {
                CustomerPartners _listCustomerPartners = new CustomerPartners();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPartners/GetCustomerPartnersById/" + _CustomerPartners.PartnerId);
                string valorrespuesta = "";
                _CustomerPartners.FechaModificacion = DateTime.Now;
                _CustomerPartners.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerPartners = JsonConvert.DeserializeObject<CustomerPartners>(valorrespuesta);
                }

                if (_listCustomerPartners == null) { _listCustomerPartners = new CustomerPartners(); }

                if (_listCustomerPartners.PartnerId == 0)
                {
                    _CustomerPartners.FechaCreacion = DateTime.Now;
                    _CustomerPartners.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerPartners);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _listCustomerPartners = ((CustomerPartners)(value));
                    if (_listCustomerPartners.PartnerId == 0)
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

                            UsuarioCreacion = _CustomerPartners.UsuarioCreacion,
                            UsuarioModificacion = _CustomerPartners.UsuarioModificacion,
                            Code = "PERSON003",
                            Type = _CustomerPartners.Listados,
                            DocumentId = _listCustomerPartners.PartnerId,
                            DocumentName = "SOCIOS",
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
                    var updateresult = await Update(_CustomerPartners.PartnerId, _CustomerPartners);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerPartners);
        }

        // POST: CustomerPartners/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerPartners>> Insert(CustomerPartners _CustomerPartners)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerPartners.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerPartners.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerPartners/Insert", _CustomerPartners);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<CustomerPartners>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerPartners);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPartners }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerPartners>> Update(Int64 id, CustomerPartners _CustomerPartners)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerPartners/Update", _CustomerPartners);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<CustomerPartners>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPartners }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerPartners>> Delete([FromBody]CustomerPartners _CustomerPartners)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerPartners/Delete", _CustomerPartners);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPartners = JsonConvert.DeserializeObject<CustomerPartners>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPartners }, Total = 1 });
        }





    }
}
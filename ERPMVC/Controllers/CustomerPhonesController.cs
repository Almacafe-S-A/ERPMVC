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
    public class CustomerPhonesController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerPhonesController(ILogger<CustomerPhonesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerPhones([FromBody]CustomerPhones _CustomerPhonesp)
        {
            CustomerPhones _CustomerPhones = new CustomerPhones();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPhones/GetCustomerPhonesById/" + _CustomerPhonesp.CustomerPhoneId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<CustomerPhones>(valorrespuesta);

                }

                if (_CustomerPhones == null)
                {
                    _CustomerPhones = new CustomerPhones { CustomerId = _CustomerPhonesp.CustomerId };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerPhones);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetCustomerPhonesByCustomerId([DataSourceRequest]DataSourceRequest request, Int64 CustomerId)
        {
            List<CustomerPhones> _CustomerPhones = new List<CustomerPhones>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPhones/GetCustomerPhonesByCustomerId/" + CustomerId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<List<CustomerPhones>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerPhones.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerPhones> _CustomerPhones = new List<CustomerPhones>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPhones/GetCustomerPhones");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<List<CustomerPhones>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerPhones.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerPhones>> SaveCustomerPhones([FromBody]CustomerPhones _CustomerPhones)
        {

            try
            {
                CustomerPhones _listCustomerPhones = new CustomerPhones();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerPhones/GetCustomerPhonesById/" + _CustomerPhones.CustomerPhoneId);
                string valorrespuesta = "";
                _CustomerPhones.FechaModificacion = DateTime.Now;
                _CustomerPhones.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerPhones = JsonConvert.DeserializeObject<CustomerPhones>(valorrespuesta);
                }

                if (_listCustomerPhones == null) { _listCustomerPhones = new CustomerPhones(); }

                if (_listCustomerPhones.CustomerPhoneId == 0)
                {
                    _CustomerPhones.FechaCreacion = DateTime.Now;
                    _CustomerPhones.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerPhones);
                    var value = (insertresult.Result as ObjectResult).Value;
                    _listCustomerPhones = ((CustomerPhones)(value));
                    if (_listCustomerPhones.CustomerPhoneId == 0)
                    {
                        return await Task.Run(() => BadRequest("No se genero el documento!"));
                    }


                }
                else
                {
                    var updateresult = await Update(_CustomerPhones.CustomerPhoneId, _CustomerPhones);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerPhones);
        }

        // POST: CustomerPhones/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerPhones>> Insert(CustomerPhones _CustomerPhones)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerPhones.Usuariocreacion = HttpContext.Session.GetString("user");
                _CustomerPhones.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerPhones/Insert", _CustomerPhones);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<CustomerPhones>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerPhones);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPhones }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerPhones>> Update(Int64 id, CustomerPhones _CustomerPhones)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerPhones/Update", _CustomerPhones);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<CustomerPhones>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPhones }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerPhones>> Delete([FromBody]CustomerPhones _CustomerPhones)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerPhones/Delete", _CustomerPhones);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerPhones = JsonConvert.DeserializeObject<CustomerPhones>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerPhones }, Total = 1 });
        }





    }
}
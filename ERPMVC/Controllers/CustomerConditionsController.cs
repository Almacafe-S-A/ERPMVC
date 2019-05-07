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
    public class CustomerConditionsController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerConditionsController(ILogger<CustomerConditionsController> logger
            , IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        public async Task<ActionResult> pvwCustomerConditions(Int64 Id = 0)
        {
            CustomerConditions _CustomerConditions = new CustomerConditions();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<CustomerConditions>(valorrespuesta);

                }

                if (_CustomerConditions == null)
                {
                    _CustomerConditions = new CustomerConditions();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerConditions);

        }


        //[HttpPost("[action]")]
        [HttpGet("[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request,CustomerConditions _Ccq)
        {
            List<CustomerConditions> _CustomerConditions = new List<CustomerConditions>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsByClass", _Ccq);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<List<CustomerConditions>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerConditions.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult<CustomerConditions>> SaveCustomerConditions([FromBody]CustomerConditions _CustomerConditions)
        {

            try
            {
                CustomerConditions _listCustomerConditions = new CustomerConditions();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerConditions/GetCustomerConditionsById/" + _CustomerConditions.CustomerConditionId);
                string valorrespuesta = "";
                _CustomerConditions.FechaModificacion = DateTime.Now;
                _CustomerConditions.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerConditions = JsonConvert.DeserializeObject<CustomerConditions>(valorrespuesta);
                }

                if (_listCustomerConditions == null)
                {
                    _listCustomerConditions = new CustomerConditions();
                }

                if (_listCustomerConditions.CustomerConditionId == 0)
                {
                    _CustomerConditions.FechaCreacion = DateTime.Now;
                    _CustomerConditions.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerConditions);

                }
                else
                {
                    var updateresult = await Update(_CustomerConditions.CustomerConditionId, _CustomerConditions);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerConditions);
        }

        // POST: CustomerConditions/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerConditions>> Insert(CustomerConditions _CustomerConditions)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerConditions.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerConditions.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/Insert", _CustomerConditions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<CustomerConditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerConditions }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerConditions>> Update(Int64 id, CustomerConditions _CustomerConditions)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerConditions/Update", _CustomerConditions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<CustomerConditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerConditions }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerConditions>> Delete([FromBody]CustomerConditions _CustomerConditions)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerConditions/Delete", _CustomerConditions);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerConditions = JsonConvert.DeserializeObject<CustomerConditions>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerConditions }, Total = 1 });
        }





    }
}
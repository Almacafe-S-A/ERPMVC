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
    public class CustomerContractWareHouseController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerContractWareHouseController(ILogger<CustomerContractWareHouseController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwCustomerContractWareHouse(Int64 Id = 0)
        {
            CustomerContractWareHouse _CustomerContractWareHouse = new CustomerContractWareHouse();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractWareHouse/GetCustomerContractWareHouseById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractWareHouse = JsonConvert.DeserializeObject<CustomerContractWareHouse>(valorrespuesta);

                }

                if (_CustomerContractWareHouse == null)
                {
                    _CustomerContractWareHouse = new CustomerContractWareHouse();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerContractWareHouse);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerContractWareHouse> _CustomerContractWareHouse = new List<CustomerContractWareHouse>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractWareHouse/GetCustomerContractWareHouse");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractWareHouse = JsonConvert.DeserializeObject<List<CustomerContractWareHouse>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerContractWareHouse.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractWareHouse>> SaveCustomerContractWareHouse([FromBody]CustomerContractWareHouse _CustomerContractWareHouse)
        {

            try
            {
                CustomerContractWareHouse _listCustomerContractWareHouse = new CustomerContractWareHouse();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerContractWareHouse/GetCustomerContractWareHouseById/" + _CustomerContractWareHouse.CustomerContractWareHouseId);
                string valorrespuesta = "";
                _CustomerContractWareHouse.FechaModificacion = DateTime.Now;
                _CustomerContractWareHouse.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerContractWareHouse = JsonConvert.DeserializeObject<CustomerContractWareHouse>(valorrespuesta);
                }

                if (_listCustomerContractWareHouse.CustomerContractWareHouseId == 0)
                {
                    _CustomerContractWareHouse.FechaCreacion = DateTime.Now;
                    _CustomerContractWareHouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerContractWareHouse);
                }
                else
                {
                    var updateresult = await Update(_CustomerContractWareHouse.CustomerContractWareHouseId, _CustomerContractWareHouse);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerContractWareHouse);
        }

        // POST: CustomerContractWareHouse/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerContractWareHouse>> Insert(CustomerContractWareHouse _CustomerContractWareHouse)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerContractWareHouse.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerContractWareHouse.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractWareHouse/Insert", _CustomerContractWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractWareHouse = JsonConvert.DeserializeObject<CustomerContractWareHouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerContractWareHouse);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractWareHouse }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerContractWareHouse>> Update(Int64 id, CustomerContractWareHouse _CustomerContractWareHouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerContractWareHouse/Update", _CustomerContractWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractWareHouse = JsonConvert.DeserializeObject<CustomerContractWareHouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractWareHouse }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerContractWareHouse>> Delete([FromBody]CustomerContractWareHouse _CustomerContractWareHouse)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerContractWareHouse/Delete", _CustomerContractWareHouse);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerContractWareHouse = JsonConvert.DeserializeObject<CustomerContractWareHouse>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerContractWareHouse }, Total = 1 });
        }





    }
}
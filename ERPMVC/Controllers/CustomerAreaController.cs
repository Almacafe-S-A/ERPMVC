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
    public class CustomerAreaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CustomerAreaController(ILogger<CustomerAreaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }


        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwCustomerArea([FromBody]CustomerArea _CustomerAreap)
        {
            CustomerArea _CustomerArea = new CustomerArea();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerArea/GetCustomerAreaById/" + _CustomerAreap.CustomerAreaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerArea = JsonConvert.DeserializeObject<CustomerArea>(valorrespuesta);

                }

                if (_CustomerArea == null)
                {
                    _CustomerArea = new CustomerArea();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CustomerArea);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CustomerArea> _CustomerArea = new List<CustomerArea>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerArea/GetCustomerArea");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerArea = JsonConvert.DeserializeObject<List<CustomerArea>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CustomerArea.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CustomerArea>> SaveCustomerArea([FromBody]CustomerArea _CustomerArea)
        {

            try
            {
                CustomerArea _listCustomerArea = new CustomerArea();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CustomerArea/GetCustomerAreaById/" + _CustomerArea.CustomerAreaId);
                string valorrespuesta = "";
                _CustomerArea.FechaModificacion = DateTime.Now;
                _CustomerArea.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCustomerArea = JsonConvert.DeserializeObject<CustomerArea>(valorrespuesta);
                }

                if (_listCustomerArea == null) { _listCustomerArea = new CustomerArea(); }

                if (_listCustomerArea.CustomerAreaId == 0)
                {
                    _CustomerArea.FechaCreacion = DateTime.Now;
                    _CustomerArea.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CustomerArea);
                }
                else
                {
                  

                    var updateresult = await Update(_CustomerArea.CustomerAreaId, _CustomerArea);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CustomerArea);
        }

        // POST: CustomerArea/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerArea>> Insert(CustomerArea _CustomerArea)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CustomerArea.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CustomerArea.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerArea/Insert", _CustomerArea);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerArea = JsonConvert.DeserializeObject<CustomerArea>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CustomerArea);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerArea }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerArea>> Update(Int64 id, CustomerArea _CustomerArea)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CustomerArea/Update", _CustomerArea);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerArea = JsonConvert.DeserializeObject<CustomerArea>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerArea }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CustomerArea>> Delete([FromBody]CustomerArea _CustomerArea)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CustomerArea/Delete", _CustomerArea);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CustomerArea = JsonConvert.DeserializeObject<CustomerArea>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CustomerArea }, Total = 1 });
        }





    }
}
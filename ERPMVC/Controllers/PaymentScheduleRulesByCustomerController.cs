using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class PaymentScheduleRulesByCustomerController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public PaymentScheduleRulesByCustomerController(ILogger<PaymentScheduleRulesByCustomerController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwPaymentScheduleRulesByCustomer([FromBody]PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomerp)
        {
            PaymentScheduleRulesByCustomerDTO _PaymentScheduleRulesByCustomer = new PaymentScheduleRulesByCustomerDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PaymentScheduleRulesByCustomer/GetPaymentScheduleRulesByCustomerById/" + _PaymentScheduleRulesByCustomerp.ScheduleSubservicesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<PaymentScheduleRulesByCustomerDTO>(valorrespuesta);
                }

                if (_PaymentScheduleRulesByCustomer == null)
                {
                    _PaymentScheduleRulesByCustomer = new PaymentScheduleRulesByCustomerDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_PaymentScheduleRulesByCustomer);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<PaymentScheduleRulesByCustomer> _PaymentScheduleRulesByCustomer = new List<PaymentScheduleRulesByCustomer>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PaymentScheduleRulesByCustomer/GetPaymentScheduleRulesByCustomer");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<List<PaymentScheduleRulesByCustomer>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PaymentScheduleRulesByCustomer.ToDataSourceResult(request);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetByScheduleId([DataSourceRequest]DataSourceRequest request, PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomerp)
        {
            List<PaymentScheduleRulesByCustomer> _PaymentScheduleRulesByCustomer = new List<PaymentScheduleRulesByCustomer>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PaymentScheduleRulesByCustomer/GetByScheduleId/"+ _PaymentScheduleRulesByCustomerp.ScheduleSubservicesId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<List<PaymentScheduleRulesByCustomer>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _PaymentScheduleRulesByCustomer.ToDataSourceResult(request);

        }


        [HttpPost("[action]")]
        public async Task<ActionResult<PaymentScheduleRulesByCustomer>> SavePaymentScheduleRulesByCustomer([FromBody]PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomer)
        {

            try
            {
                PaymentScheduleRulesByCustomer _listPaymentScheduleRulesByCustomer = new PaymentScheduleRulesByCustomer();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/PaymentScheduleRulesByCustomer/GetPaymentScheduleRulesByCustomerById/" + _PaymentScheduleRulesByCustomer.PaymentScheduleRulesByCustomerId);
                string valorrespuesta = "";
                _PaymentScheduleRulesByCustomer.FechaModificacion = DateTime.Now;
                _PaymentScheduleRulesByCustomer.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listPaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<PaymentScheduleRulesByCustomer>(valorrespuesta);
                }

                if (_listPaymentScheduleRulesByCustomer == null) { _listPaymentScheduleRulesByCustomer = new PaymentScheduleRulesByCustomer(); }

                if (_listPaymentScheduleRulesByCustomer.PaymentScheduleRulesByCustomerId == 0)
                {
                    _PaymentScheduleRulesByCustomer.FechaCreacion = DateTime.Now;
                    _PaymentScheduleRulesByCustomer.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_PaymentScheduleRulesByCustomer);
                }
                else
                {
                    var updateresult = await Update(_PaymentScheduleRulesByCustomer);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_PaymentScheduleRulesByCustomer);
        }

        // POST: PaymentScheduleRulesByCustomer/Insert
        [HttpPost("[controller]/[action]")]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult<PaymentScheduleRulesByCustomer>> Insert(PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomer)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _PaymentScheduleRulesByCustomer.UsuarioCreacion = HttpContext.Session.GetString("user");
                _PaymentScheduleRulesByCustomer.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/PaymentScheduleRulesByCustomer/Insert", _PaymentScheduleRulesByCustomer);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<PaymentScheduleRulesByCustomer>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_PaymentScheduleRulesByCustomer);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _PaymentScheduleRulesByCustomer }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<PaymentScheduleRulesByCustomer>> Update(PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomer)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/PaymentScheduleRulesByCustomer/Update", _PaymentScheduleRulesByCustomer);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<PaymentScheduleRulesByCustomer>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_PaymentScheduleRulesByCustomer);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<PaymentScheduleRulesByCustomer>> Delete([FromBody]PaymentScheduleRulesByCustomer _PaymentScheduleRulesByCustomer)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/PaymentScheduleRulesByCustomer/Delete", _PaymentScheduleRulesByCustomer);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _PaymentScheduleRulesByCustomer = JsonConvert.DeserializeObject<PaymentScheduleRulesByCustomer>(valorrespuesta);
                }
                else
                {
                    string d = await (result.Content.ReadAsStringAsync());
                    throw new Exception(d);
                    //return await Task.Run(() => BadRequest($"Ocurrio un error: {d}"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return Ok(_PaymentScheduleRulesByCustomer);
        }





    }
}
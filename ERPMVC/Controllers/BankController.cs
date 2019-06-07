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
    public class BankController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public BankController(ILogger<BankController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwBank(Int64 Id = 0)
        {
            Bank _Bank = new Bank();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Bank/GetBankById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);

                }

                if (_Bank == null)
                {
                    _Bank = new Bank();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Bank);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Bank> _Bank = new List<Bank>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Bank/GetBank");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<List<Bank>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Bank.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetJson([DataSourceRequest]DataSourceRequest request)
        {
            List<Bank> _Bank = new List<Bank>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Bank/GetBank");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<List<Bank>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Bank.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Bank>> SaveBank([FromBody]Bank _Bank)
        {

            try
            {
                Bank _listBank = new Bank();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Bank/GetBankById/" + _Bank.BankId);
                string valorrespuesta = "";
                _Bank.FechaModificacion = DateTime.Now;
                _Bank.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listBank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);
                }

                if (_listBank.BankId == 0)
                {
                    _Bank.FechaCreacion = DateTime.Now;
                    _Bank.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Bank);
                }
                else
                {
                    var updateresult = await Update(_Bank.BankId, _Bank);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Bank);
        }

        // POST: Bank/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Bank>> Insert(Bank _Bank)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Bank.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Bank.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Bank/Insert", _Bank);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Bank);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Bank }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Bank>> Update(Int64 id, Bank _Bank)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Bank/Update", _Bank);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Bank }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Bank>> Delete([FromBody]Bank _Bank)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Bank/Delete", _Bank);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Bank }, Total = 1 });
        }





    }
}
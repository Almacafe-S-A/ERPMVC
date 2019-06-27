using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class BankController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public BankController(ILogger<BankController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Bank()
        {
            return View();
        }

        public async Task<ActionResult> pvwAddBank([FromBody]BankDTO _sarpara)
        {
            BankDTO _Bank = new BankDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Bank/GetBankById/" + _sarpara.BankId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Bank = JsonConvert.DeserializeObject<BankDTO>(valorrespuesta);

                }

                if (_Bank == null)
                {
                    _Bank = new BankDTO();
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
        public async Task<DataSourceResult> GetBank([DataSourceRequest]DataSourceRequest request)
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
        public async Task<ActionResult<Bank>> SaveBank([FromBody]BankDTO _BankS)
        {
            Bank _Bank = _BankS;
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
                    _Bank = JsonConvert.DeserializeObject<Bank>(valorrespuesta);
                }

                if (_Bank == null) { _Bank = new Models.Bank(); }

                if (_BankS.BankId == 0)
                {
                    _BankS.FechaCreacion = DateTime.Now;
                    _BankS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BankS);
                }
                else
                {
                    _BankS.UsuarioCreacion = _Bank.UsuarioCreacion;
                    _BankS.FechaCreacion = _Bank.FechaCreacion;
                    var updateresult = await Update(_Bank.BankId, _BankS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_BankS);
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
                _Bank.FechaModificacion = DateTime.Now;
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

        [HttpPost]
        public async Task<ActionResult<Bank>> Delete(Int64 BankId, Bank _Bank)
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
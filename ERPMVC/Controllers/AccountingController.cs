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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AccountingController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public AccountingController(ILogger<AccountingController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: Accounting
        public ActionResult Accounting()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<Account> _Account = new List<Account>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Account/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<List<Account>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Account.ToDataSourceResult(request));

        }
        [HttpGet]
        public async Task<JsonResult> GetAccounting([DataSourceRequest]DataSourceRequest request)
        {
            List<Account> _customers = new List<Account>();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Account/GetAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<List<Account>>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return Json(_customers.ToDataSourceResult(request));

        }
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> GetAccountClass([DataSourceRequest]DataSourceRequest request)
        {
            List<AccountClass> _clientes = new List<AccountClass>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Account/GetEnumClass" );
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _clientes = JsonConvert.DeserializeObject<List<AccountClass>>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            //  JsonSerializerSettings settings = new JsonSerializerSettings();
            //  settings.Formatting = Formatting.Indented;
            //  settings.TypeNameHandling = TypeNameHandling.Auto;
            //settingall
            // return Json(new { _clientes });



            return Json(_clientes.ToDataSourceResult(request));
        }   

        public async Task<ActionResult<Account>> SaveAccounting([FromBody]AccountDTO _AccountP)
        {
            Account _Account = _AccountP;
            try
            {
                Account _listAccount = new Account();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Account/GetAccountById/" + _Account.AccountId);
                string valorrespuesta = "";
                _Account.FechaModificacion = DateTime.Now;
                _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
                }

                if (_Account == null) { _Account = new Models.Account(); }

                if (_AccountP.AccountId == 0)
                {
                    _Account.FechaCreacion = DateTime.Now;
                    _Account.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_AccountP);
                }
                else
                {
                    _AccountP.UsuarioCreacion = _Account.UsuarioCreacion;
                    _AccountP.FechaCreacion = _Account.FechaCreacion;
                    var updateresult = await Update(_Account.AccountId, _AccountP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_AccountP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddAccount([FromBody]AccountDTO _sarpara)
        {
            AccountDTO _Account = new AccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Account/GetAccountById/" + _sarpara.AccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<AccountDTO>(valorrespuesta);

                }

                if (_Account == null)
                {
                    _Account = new AccountDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Account);

        }

        // POST: Account/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(Account _Account)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Account.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Account.FechaCreacion = DateTime.Now;
                _Account.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Account/Insert", _Account);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }

        [HttpPut("AccountId")]
        public async Task<IActionResult> Update(Int64 AccountId, Account _Account)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Account/Update", _Account);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Account = JsonConvert.DeserializeObject<Account>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Account }, Total = 1 });
        }


    }
}
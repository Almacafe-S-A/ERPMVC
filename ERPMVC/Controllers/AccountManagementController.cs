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
    public class AccountManagementController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public AccountManagementController(ILogger<AccountManagementController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetAccountManagementById(Int64 AccountManagementId)
        {
            AccountManagement _AccountManagementP = new AccountManagement();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementById/" + AccountManagementId);
                string valorrespuesta = "";
                _AccountManagementP.FechaModificacion = DateTime.Now;
                _AccountManagementP.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagementP = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_AccountManagementP);
        }
        public async Task<ActionResult> pvwAddAccountManagement([FromBody]AccountManagementDTO _sarpara)
        {
            AccountManagementDTO _AccountManagement = new AccountManagementDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementById/" + _sarpara.AccountManagementId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagementDTO>(valorrespuesta);

                }

                if (_AccountManagement == null)
                {
                    _AccountManagement = new AccountManagementDTO { FechaCreacion = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_AccountManagement);

        }
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetAccountManagement([DataSourceRequest]DataSourceRequest request)
        {
            List<AccountManagement> _AccountManagement = new List<AccountManagement>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetAccountManagement");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<List<AccountManagement>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _AccountManagement.ToDataSourceResult(request);

        }
        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetJson([DataSourceRequest]DataSourceRequest request)
        {
            List<AccountManagement> _AccountManagement = new List<AccountManagement>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetAccountManagement");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<List<AccountManagement>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_AccountManagement.ToDataSourceResult(request));

        }
        [HttpPost("[action]")]
        public async Task<ActionResult<AccountManagement>> SaveAccountManagement([FromBody]AccountManagementDTO _AccountManagementS)
        {
            AccountManagement _AccountManagement = _AccountManagementS;
            try
            {
                AccountManagement _listAccountManagement = new AccountManagement();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementById/" + _AccountManagement.AccountManagementId);
                string valorrespuesta = "";
                _AccountManagement.FechaModificacion = DateTime.Now;
                _AccountManagement.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                }

                if (_AccountManagement == null) { _AccountManagement = new Models.AccountManagement(); }

                if (_AccountManagementS.AccountManagementId == 0)
                {
                    _AccountManagementS.FechaCreacion = DateTime.Now;
                    _AccountManagementS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_AccountManagementS);
                }
                else
                {
                    _AccountManagementS.UsuarioCreacion = _AccountManagement.UsuarioCreacion;
                    _AccountManagementS.FechaCreacion = _AccountManagement.FechaCreacion;
                    var updateresult = await Update(_AccountManagement.AccountManagementId, _AccountManagementS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_AccountManagementS);
        }
        // POST: AccountManagement/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<AccountManagement>> Insert(AccountManagement _AccountManagement)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _AccountManagement.UsuarioCreacion = HttpContext.Session.GetString("user");
                _AccountManagement.UsuarioModificacion = HttpContext.Session.GetString("user");
                _AccountManagement.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/AccountManagement/Insert", _AccountManagement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_AccountManagement);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountManagement>> Update(Int64 id, AccountManagement _AccountManagement)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/AccountManagement/Update", _AccountManagement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _AccountManagement }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<AccountManagement>> Delete(Int64 AccountManagementId, AccountManagement _AccountManagement)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/AccountManagement/Delete", _AccountManagement);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _AccountManagement }, Total = 1 });
        }
    }
}
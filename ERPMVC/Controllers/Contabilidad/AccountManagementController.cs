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
using System.Security.Claims;
using Syncfusion.XlsIO.Implementation;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AccountManagementController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public AccountManagementController(ILogger<AccountManagementController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Bancos.Cuentas Bancarias")]
        public IActionResult Index()
        {
            ViewData["permisoAgregar"] = _principal.HasClaim("Bancos.Cuentas Bancarias.Agregar", "true");
            ViewData["permisoEditar"] = _principal.HasClaim("Bancos.Cuentas Bancarias.Editar", "true");
            ViewData["permisoEliminar"] = _principal.HasClaim("Bancos.Cuentas Bancarias.Eliminar", "true");
            ViewData["permisoExportar"] = _principal.HasClaim("Bancos.Cuentas Bancarias.Exportar", "true");
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


        public async Task<DataSourceResult> GetAccountManagementByBankNCurrency(DataSourceRequest request, Int64 BankId, int CurrencyId)
        {
            List<AccountManagement> _AccountManagementP = new List<AccountManagement>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetAccountManagementByBankNCurrency/" + BankId + "/" + CurrencyId);
                string valorrespuesta = "";

                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _AccountManagementP = JsonConvert.DeserializeObject<List<AccountManagement>>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            
            return _AccountManagementP.ToDataSourceResult(request);
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
                    _AccountManagement = new AccountManagementDTO { FechaCreacion = DateTime.Now , OpeningDate = DateTime.Now};
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

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetAccountManagementByBankId([DataSourceRequest]DataSourceRequest request , int Bankid)
        {
            List<AccountManagement> _AccountManagement = new List<AccountManagement>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetAccountManagementByBankId/"+ Bankid);
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

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetAccountManagementByBankIdByAccountType([DataSourceRequest]DataSourceRequest request, int Bankid,int pTypeAccount)
        {
            List<AccountManagement> _AccountManagement = new List<AccountManagement>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/AccountManagement/GetAccountManagementByBankIdTypeAccountId/{Bankid}/{pTypeAccount}");
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
                var result1 = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementByAccountTypeAccountNumber/" + _AccountManagement.AccountNumber);
                string valorrespuesta1 = "";
                _AccountManagement.FechaCreacion = DateTime.Now;
                _AccountManagement.UsuarioCreacion = HttpContext.Session.GetString("user");

                if (result1.IsSuccessStatusCode)
                {
                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                    _AccountManagement = JsonConvert.DeserializeObject<AccountManagementDTO>(valorrespuesta1);
                }

                if (_AccountManagement == null) { _AccountManagement = new Models.AccountManagement(); }

                if (_AccountManagement.AccountManagementId > 0)
                {
                    if (_AccountManagement.AccountManagementId != _AccountManagementS.AccountManagementId)
                        return await Task.Run(() => BadRequest($"Ya existe un registro con el mismo Número de Cuenta."));
                }


                if (_AccountManagementS.AccountManagementId == 0)
                {
                    _AccountManagement.FechaCreacion = DateTime.Now;
                    _AccountManagement.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_AccountManagementS);
                }
                else
                {
                    var result = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementById/" + _AccountManagement.AccountManagementId);
                    _AccountManagementS.FechaCreacion = _AccountManagement.FechaCreacion;
                    _AccountManagementS.UsuarioCreacion = _AccountManagement.UsuarioCreacion;
                    
                    var updateresult = await Update(_AccountManagement.AccountManagementId, _AccountManagementS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_AccountManagement);
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
                //_AccountManagement.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_AccountManagement.UsuarioModificacion = HttpContext.Session.GetString("user");
                //_AccountManagement.FechaModificacion = DateTime.Now;
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
                //_AccountManagement.FechaModificacion = DateTime.Now;
                //_AccountManagement.UsuarioModificacion = HttpContext.Session.GetString("user");
                
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




        //[HttpPost]
        //public async Task<ActionResult<AccountManagement>> Delete(Int64 AccountManagementId, AccountManagement _AccountManagement)
        //{
        //    try
        //    {
        //        string baseadress = config.Value.urlbase;
        //        HttpClient _client = new HttpClient();
        //        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

        //        var result = await _client.PostAsJsonAsync(baseadress + "api/AccountManagement/Delete", _AccountManagement);
        //        string valorrespuesta = "";
        //        if (result.IsSuccessStatusCode)
        //        {
        //            valorrespuesta = await (result.Content.ReadAsStringAsync());
        //            _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
        //        return BadRequest($"Ocurrio un error: {ex.Message}");
        //    }



        //    return new ObjectResult(new DataSourceResult { Data = new[] { _AccountManagement }, Total = 1 });
        //}
        [HttpPost]
        public async Task<ActionResult<AccountManagement>> Delete(Int64 AccountManagementId,[FromBody] AccountManagement _AccountManagementS)
        {
            AccountManagement _AccountManagement = _AccountManagementS;
            List<CheckAccount> _CheckAccount = new List<CheckAccount>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result1 = await _client.GetAsync(baseadress + "api/AccountManagement/ValidationDelete/" + _AccountManagement.AccountManagementId);
                string valorrespuesta1 = "";

                if (result1.IsSuccessStatusCode)
                {

                    valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                }
                if (valorrespuesta1 == "0")
                {
                    _AccountManagement.AccountType = "0";
                    _AccountManagement.BankId = 0;
                    _AccountManagement.CurrencyId = 0;

                    var result = await _client.PostAsJsonAsync(baseadress + "api/AccountManagement/Delete", _AccountManagement);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);
                    }
                }
                else
                {
                    return await Task.Run(() => BadRequest("Este registro tiene referencia a otros datos,No se puede Eliminar"));
                }


            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _AccountManagement }, Total = 1 });
        }

    }
}
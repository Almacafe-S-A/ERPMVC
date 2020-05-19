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

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class CheckAccountController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public CheckAccountController(ILogger<CheckAccountController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Bancos.Cheques")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> SFCheque(int id)
        {
            CheckAccountLines _Cheque = new CheckAccountLines();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccountLines/SetImpreso/" + id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Cheque = JsonConvert.DeserializeObject<CheckAccountLines>(valorrespuesta);
                }
                else
                {
                    _Cheque = new CheckAccountLines { Id = id, };
                }
                return await Task.Run(() => View(_Cheque));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: { ex.ToString() }");
            }
        }

        [HttpGet]
        public async Task<ActionResult> SFVoucherCheque(int id)
        {
            try
            {
                CheckAccountLines _check = new CheckAccountLines { Id = id, };
                return await Task.Run(() => View(_check));
            }
            catch (Exception)
            {
                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }
        }

        public async Task<ActionResult> pvwAddCheck([FromBody]CheckAccountLinesDTO _pCheque)
        {
            //CheckAccountLines _Check = new CheckAccountLines();
            
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccountLines/GetCheckAccountLinesById/" + _pCheque.Id);
                string valorrespuesta = "";
                long cuentaid = _pCheque.AccountManagement.AccountManagementId;
                if (result.IsSuccessStatusCode&& await result.Content.ReadAsStringAsync()!= "")
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _pCheque = JsonConvert.DeserializeObject<CheckAccountLinesDTO>(valorrespuesta);                    

                }      
                if (_pCheque==null || _pCheque.Id == 0)
                {
                    _pCheque.Id = 0;
                    _pCheque.Date = DateTime.Now;
                    _pCheque.FechaCreacion = DateTime.Now;
                    _pCheque.Estado = "Pendiente de Aprobacion";
                    _pCheque.IdEstado = 97;
                    //////////Busca la cuenta bancaria asociadada a la chequera
                    _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    result = await _client.GetAsync(baseadress + "api/AccountManagement/GetSAccountManagementById/" + cuentaid);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _pCheque.AccountManagement = JsonConvert.DeserializeObject<AccountManagement>(valorrespuesta);

                    }



                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_pCheque);

        }



        public async Task<ActionResult> AnularCheque([FromBody]CheckAccountLines _pCheque)
        {
            //CheckAccountLines _Check = new CheckAccountLines();

            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccountLines/GetCheckAccountLinesById/" + _pCheque.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _pCheque = JsonConvert.DeserializeObject<CheckAccountLines>(valorrespuesta);
                }
                if (_pCheque.Estado != "Anulado")
                {
                   
                    result = await _client.PutAsJsonAsync(baseadress + "api/CheckAccountLines/AnularCheque", _pCheque);
                    valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _pCheque = JsonConvert.DeserializeObject<CheckAccountLines>(valorrespuesta);
                    }
                }
                else
                {
                    return BadRequest($"Error este cheque ya habia sido Anulado.");
                }
            }
               
            
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: { ex.ToString() }");
            }



            return Ok(_pCheque);

        }

        public async Task<ActionResult> AprobarCheque([FromBody]CheckAccountLines _pCheque, bool aprobacion)
        {
            
            if (_pCheque.IdEstado == 98 )
            {
                aprobacion = true;
            }
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/CheckAccountLines/SetAprobadoRechazado/{_pCheque.Id}/{aprobacion}"  );
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _pCheque = JsonConvert.DeserializeObject<CheckAccountLines>(valorrespuesta);
                }                
                else
                {
                    return BadRequest($"Error este cheque ya habia sido Aprobado o Rechazado.");
                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: { ex.ToString() }");
            }



            return Ok(aprobacion);

        }
        public async Task<JsonResult> GetCheckAccountById(Int64 CheckAccountId)
        {
            CheckAccount _CheckAccountP = new CheckAccount();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + CheckAccountId);
                string valorrespuesta = "";
                _CheckAccountP.FechaModificacion = DateTime.Now;
                _CheckAccountP.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountP = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CheckAccountP);
        }
        public async Task<JsonResult> GetCheckAccountByAccountNumber(string AccountNumber)
        {
            CheckAccount _CheckAccountP = new CheckAccount();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountByAccountNumber/" + AccountNumber);
                string valorrespuesta = "";
                _CheckAccountP.FechaModificacion = DateTime.Now;
                _CheckAccountP.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountP = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CheckAccountP);
        }
        public async Task<ActionResult> pvwAddCheckAccount([FromBody]CheckAccountDTO _sarpara)
        {
            CheckAccountDTO _CheckAccount = new CheckAccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + _sarpara.CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccountDTO>(valorrespuesta);

                }

                if (_CheckAccount == null)
                {
                    _CheckAccount = new CheckAccountDTO { FechaIngreso = DateTime.Now};
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CheckAccount);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCheckAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<CheckAccount> _CheckAccount = new List<CheckAccount>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<List<CheckAccount>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _CheckAccount.ToDataSourceResult(request);
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCheckAccountByBankId([DataSourceRequest]DataSourceRequest request, string bankId)
        {
            List<CheckAccount> cuentasDeBanco = new List<CheckAccount>();
            try
            {
                if (!string.IsNullOrEmpty(bankId))
                {
                    string baseAddress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseAddress + $"api/CheckAccount/GetCheckAccountByBankId/{bankId}");
                    string valorRespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorRespuesta = await (result.Content.ReadAsStringAsync());
                        cuentasDeBanco = JsonConvert.DeserializeObject<List<CheckAccount>>(valorRespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return cuentasDeBanco.ToDataSourceResult(request);
        }



        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetJson([DataSourceRequest]DataSourceRequest request)
        {
            List<CheckAccount> _CheckAccount = new List<CheckAccount>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<List<CheckAccount>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_CheckAccount.ToDataSourceResult(request));

        }
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetCheckAccountLinesLinesByCheckAccountLinesId([DataSourceRequest]DataSourceRequest request , int id)
        {
            List<CheckAccountLines> _CheckAccountLines = new List<CheckAccountLines>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccountLines/GetCheckAccountLines");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountLines = JsonConvert.DeserializeObject<List<CheckAccountLines>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CheckAccountLines.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CheckAccount>> SaveCheckAccount(CheckAccountDTO _CheckAccountS)
        {
            CheckAccount _CheckAccount = _CheckAccountS;
            try
            {
                CheckAccount _listCheckAccount = new CheckAccount();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + _CheckAccount.CheckAccountId);
                string valorrespuesta = "";
                _CheckAccount.FechaModificacion = DateTime.Now;
                _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }

                if (_CheckAccount == null) { _CheckAccount = new Models.CheckAccount(); }

                if (_CheckAccountS.CheckAccountId == 0)
                {
                    _CheckAccountS.FechaCreacion = DateTime.Now;
                    _CheckAccountS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CheckAccountS);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(((BadRequestObjectResult)insertresult.Result).Value);
                    }

                }
                else
                {
                    _CheckAccountS.UsuarioCreacion = _CheckAccount.UsuarioCreacion;
                    _CheckAccountS.FechaCreacion = _CheckAccount.FechaCreacion;
                    var updateresult = await Update(_CheckAccount.CheckAccountId, _CheckAccountS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CheckAccountS);
        }

        // POST: CheckAccount/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CheckAccount>> Insert(CheckAccount _CheckAccount)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CheckAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CheckAccount.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CheckAccount/Insert", _CheckAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    if (error.Length>100)
                    {
                        error = "Error al Guardar";
                    }
                    return BadRequest($"{error}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CheckAccount);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CheckAccount }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CheckAccount>> Update(Int64 id, CheckAccount _CheckAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CheckAccount/Update", _CheckAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CheckAccount }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<CheckAccount>> Delete(Int64 CheckAccountId, CheckAccount _CheckAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CheckAccount/Delete", _CheckAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CheckAccount }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CheckAccount>> SaveCheck([FromBody]CheckAccountLines _Check)
        {
            CheckAccountLines _CheckAccount = _Check;
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GetCheckAccountLines/GetCheckAccountLinesById/" + _CheckAccount.CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    //_CheckAccount = JsonConvert.DeserializeObject<CheckAccountLines>(valorrespuesta);
                }

                if (_CheckAccount == null) { _CheckAccount = new Models.CheckAccountLines(); }
                
                if (_Check.Id == 0)
                {
                    var insertresult = await InsertCheck(_Check);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(insertresult.Result.ToString());
                    }
                }
                else
                {

                    var updateresult = await UpdateCheck(_Check);
                    if (updateresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(updateresult.Result.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Check);
        }

        // POST: CheckAccount/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CheckAccount>> InsertCheck(CheckAccountLines _CheckAccount)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CheckAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CheckAccount.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/CheckAccountLines/Insert", _CheckAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccountLinesDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CheckAccount);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CheckAccount }, Total = 1 });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CheckAccount>> UpdateCheck(CheckAccountLines _CheckAccountLines)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CheckAccountLines/Update", _CheckAccountLines);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountLines = JsonConvert.DeserializeObject<CheckAccountLinesDTO>(valorrespuesta);
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync());
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CheckAccountLines }, Total = 1 });
        }



        public async Task<ActionResult> CheckAccountLines(Int64 CheckAccountId)
        {
            
            CheckAccountDTO _CheckAccountLines = new CheckAccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccountLines = JsonConvert.DeserializeObject<CheckAccountDTO>(valorrespuesta);

                }
                ViewData["permisos"] = _principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_CheckAccountLines));
        }
    }
}
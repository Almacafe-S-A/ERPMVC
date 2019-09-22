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
    public class CheckAccountController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CheckAccountController(ILogger<EmployeesController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("[action]")]
        public async Task<JsonResult> GetCheckAccount([DataSourceRequest]DataSourceRequest request)
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
        [HttpGet]
        public async Task<JsonResult> Get([DataSourceRequest]DataSourceRequest request)
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

        [HttpPost("[action]")]
        public async Task<ActionResult> CheckAccount([FromBody]CheckAccount _CheckAccountP)
        {
            CheckAccount _CheckAccount = new CheckAccount();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "/api/CheckAccount/GetCheckAccountById/" + _CheckAccountP.CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccount>(valorrespuesta);
                }
                if (_CheckAccount == null)
                {
                    _CheckAccount = new CheckAccount();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_CheckAccount);
        }

        public async Task<ActionResult<CheckAccount>> SaveCheckAccount([FromBody]CheckAccountDTO _CheckAccountP)
        {

            CheckAccount _CheckAccount = _CheckAccountP;
            if (_CheckAccountP != null)
            {
                try
                {
                    CheckAccount _listCheckAccount = new CheckAccount();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + _CheckAccount.CheckAccountId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _CheckAccount = JsonConvert.DeserializeObject<CheckAccountDTO>(valorrespuesta);
                    }

                    if (_CheckAccount == null) { _CheckAccount = new Models.CheckAccount(); }

                    if (_CheckAccount.CheckAccountId == 0)
                    {
                        _CheckAccount.FechaCreacion = DateTime.Now;
                        _CheckAccount.FechaModificacion = DateTime.Now;
                        _CheckAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                        _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_CheckAccount);
                    }
                    else
                    {
                        _CheckAccountP.UsuarioCreacion = _CheckAccount.UsuarioCreacion;
                        _CheckAccountP.FechaCreacion = _CheckAccount.FechaCreacion;
                        var updateresult = await Update(_CheckAccount.CheckAccountId, _CheckAccountP);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    throw ex;
                }
            }
            else
            {
                return BadRequest("No llego correctamente el modelo!");
            }
            return Json(_CheckAccount);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CheckAccount _CheckAccount)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CheckAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CheckAccount.FechaCreacion = DateTime.Now;
                _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CheckAccount/Insert", _CheckAccount);
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

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddCheckAccount([FromBody]CheckAccountDTO _CheckAccountE)
        {
            CheckAccountDTO _CheckAccount = new CheckAccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CheckAccount/GetCheckAccountById/" + _CheckAccountE.CheckAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CheckAccount = JsonConvert.DeserializeObject<CheckAccountDTO>(valorrespuesta);
                }
                if (_CheckAccount == null)
                {
                    _CheckAccount = new CheckAccountDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_CheckAccount);
        }

        [HttpPut("CheckAccountId")]
        public async Task<IActionResult> Update(Int64 CheckAccountId, CheckAccount _CheckAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CheckAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                _CheckAccount.FechaModificacion = DateTime.Now;

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

        [HttpDelete("CheckAccountId")]
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
    }
}
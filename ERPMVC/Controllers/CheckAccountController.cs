﻿using System;
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
    public class CheckAccountController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CheckAccountController(ILogger<CheckAccountController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost("[action]")]
        public async Task<ActionResult<CheckAccount>> SaveCheckAccount([FromBody]CheckAccountDTO _CheckAccountS)
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





    }
}
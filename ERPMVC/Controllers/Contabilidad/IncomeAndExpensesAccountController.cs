using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.DTO;
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
    public class IncomeAndExpensesAccountController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public IncomeAndExpensesAccountController(ILogger<IncomeAndExpensesAccountController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwIncomeAndExpensesAccount([FromBody]IncomeAndExpensesAccountDTO _IncomeAndExpensesAccountDTO)
        {
            IncomeAndExpensesAccountDTO _IncomeAndExpensesAccount = new IncomeAndExpensesAccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpensesAccount/GetIncomeAndExpensesAccountById/" + _IncomeAndExpensesAccountDTO.IncomeAndExpensesAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpensesAccount = JsonConvert.DeserializeObject<IncomeAndExpensesAccountDTO>(valorrespuesta);

                }

                if (_IncomeAndExpensesAccount == null)
                {
                    _IncomeAndExpensesAccount = new IncomeAndExpensesAccountDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_IncomeAndExpensesAccount);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<IncomeAndExpensesAccount> _IncomeAndExpensesAccount = new List<IncomeAndExpensesAccount>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpensesAccount/GetIncomeAndExpensesAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpensesAccount = JsonConvert.DeserializeObject<List<IncomeAndExpensesAccount>>(valorrespuesta);
                    //_IncomeAndExpensesAccount = (from c in _IncomeAndExpensesAccount
                    //                                 // .Where(q => q.CustomerId == CustomerId)
                    //                             select new IncomeAndExpensesAccount
                    //                             {
                    //                                 IncomeAndExpensesAccountId = c.IncomeAndExpensesAccountId
                                           
                    //                   }
                    //                  ).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _IncomeAndExpensesAccount.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<IncomeAndExpensesAccount>> SaveIncomeAndExpensesAccount([FromBody]IncomeAndExpensesAccount _IncomeAndExpensesAccount)
        {

            try
            {
                IncomeAndExpensesAccount _listIncomeAndExpensesAccount = new IncomeAndExpensesAccount();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/IncomeAndExpensesAccount/GetIncomeAndExpensesAccountById/" + _IncomeAndExpensesAccount.IncomeAndExpensesAccountId);
                string valorrespuesta = "";
                _IncomeAndExpensesAccount.FechaModificacion = DateTime.Now;
                _IncomeAndExpensesAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listIncomeAndExpensesAccount = JsonConvert.DeserializeObject<IncomeAndExpensesAccount>(valorrespuesta);
                }

                if (_IncomeAndExpensesAccount.IncomeAndExpensesAccountId == 0)
                {
                    _IncomeAndExpensesAccount.FechaCreacion = DateTime.Now;
                    _IncomeAndExpensesAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_IncomeAndExpensesAccount);
                }
                else
                {
                    var updateresult = await Update(_IncomeAndExpensesAccount.IncomeAndExpensesAccountId, _IncomeAndExpensesAccount);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_IncomeAndExpensesAccount);
        }

        // POST: IncomeAndExpensesAccount/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IncomeAndExpensesAccount>> Insert(IncomeAndExpensesAccount _IncomeAndExpensesAccount)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _IncomeAndExpensesAccount.UsuarioCreacion = HttpContext.Session.GetString("user");
                _IncomeAndExpensesAccount.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/IncomeAndExpensesAccount/Insert", _IncomeAndExpensesAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpensesAccount = JsonConvert.DeserializeObject<IncomeAndExpensesAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_IncomeAndExpensesAccount);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpensesAccount }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IncomeAndExpensesAccount>> Update(Int64 id, IncomeAndExpensesAccount _IncomeAndExpensesAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/IncomeAndExpensesAccount/Update", _IncomeAndExpensesAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpensesAccount = JsonConvert.DeserializeObject<IncomeAndExpensesAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpensesAccount }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IncomeAndExpensesAccount>> Delete([FromBody]IncomeAndExpensesAccount _IncomeAndExpensesAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/IncomeAndExpensesAccount/Delete", _IncomeAndExpensesAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _IncomeAndExpensesAccount = JsonConvert.DeserializeObject<IncomeAndExpensesAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _IncomeAndExpensesAccount }, Total = 1 });
        }





    }
}
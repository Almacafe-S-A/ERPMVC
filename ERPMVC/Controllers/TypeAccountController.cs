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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class TypeAccountController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public TypeAccountController(ILogger<TypeAccountController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: TypeAccount
        public ActionResult TypeAccount()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetTypeAccount([DataSourceRequest]DataSourceRequest request)
        {
            List<TypeAccount> _TypeAccount = new List<TypeAccount>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeAccount/GetTypeAccount");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<List<TypeAccount>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TypeAccount.ToDataSourceResult(request));

        }
        public async Task<ActionResult<TypeAccount>> SaveTypeAccount([FromBody]TypeAccountDTO _TypeAccountP)
        {
            TypeAccount _TypeAccount = _TypeAccountP;
            try
            {
                TypeAccount _listAccount = new TypeAccount();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeAccount/GetTypeAccountByName/" + _TypeAccount.TypeAccountName);
                string valorrespuesta = "";
                _TypeAccount.ModifiedDate = DateTime.Now;
                _TypeAccount.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<TypeAccount>(valorrespuesta);
                }


                if (_TypeAccount == null) { _TypeAccount = new Models.TypeAccount(); }



                if (_TypeAccount.TypeAccountId > 0)
                {
                    if (_TypeAccount.TypeAccountId != _TypeAccountP.TypeAccountId)
                        return await Task.Run(() => BadRequest($"Ya existe un tipo de cuenta registrado con ese nombre."));
                }
                if (_TypeAccountP.TypeAccountId == 0)
                {
                    _TypeAccount.CreatedDate = DateTime.Now;
                    _TypeAccount.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TypeAccountP);
                }
                else
                {
                    _TypeAccountP.CreatedUser = _TypeAccount.CreatedUser;
                    _TypeAccountP.CreatedDate = _TypeAccount.CreatedDate;
                    var updateresult = await Update(_TypeAccount.TypeAccountId, _TypeAccountP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TypeAccountP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddTypeAccount([FromBody]TypeAccountDTO _sarpara)
        {
            TypeAccountDTO _TypeAccount = new TypeAccountDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeAccount/GetTypeAccountById/" + _sarpara.TypeAccountId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<TypeAccountDTO>(valorrespuesta);

                }

                if (_TypeAccount == null)
                {
                    _TypeAccount = new TypeAccountDTO();
                }
                ViewData["naturaleza"] = new List<SelectListItem>
                                     {
                                         new SelectListItem("Acreedora","A"),
                                         new SelectListItem("Deudora","D")
                                     };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TypeAccount);

        }

        // POST: TypeAccount/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(TypeAccount _TypeAccount)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

               

                _TypeAccount.CreatedUser = HttpContext.Session.GetString("user");
                _TypeAccount.CreatedDate = DateTime.Now;
                _TypeAccount.ModifiedUser = HttpContext.Session.GetString("user");
                _TypeAccount.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/TypeAccount/Insert", _TypeAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<TypeAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TypeAccount }, Total = 1 });
        }

        [HttpPut("TypeAccountId")]
        public async Task<IActionResult> Update(Int64 TypeAccountId, TypeAccount _TypeAccount)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/TypeAccount/Update", _TypeAccount);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeAccount = JsonConvert.DeserializeObject<TypeAccount>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TypeAccount }, Total = 1 });
        }

    }
}
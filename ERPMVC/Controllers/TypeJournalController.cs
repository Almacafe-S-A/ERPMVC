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
    public class TypeJournalController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public TypeJournalController(ILogger<TypeJournalController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }
        // GET: TypeAccount
        public ActionResult TypeJournal()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetTypeJournal([DataSourceRequest]DataSourceRequest request)
        {
            List<TypeJournal> _TypeJournal = new List<TypeJournal>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeJournal/GetTypeJournal");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeJournal = JsonConvert.DeserializeObject<List<TypeJournal>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_TypeJournal.ToDataSourceResult(request));

        }
        public async Task<ActionResult<TypeJournal>> SaveTypeAccount([FromBody]TypeJournalDTO _TypeJournalP)
        {
            TypeJournal _TypeJournal = _TypeJournalP;
            try
            {
                TypeJournal _listAccount = new TypeJournal();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeJournal/GetTypeJournalById/" + _TypeJournal.TypeJournalId);
                string valorrespuesta = "";
                _TypeJournal.ModifiedDate = DateTime.Now;
                _TypeJournal.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeJournal = JsonConvert.DeserializeObject<TypeJournal>(valorrespuesta);
                }

                if (_TypeJournal == null) { _TypeJournal = new Models.TypeJournal(); }

                if (_TypeJournalP.TypeJournalId == 0)
                {
                    _TypeJournal.CreatedDate = DateTime.Now;
                    _TypeJournal.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_TypeJournalP);
                }
                else
                {
                    _TypeJournalP.CreatedUser = _TypeJournal.CreatedUser;
                    _TypeJournalP.CreatedDate = _TypeJournal.CreatedDate;
                    var updateresult = await Update(_TypeJournal.TypeJournalId, _TypeJournalP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_TypeJournalP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddTypeJournal([FromBody]TypeJournalDTO _sarpara)
        {
            TypeJournalDTO _TypeJournal = new TypeJournalDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/TypeJournal/GetTypeJournalById/" + _sarpara.TypeJournalId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeJournal = JsonConvert.DeserializeObject<TypeJournalDTO>(valorrespuesta);

                }

                if (_TypeJournal == null)
                {
                    _TypeJournal = new TypeJournalDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_TypeJournal);

        }

        // POST: TypeJournal/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(TypeJournal _TypeJournal)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _TypeJournal.CreatedUser = HttpContext.Session.GetString("user");
                _TypeJournal.CreatedDate = DateTime.Now;
                _TypeJournal.ModifiedUser = HttpContext.Session.GetString("user");
                _TypeJournal.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/TypeJournal/Insert", _TypeJournal);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeJournal = JsonConvert.DeserializeObject<TypeJournal>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TypeJournal }, Total = 1 });
        }

        [HttpPut("TypeJournalId")]
        public async Task<IActionResult> Update(Int64 TypeJournalId, TypeJournal _TypeJournal)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/TypeJournal/Update", _TypeJournal);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _TypeJournal = JsonConvert.DeserializeObject<TypeJournal>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _TypeJournal }, Total = 1 });
        }

    }
}
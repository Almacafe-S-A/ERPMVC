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
    public class JournalEntryLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public JournalEntryLineController(ILogger<JournalEntryLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: 
        public ActionResult JournalEntryLine()
        {
            return View();
        }
         [HttpGet("[action]")]
        public async Task<JsonResult> GetJournalEntryLine([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntryLine> _JournalEntryLine = new List<JournalEntryLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_JournalEntryLine.ToDataSourceResult(request));

        }





        
        [HttpPost]
        public async Task<ActionResult> Insert(JournalEntryLine _JournalEntryLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _JournalEntryLine.CreatedUser = HttpContext.Session.GetString("user");
                _JournalEntryLine.CreatedDate = DateTime.Now;
                _JournalEntryLine.ModifiedUser = HttpContext.Session.GetString("user");
                _JournalEntryLine.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryLine/Insert", _JournalEntryLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryLine }, Total = 1 });
        }


        [HttpPut("JournalEntryLineId")]
        public async Task<IActionResult> Update(Int64 JournalEntryLineId, JournalEntryLine _JournalEntryLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/JournalEntryLine/Update", _JournalEntryLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryLine }, Total = 1 });
        }

        public async Task<ActionResult<JournalEntryLine>> SaveJournalEntryLine([FromBody]JournalEntryLineDTO _JournalEntryLineP)
        {
            JournalEntryLine _JournalEntryLine = _JournalEntryLineP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryById/" + _JournalEntryLine.JournalEntryLineId);
                string valorrespuesta = "";
                _JournalEntryLine.ModifiedDate = DateTime.Now;
                _JournalEntryLine.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);
                }

                if (_JournalEntryLine == null) { _JournalEntryLine = new Models.JournalEntryLine(); }

                if (_JournalEntryLineP.JournalEntryLineId == 0)
                {
                    _JournalEntryLine.CreatedDate = DateTime.Now;
                    _JournalEntryLine.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_JournalEntryLineP);
                }
                else
                {
                    _JournalEntryLineP.CreatedUser = _JournalEntryLine.CreatedUser;
                    _JournalEntryLineP.CreatedDate = _JournalEntryLine.CreatedDate;
                    var updateresult = await Update(_JournalEntryLine.JournalEntryLineId, _JournalEntryLineP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_JournalEntryLineP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddJournalEntryLine([FromBody]JournalEntryLineDTO _sarpara)
        {
            JournalEntryLineDTO _JournalEntryLine = new JournalEntryLineDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryById/" + _sarpara.JournalEntryLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLineDTO>(valorrespuesta);

                }

                if (_JournalEntryLine == null)
                {
                    _JournalEntryLine = new JournalEntryLineDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_JournalEntryLine);

        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetJournalEntryLineById(Int64 JournalEntryLineId)
        {
            JournalEntryLine _customers = new JournalEntryLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryLineById/" + JournalEntryLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_customers));
        }


    }

}
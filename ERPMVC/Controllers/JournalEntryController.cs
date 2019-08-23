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
    public class JournalEntryController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public JournalEntryController(ILogger<JournalEntryController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: Purch
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult Proveedores()
        {
            return View();
        }*/
        // GET: Purch/Details/5
        // public ActionResult Details(int id)
        //{
        //  return View();
        //}

        // GET: Purch/Create
        public ActionResult Create()
        {
            return View();
        }
        /*
        public async Task<ActionResult> Proveedores(Int64 PurchId)
        {
            Purch _customers = new Purch();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => View(_customers));
        }

            */
        [HttpGet("[action]")]
        public async Task<JsonResult> GetJournalEntry([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntry> _JournalEntry = new List<JournalEntry>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntry");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<List<JournalEntry>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_JournalEntry.ToDataSourceResult(request));

        }

        



        // POST: TypeAccount/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(JournalEntry _JournalEntry)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _JournalEntry.CreatedUser = HttpContext.Session.GetString("user");
                _JournalEntry.CreatedDate = DateTime.Now;
                _JournalEntry.ModifiedUser = HttpContext.Session.GetString("user");
                _JournalEntry.ModifiedDate = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntry/Insert", _JournalEntry);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntry }, Total = 1 });
        }


        [HttpPut("JournalEntryId")]
        public async Task<IActionResult> Update(Int64 JournalEntryId, JournalEntry _JournalEntry)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/JournalEntry/Update", _JournalEntry);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntry }, Total = 1 });
        }
        // GET: JournalEntry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JournalEntry/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult<Purch>> SaveJournalEntry([FromBody]JournalEntryDTO _JournalEntryP)
        {
            JournalEntry _JournalEntry = _JournalEntryP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _JournalEntry.JournalEntryId);
                string valorrespuesta = "";
                _JournalEntry.ModifiedDate = DateTime.Now;
                _JournalEntry.ModifiedUser = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);
                }

                if (_JournalEntry == null) { _JournalEntry = new Models.JournalEntry(); }

                if (_JournalEntryP.JournalEntryId == 0)
                {
                    _JournalEntry.CreatedDate = DateTime.Now;
                    _JournalEntry.CreatedUser = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_JournalEntryP);
                }
                else
                {
                    _JournalEntryP.CreatedUser = _JournalEntry.CreatedUser;
                    _JournalEntryP.CreatedDate = _JournalEntry.CreatedDate;
                    var updateresult = await Update(_JournalEntry.JournalEntryId, _JournalEntryP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_JournalEntryP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddJournalEntry([FromBody]JournalEntryDTO _sarpara)
        {
            JournalEntryDTO _JournalEntry = new JournalEntryDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _sarpara.JournalEntryId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta);

                }

                if (_JournalEntry == null)
                {
                    _JournalEntry = new JournalEntryDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_JournalEntry);

        }
        // GET: Customer/Details/5

        /*
                [HttpGet("[action]")]
                public async Task<ActionResult> Proveedores(Int64 PurchId)
                {
                    Purch _customers = new Purch();
                    try
                    {
                        string baseadress = config.Value.urlbase;
                        HttpClient _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                        var result = await _client.GetAsync(baseadress + "api/Purch/GetPurchById/" + PurchId);
                        string valorrespuesta = "";
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            _customers = JsonConvert.DeserializeObject<Purch>(valorrespuesta);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                        throw ex;
                    }



                    return await Task.Run(() => View(_customers));
                }
          */
        [HttpGet("[action]")]
        public async Task<ActionResult> GetJournalEntryById(Int64 JournalEntryId)
        {
            JournalEntry _customers = new JournalEntry();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + JournalEntryId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);

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
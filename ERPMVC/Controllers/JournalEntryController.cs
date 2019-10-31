﻿using System;
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

        public ActionResult IndexAjustes()
        {         
            return View();
        }
        public ActionResult JournalEntryLine()
        {
            return View();
        }
        public ActionResult JournalEntryLineAjuste()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> SFAsientoContable(Int64 id)
        {
            try
            {
                JournalEntryDTO _journalentrydto = new JournalEntryDTO { JournalEntryId = id, };
                return await Task.Run(() => View(_journalentrydto));
            }
            catch (Exception)
            {
                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }
        }

        [HttpGet]
        public async Task<ActionResult> SFAjusteContable(Int64 id)
        {
            try
            {
                JournalEntryDTO _journalentrydto = new JournalEntryDTO { JournalEntryId = id, };
                return await Task.Run(() => View(_journalentrydto));
            }
            catch (Exception)
            {
                return await Task.Run(() => BadRequest("Ocurrio un error"));
            }
        }



        /*public ActionResult Proveedores()
        {
            return View();
        }*/
        // GET: Purch/Details/5
        public async Task<ActionResult> Details(Int64 JournalEntryId)
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
                    valorrespuesta = await(result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return await Task.Run(() => View(_customers));
        }

        // GET: Purch/Create
        /*
        public async Task<ActionResult> Proveedores(Int64 PurchId)
        {
           

            return await Task.Run(() => View(_customers));
        }

            */


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<JournalEntryDTO>> Aprobar([FromBody]JournalEntryDTO _JournalEntry)
        {
            JournalEntryDTO _so = new JournalEntryDTO();
            if (_JournalEntry != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _JournalEntry.JournalEntryId);
                    string jsonresult = "";                 
                    jsonresult = JsonConvert.SerializeObject(_JournalEntry);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta);
                        _so.EstadoId = 6;
                        _so.EstadoName = "Aprobado";

                        var resultsalesorder = await Update(_so.JournalEntryId, _so);

                        var value = (resultsalesorder.Result as ObjectResult).Value;
                        JournalEntry resultado = ((JournalEntry)(value));
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
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }
            return await Task.Run(() => Ok(_so));
        }


        
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<JournalEntryDTO>> Rechazar([FromBody]JournalEntryDTO _JournalEntry)
        {
            JournalEntryDTO _so = new JournalEntryDTO();
            if (_JournalEntry != null)
            {
                try
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _JournalEntry.JournalEntryId);
                    string jsonresult = "";
                    jsonresult = JsonConvert.SerializeObject(_JournalEntry);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _so = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta);
                        _so.EstadoId = 7;
                        _so.EstadoName = "Rechazado";
                        var resultsalesorder = await Update(_so.JournalEntryId, _so);
                       var value = (resultsalesorder.Result as ObjectResult).Value;
                        JournalEntry resultado = ((JournalEntry)(value));
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
                return await Task.Run(() => BadRequest("No llego correctamente el modelo!"));
            }
            return await Task.Run(() => Ok(_so));
        }

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
                    _JournalEntry = _JournalEntry.OrderByDescending(q => q.JournalEntryId).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_JournalEntry.ToDataSourceResult(request));

        }


        [HttpGet("[action]")]
        public async Task<JsonResult> GetJournalEntryAsientos([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntry> _JournalEntry = new List<JournalEntry>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryAsientos");
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


        [HttpGet("[action]")]
        public async Task<JsonResult> GetJournalEntryAjustes([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntry> _JournalEntry = new List<JournalEntry>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryAjustes");
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
        public async Task<ActionResult<JournalEntry>> Insert(JournalEntry _JournalEntry)
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
                _JournalEntry.IdPaymentCode = 0;
                _JournalEntry.IdTypeofPayment = 0;
                _JournalEntry.DocumentId = 0;

                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntry/Insert", _JournalEntry);
                string jsonresult = "";
                jsonresult = JsonConvert.SerializeObject(_JournalEntry);

                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntry = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Ocurrio un error{valorrespuesta}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
               // return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_JournalEntry);
           // return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntry }, Total = 1 });
        }


        [HttpPut("action")]
        public async Task<ActionResult<JournalEntry>> Update(Int64 JournalEntryId, JournalEntryDTO _JournalEntryLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntry/Update", _JournalEntryLine);
                string jsonresult = "";
                jsonresult = JsonConvert.SerializeObject(_JournalEntryLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta);
                }
                else
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    throw new Exception($"Ocurrio un error:{valorrespuesta}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
               // return BadRequest($"Ocurrio un error{ex.Message}");
            }
            // return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntry }, Total = 1 });
            return Ok(_JournalEntryLine);
        }

        public async Task<ActionResult<JournalEntry>> SaveJournalEntry([FromBody]dynamic dto)
        // public async Task<ActionResult<JournalEntry>> SaveJournalEntry([FromBody]JournalEntryDTO _JournalEntryP)
        {
            JournalEntryDTO _JournalEntryP = new JournalEntryDTO(); //_JournalEntryP;
            JournalEntry _JournalEntry = new JournalEntry();


            try
            {
                _JournalEntryP = JsonConvert.DeserializeObject<JournalEntryDTO>(dto.ToString());
                _JournalEntry = _JournalEntryP;
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

                    foreach (var item in _JournalEntryP.JournalEntryLines)
                    {
                        item.CreatedUser = HttpContext.Session.GetString("user");
                        item.ModifiedUser = HttpContext.Session.GetString("user");
                    }

                    var insertresult = await Insert(_JournalEntryP);
                  
                    var value = (insertresult.Result as ObjectResult).Value;
                    _JournalEntry  = ((JournalEntry)(value));
                    if (_JournalEntry.JournalEntryId <= 0)

                    {
                        return BadRequest("No se guardo el formulario!");
                    }
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

            return Json(_JournalEntry);
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
                    _JournalEntry.Date = DateTime.Now;
                    _JournalEntry.DatePosted = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_JournalEntry);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddJournalEntryAjuste([FromBody]JournalEntryDTO _sarpara)
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
                    _JournalEntry.Date = DateTime.Now;
                    _JournalEntry.DatePosted = DateTime.Now;
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
            JournalEntry _journalentry = new JournalEntry();
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
                    _journalentry = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta);
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return await Task.Run(() => Json(_journalentry));
        }


    }
}
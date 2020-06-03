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
        public IActionResult JournalEntryLine()
        {
            return PartialView();
        }
         [HttpGet("[action]")]
        public async Task<DataSourceResult> GetJournalEntryLine([DataSourceRequest]DataSourceRequest request,JournalEntryLine _JournalEntryLinep)
         {
            List<JournalEntryLine> _JournalEntryLine = new List<JournalEntryLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                if (HttpContext.Session.Get("journalentryline") == null
                   || HttpContext.Session.GetString("journalentryline") == "")
                {
                    if (_JournalEntryLinep.JournalEntryId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_JournalEntryLinep).ToString();
                        HttpContext.Session.SetString("journalentryline", serialzado);
                    }
                }
                else
                {
                    _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(HttpContext.Session.GetString("journalentryline"));
                }
                if (_JournalEntryLinep.JournalEntryId > 0 && _JournalEntryLinep.JournalEntryLineId > 0)
                {
                    JournalEntry _JournalEntryP = new JournalEntry();

                    _JournalEntryLinep.JournalEntryLineId = 0;
                    var insertresult = await Insert(_JournalEntryLinep);
                    
                    var result1 = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryLineByJournalId/" + _JournalEntryLinep.JournalEntryId);
                    string valorrespuesta1 = "";
                    if (result1.IsSuccessStatusCode)
                    {
                        valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                        _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(valorrespuesta1);

                        var result2 = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _JournalEntryLinep.JournalEntryId);
                        string valorrespuesta2 = "";
                        if (result2.IsSuccessStatusCode)
                        {
                            valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                            _JournalEntryP = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta2);
                        }
                        _JournalEntryP.PartyTypeName = "";
                        var beneficiarios = 0;
                        var NombreBeneficiario = "";
                        var CountParty = 0;
                        var CountPartyDistintos = 0;
                        foreach (var item in _JournalEntryP.JournalEntryLines)
                        {
                            if (item.PartyName != null)
                            {
                                _JournalEntryP.PartyTypeId = item.PartyTypeId;
                                _JournalEntryP.PartyTypeName = item.PartyTypeName;
                                if (item.PartyName == "Otros")
                                {
                                    item.PartyId = 0;
                                }
                                _JournalEntryP.PartyId = item.PartyId;
                                _JournalEntryP.PartyName = item.PartyName;

                                beneficiarios++;

                                NombreBeneficiario = item.PartyName;
                                foreach (var item1 in _JournalEntryP.JournalEntryLines)
                                {
                                    if (item1.PartyName != null)
                                    {
                                        if (NombreBeneficiario == item1.PartyName)
                                        {
                                            CountParty++;
                                        }
                                        else
                                        {
                                            CountPartyDistintos++;
                                        }
                                    }
                                }
                            }
                        }
                        if (CountParty > 1 && CountPartyDistintos == 0)
                        {
                            _JournalEntryP.PartyName = NombreBeneficiario;
                        }
                        else if (beneficiarios > 1)
                        {
                            _JournalEntryP.PartyName = "Varios";
                            _JournalEntryP.PartyId = 0;
                            _JournalEntryP.PartyTypeName = "";
                            _JournalEntryP.PartyTypeId = 0;
                        }
                        _JournalEntryP.ModifiedUser = HttpContext.Session.GetString("user");
                        _JournalEntryP.ModifiedDate = DateTime.Now;
                        var result3 = await _client.PostAsJsonAsync(baseadress + "api/JournalEntry/Update", _JournalEntryP);
                        string valorrespuesta3 = "";
                        if (result3.IsSuccessStatusCode)
                        {
                            valorrespuesta3 = await (result3.Content.ReadAsStringAsync());
                            _JournalEntryP = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta3);
                        }
                    }
                    
                }
                if (_JournalEntryLinep.JournalEntryId > 0)
                {
                    var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryLineByJournalId/"+ _JournalEntryLinep.JournalEntryId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(valorrespuesta);
                        HttpContext.Session.SetString("journalentryline", JsonConvert.SerializeObject(_JournalEntryLine).ToString());

                    }
                }
                else
                {

                    List<JournalEntryLine> _existelinea = new List<JournalEntryLine>();
                    if (HttpContext.Session.GetString("journalentryline") != "" && HttpContext.Session.GetString("journalentryline") !=null)
                    {
                        _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(HttpContext.Session.GetString("journalentryline"));
                        _existelinea = _JournalEntryLine.Where(q => q.JournalEntryLineId == _JournalEntryLinep.JournalEntryLineId).ToList();
                    }


                    if (_JournalEntryLinep.JournalEntryLineId > 0 && _existelinea.Count == 0)
                    {
                        _JournalEntryLine.Add(_JournalEntryLinep);
                        HttpContext.Session.SetString("journalentryline", JsonConvert.SerializeObject(_JournalEntryLine).ToString());
                    }
                    else
                    {
                        var obj = _JournalEntryLine.Where(x => x.JournalEntryLineId == _JournalEntryLinep.JournalEntryLineId).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Description = _JournalEntryLinep.Description;
                            obj.Credit = _JournalEntryLinep.Credit;
                            obj.Debit = _JournalEntryLinep.Debit;
                            obj.AccountId = _JournalEntryLinep.AccountId;
                            obj.AccountName = _JournalEntryLinep.AccountName;
                            obj.JournalEntry = _JournalEntryLinep.JournalEntry;
                            //obj.Num = _JournalEntryLinep.Num;
                            obj.CostCenterId = _JournalEntryLinep.CostCenterId;
                            obj.CostCenterName = _JournalEntryLinep.CostCenterName;

                        }

                        HttpContext.Session.SetString("journalentryline", JsonConvert.SerializeObject(_JournalEntryLine).ToString());

                    }
                }




                }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return (_JournalEntryLine.ToDataSourceResult(request));

        }


        [HttpPost("[action]")]
        public async Task<JournalEntryLine> UpdateJournalEntryLine([FromBody]JournalEntryLine _JournalEntryLinep)
        {
            List<JournalEntryLine> _JournalEntryLine = new List<JournalEntryLine>();
            try
            {
               
                    _JournalEntryLine = JsonConvert.DeserializeObject<List<JournalEntryLine>>(HttpContext.Session.GetString("journalentryline"));
                foreach (var item in _JournalEntryLine)
                {
                    if (item.AccountId == _JournalEntryLinep.AccountId)
                    {
                        item.Credit = _JournalEntryLinep.Credit;
                    }
                }
                
                   string serialzado = JsonConvert.SerializeObject(_JournalEntryLine).ToString();
                        HttpContext.Session.SetString("journalentryline", serialzado);
                   
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _JournalEntryLinep;

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<JournalEntryLine>> Delete([FromBody]JournalEntryLine _JournalEntryLine)
        {
            List<JournalEntryLine> _journalentryLIST = new List<JournalEntryLine>();
            try
            {
                if (_JournalEntryLine.JournalEntryId > 0)
                {
                    JournalEntry _JournalEntryP = new JournalEntry();
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryLine/Delete", _JournalEntryLine);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);

                        List<JournalEntryLine> _JournalEntryLineList = new List<JournalEntryLine>();
                        var result1 = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryLineByJournalId/" + _JournalEntryLine.JournalEntryId);
                        string valorrespuesta1 = "";
                        if (result1.IsSuccessStatusCode)
                        {
                            valorrespuesta1 = await (result1.Content.ReadAsStringAsync());
                            _JournalEntryLineList = JsonConvert.DeserializeObject<List<JournalEntryLine>>(valorrespuesta1);

                            var result2 = await _client.GetAsync(baseadress + "api/JournalEntry/GetJournalEntryById/" + _JournalEntryLine.JournalEntryId);
                            string valorrespuesta2 = "";
                            if (result2.IsSuccessStatusCode)
                            {
                                valorrespuesta2 = await (result2.Content.ReadAsStringAsync());
                                _JournalEntryP = JsonConvert.DeserializeObject<JournalEntry>(valorrespuesta2);
                            }
                            _JournalEntryP.PartyTypeName = "";
                            var beneficiarios = 0;
                            var NombreBeneficiario = "";
                            var CountParty = 0;
                            var CountPartyDistintos = 0;
                            foreach (var item in _JournalEntryP.JournalEntryLines)
                            {
                                if (item.PartyName != null)
                                {
                                    _JournalEntryP.PartyTypeId = item.PartyTypeId;
                                    _JournalEntryP.PartyTypeName = item.PartyTypeName;
                                    _JournalEntryP.PartyId = item.PartyId;
                                    _JournalEntryP.PartyName = item.PartyName;

                                    beneficiarios++;

                                    NombreBeneficiario = item.PartyName;
                                    foreach (var item1 in _JournalEntryP.JournalEntryLines)
                                    {
                                        if (item1.PartyName != null)
                                        {
                                            if (NombreBeneficiario == item1.PartyName)
                                            {
                                                CountParty++;
                                            }
                                            else
                                            {
                                                CountPartyDistintos++;
                                            }
                                        }
                                    }
                                }
                            }
                            if (CountParty > 1 && CountPartyDistintos == 0)
                            {
                                _JournalEntryP.PartyName = NombreBeneficiario;
                            }
                            else if (beneficiarios > 1)
                            {
                                _JournalEntryP.PartyName = "Varios";
                                _JournalEntryP.PartyId = 0;
                                _JournalEntryP.PartyTypeName = "";
                                _JournalEntryP.PartyTypeId = 0;
                            }
                            _JournalEntryP.ModifiedUser = HttpContext.Session.GetString("user");
                            _JournalEntryP.ModifiedDate = DateTime.Now;
                            var result3 = await _client.PostAsJsonAsync(baseadress + "api/JournalEntry/Update", _JournalEntryP);
                            string valorrespuesta3 = "";
                            if (result3.IsSuccessStatusCode)
                            {
                                valorrespuesta3 = await (result3.Content.ReadAsStringAsync());
                                _JournalEntryP = JsonConvert.DeserializeObject<JournalEntryDTO>(valorrespuesta3);
                            }
                        }
                    }
                }
                _journalentryLIST =
                JsonConvert.DeserializeObject<List<JournalEntryLine>>(HttpContext.Session.GetString("journalentryline"));

                if (_journalentryLIST != null)
                {
                    _journalentryLIST = _journalentryLIST
                          .Where(q => q.JournalEntryLineId != _JournalEntryLine.JournalEntryLineId)                         
                          .ToList();

                    HttpContext.Session.SetString("journalentryline", JsonConvert.SerializeObject(_journalentryLIST));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return await Task.Run(() => Ok(_journalentryLIST));
          //  return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryLine }, Total = 1 });
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
                _JournalEntryLine.CreditME = 0;
                _JournalEntryLine.DebitME = 0;
                _JournalEntryLine.CreditSy = 0;
                _JournalEntryLine.DebitSy = 0;


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
                var result = await _client.GetAsync(baseadress + "api/JournalEntryLine/GetJournalEntryLineById/" + _sarpara.JournalEntryLineId);
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


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddJournalEntryLineAjuste([FromBody]JournalEntryLineDTO _sarpara)
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
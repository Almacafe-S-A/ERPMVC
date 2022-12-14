using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class JournalEntryConfigurationLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public JournalEntryConfigurationLineController(ILogger<JournalEntryConfigurationLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwJournalEntryConfigurationLine([FromBody]JournalEntryConfigurationLine _JournalEntryConfigurationLinep)
        {
            JournalEntryConfigurationLine _JournalEntryConfigurationLine = new JournalEntryConfigurationLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfigurationLine/GetJournalEntryConfigurationLineById/" + _JournalEntryConfigurationLinep.JournalEntryConfigurationLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);

                }

                if (_JournalEntryConfigurationLine == null)
                {
                    _JournalEntryConfigurationLine = new JournalEntryConfigurationLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView("~/Views/JournalEntryConfiguration/pvwJournalEntryConfigurationMant.cshtml", _JournalEntryConfigurationLine);

        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetJournalEntryConfigurationLine([DataSourceRequest]DataSourceRequest request, JournalEntryConfigurationLine _JournalEntryConfigurationLine)
        {
            List<JournalEntryConfigurationLine> _JournalEntryConfigurationLinelist = new List<JournalEntryConfigurationLine>();

            try
            {

                if (HttpContext.Session.Get("JournalEntryConfigurationLine") == null
                    || HttpContext.Session.GetString("JournalEntryConfigurationLine") == "")
                {
                    if (_JournalEntryConfigurationLine.JournalEntryConfigurationId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_JournalEntryConfigurationLinelist).ToString();
                        HttpContext.Session.SetString("JournalEntryConfigurationLine", serialzado);
                    }
                }
                else
                {
                    _JournalEntryConfigurationLinelist = JsonConvert.DeserializeObject<List<JournalEntryConfigurationLine>>(HttpContext.Session.GetString("JournalEntryConfigurationLine"));
                }

                if (_JournalEntryConfigurationLine.JournalEntryConfigurationId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    // _client.DefaultRequestHeaders.Add("JournalEntryConfigurationId", _JournalEntryConfigurationLine.JournalEntryConfigurationId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/JournalEntryConfigurationLine/GetJournalEntryConfigurationLineByConfigurationId/" + _JournalEntryConfigurationLine.JournalEntryConfigurationId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _JournalEntryConfigurationLinelist = JsonConvert.DeserializeObject<List<JournalEntryConfigurationLine>>(valorrespuesta);

                        if (_JournalEntryConfigurationLinelist.Count <= 0)
                        {
                            if (_JournalEntryConfigurationLine.JournalEntryConfigurationLineId > 0)
                            {
                                _JournalEntryConfigurationLinelist.Add(_JournalEntryConfigurationLine);
                            }
                        }


                        _client = new HttpClient();
                        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                         result = await _client.GetAsync(baseadress + "api/JournalEntryConfigurationLine/GetJournalEntryConfigurationLineById/" + _JournalEntryConfigurationLine.JournalEntryConfigurationLineId);
                         valorrespuesta = "";
                        JournalEntryConfigurationLine qJournalEntryConfigurationLine = new JournalEntryConfigurationLine();
                        if (result.IsSuccessStatusCode)
                        {
                            valorrespuesta = await (result.Content.ReadAsStringAsync());
                            qJournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);
                            _JournalEntryConfigurationLinelist = _JournalEntryConfigurationLinelist.OrderByDescending(q => q.JournalEntryConfigurationLineId).ToList();

                        }

                        if(qJournalEntryConfigurationLine != null)
                        {

                            var actualizacion = await Update(_JournalEntryConfigurationLine.JournalEntryConfigurationLineId, _JournalEntryConfigurationLine);

                            var obj = _JournalEntryConfigurationLinelist.FirstOrDefault(x => x.JournalEntryConfigurationLineId == _JournalEntryConfigurationLine.JournalEntryConfigurationLineId);
                            if (obj != null)
                            {
                                obj.AccountId = _JournalEntryConfigurationLine.AccountId;
                                obj.AccountName = _JournalEntryConfigurationLine.AccountName;
                                obj.DebitCreditId = _JournalEntryConfigurationLine.DebitCreditId;
                                obj.DebitCredit = _JournalEntryConfigurationLine.DebitCredit;
                                obj.CostCenterId = _JournalEntryConfigurationLine.CostCenterId;
                                obj.CostCenterName = _JournalEntryConfigurationLine.CostCenterName;
                                obj.SubProductId = _JournalEntryConfigurationLine.SubProductId;
                                obj.SubProductName = _JournalEntryConfigurationLine.SubProductName;
                            }


                        }
                        else
                        {
                            if (_JournalEntryConfigurationLine.JournalEntryConfigurationLineId > 0)
                            {
                              
                                _JournalEntryConfigurationLinelist.Add(_JournalEntryConfigurationLine);
                                _JournalEntryConfigurationLine.JournalEntryConfigurationLineId = 0;
                                var resultline = await Insert(_JournalEntryConfigurationLine);
                                var value = (resultline.Result as ObjectResult).Value;
                                JournalEntryConfigurationLine resultado = ((JournalEntryConfigurationLine)(value));
                                _JournalEntryConfigurationLine.JournalEntryConfigurationLineId = resultado.JournalEntryConfigurationLineId;
                                // HttpContext.Session.SetString("JournalEntryConfigurationLine", JsonConvert.SerializeObject(_JournalEntryConfigurationLinelist).ToString());
                            }

                        }
                        _JournalEntryConfigurationLinelist = _JournalEntryConfigurationLinelist.OrderByDescending(q => q.JournalEntryConfigurationLineId).ToList();
                        HttpContext.Session.SetString("JournalEntryConfigurationLine", JsonConvert.SerializeObject(_JournalEntryConfigurationLinelist).ToString());
                    }
                }
                else
                {

                    List<JournalEntryConfigurationLine> _existelinea = new List<JournalEntryConfigurationLine>();
                    if (HttpContext.Session.GetString("JournalEntryConfigurationLine") != "" && HttpContext.Session.GetString("JournalEntryConfigurationLine") != null)
                    {
                        _JournalEntryConfigurationLinelist = JsonConvert.DeserializeObject<List<JournalEntryConfigurationLine>>(HttpContext.Session.GetString("JournalEntryConfigurationLine"));
                        _existelinea = _JournalEntryConfigurationLinelist.Where(q => q.JournalEntryConfigurationLineId == _JournalEntryConfigurationLine.JournalEntryConfigurationLineId).ToList();
                    }



                    if (_JournalEntryConfigurationLine.JournalEntryConfigurationLineId > 0 && _existelinea.Count == 0)
                    {
                        _JournalEntryConfigurationLinelist.Add(_JournalEntryConfigurationLine);
                        _JournalEntryConfigurationLinelist = _JournalEntryConfigurationLinelist.OrderByDescending(q => q.JournalEntryConfigurationLineId).ToList();
                        HttpContext.Session.SetString("JournalEntryConfigurationLine", JsonConvert.SerializeObject(_JournalEntryConfigurationLinelist).ToString());
                    }
                    else
                    {
                        var obj = _JournalEntryConfigurationLinelist.FirstOrDefault(x => x.JournalEntryConfigurationLineId == _JournalEntryConfigurationLine.JournalEntryConfigurationLineId);
                        if (obj != null)
                        {
                            obj.AccountId = _JournalEntryConfigurationLine.AccountId;
                            obj.AccountName = _JournalEntryConfigurationLine.AccountName;
                            obj.DebitCredit = _JournalEntryConfigurationLine.DebitCredit;
                            obj.CostCenterId = _JournalEntryConfigurationLine.CostCenterId;
                            obj.CostCenterName = _JournalEntryConfigurationLine.CostCenterName;
                            obj.SubProductId = _JournalEntryConfigurationLine.SubProductId;
                            obj.SubProductName = _JournalEntryConfigurationLine.SubProductName;
                        }
                        _JournalEntryConfigurationLinelist = _JournalEntryConfigurationLinelist.OrderByDescending(q => q.JournalEntryConfigurationLineId).ToList();
                        HttpContext.Session.SetString("JournalEntryConfigurationLine", JsonConvert.SerializeObject(_JournalEntryConfigurationLinelist).ToString());

                    }



                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _JournalEntryConfigurationLinelist.ToDataSourceResult(request);
        }




        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<JournalEntryConfigurationLine> _JournalEntryConfigurationLine = new List<JournalEntryConfigurationLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfigurationLine/GetJournalEntryConfigurationLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfigurationLine = JsonConvert.DeserializeObject<List<JournalEntryConfigurationLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _JournalEntryConfigurationLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<JournalEntryConfigurationLine>> SaveJournalEntryConfigurationLine([FromBody]JournalEntryConfigurationLine _JournalEntryConfigurationLine)
        {

            try
            {
                JournalEntryConfigurationLine _listJournalEntryConfigurationLine = new JournalEntryConfigurationLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/JournalEntryConfigurationLine/GetJournalEntryConfigurationLineById/" + _JournalEntryConfigurationLine.JournalEntryConfigurationLineId);
                string valorrespuesta = "";
                _JournalEntryConfigurationLine.FechaModificacion = DateTime.Now;
                _JournalEntryConfigurationLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listJournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);
                }

                if (_listJournalEntryConfigurationLine.JournalEntryConfigurationLineId == 0)
                {
                    _JournalEntryConfigurationLine.FechaCreacion = DateTime.Now;
                    _JournalEntryConfigurationLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_JournalEntryConfigurationLine);
                }
                else
                {
                    var updateresult = await Update(_JournalEntryConfigurationLine.JournalEntryConfigurationLineId, _JournalEntryConfigurationLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_JournalEntryConfigurationLine);
        }

        // POST: JournalEntryConfigurationLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<JournalEntryConfigurationLine>> Insert(JournalEntryConfigurationLine _JournalEntryConfigurationLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _JournalEntryConfigurationLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                _JournalEntryConfigurationLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryConfigurationLine/Insert", _JournalEntryConfigurationLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_JournalEntryConfigurationLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryConfigurationLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JournalEntryConfigurationLine>> Update(Int64 id, JournalEntryConfigurationLine _JournalEntryConfigurationLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/JournalEntryConfigurationLine/Update", _JournalEntryConfigurationLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _JournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

          return  Ok(_JournalEntryConfigurationLine);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryConfigurationLine }, Total = 1 });
        }

        //[HttpDelete("_JournalEntryConfigurationLine")]
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<JournalEntryConfigurationLine>> Delete([FromBody]JournalEntryConfigurationLine _JournalEntryConfigurationLine)
        {
            List<JournalEntryConfigurationLine> _journalLIST = new List<JournalEntryConfigurationLine>();
            try
            {
                _journalLIST =
                JsonConvert.DeserializeObject<List<JournalEntryConfigurationLine>>(HttpContext.Session.GetString("JournalEntryConfigurationLine"));

                if (_journalLIST != null)
                {
                    _journalLIST = _journalLIST.Where(q => q.JournalEntryConfigurationLineId != _JournalEntryConfigurationLine.JournalEntryConfigurationLineId).ToList();
                    HttpContext.Session.SetString("JournalEntryConfigurationLine", JsonConvert.SerializeObject(_journalLIST));
                }
                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryConfigurationLine/Delete", _JournalEntryConfigurationLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _JournalEntryConfigurationLine = JsonConvert.DeserializeObject<JournalEntryConfigurationLine>(valorrespuesta);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return await Task.Run(() => Ok(_journalLIST));
            //return new ObjectResult(new DataSourceResult { Data = new[] { _journalentryLIST }, Total = 1 });
        }

    }
}
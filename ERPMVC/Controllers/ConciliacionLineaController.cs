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
    public class ConciliacionLineaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ConciliacionLineaController(ILogger<ConciliacionLineaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        // GET: 
        public IActionResult ConciliacionLinea()
        {
            return ConciliacionLinea();
        }
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetConciliacionLinea([DataSourceRequest]DataSourceRequest request, ConciliacionLinea _ConciliacionLineap)
        {
            List<ConciliacionLinea> _ConciliacionLinea = new List<ConciliacionLinea>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();

                if (HttpContext.Session.Get("ConciliacionLinea") == null
                   || HttpContext.Session.GetString("ConciliacionLinea") == "")
                {
                    if (_ConciliacionLineap.ConciliacionLineaId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_ConciliacionLineap).ToString();
                        HttpContext.Session.SetString("ConciliacionLinea", serialzado);
                    }
                }
                else
                {
                    _ConciliacionLinea = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(HttpContext.Session.GetString("ConciliacionLinea"));
                }

                if (_ConciliacionLineap.ConciliacionLineaId > 0)
                {
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaByConciliacionId/" + _ConciliacionLineap.ConciliacionLineaId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ConciliacionLinea = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(valorrespuesta);
                        HttpContext.Session.SetString("ConciliacionLinea", JsonConvert.SerializeObject(_ConciliacionLinea).ToString());

                    }
                }
                else
                {

                    List<ConciliacionLinea> _existelinea = new List<ConciliacionLinea>();
                    if (HttpContext.Session.GetString("ConciliacionLinea") != "" && HttpContext.Session.GetString("ConciliacionLinea") != null)
                    {
                        _ConciliacionLinea = JsonConvert.DeserializeObject<List<ConciliacionLinea>>(HttpContext.Session.GetString("ConciliacionLinea"));
                        _existelinea = _ConciliacionLinea.Where(q => q.ConciliacionLineaId == _ConciliacionLineap.ConciliacionLineaId).ToList();
                    }


                    if (_ConciliacionLineap.ConciliacionLineaId > 0 && _existelinea.Count == 0)
                    {
                        _ConciliacionLinea.Add(_ConciliacionLineap);
                        HttpContext.Session.SetString("ConciliacionLinea", JsonConvert.SerializeObject(_ConciliacionLinea).ToString());
                    }
                    else
                    {
                        var obj = _ConciliacionLinea.Where(x => x.ConciliacionLineaId == _ConciliacionLineap.ConciliacionLineaId).FirstOrDefault();
                        if (obj != null)
                        {
                            //obj.Description = _ConciliacionLineap.Description;
                            obj.Credit = _ConciliacionLineap.Credit;
                            obj.Debit = _ConciliacionLineap.Debit;
                            obj.AccountId = _ConciliacionLineap.AccountId;
                            obj.AccountName = _ConciliacionLineap.AccountName;
                            obj.JournalEntryId = _ConciliacionLineap.JournalEntryId;
                            //obj.Num = _JournalEntryLinep.Num;
                            
                        }

                        HttpContext.Session.SetString("ConciliacionLinea", JsonConvert.SerializeObject(_ConciliacionLinea).ToString());

                    }
                }




            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return (_ConciliacionLinea.ToDataSourceResult(request));

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<ConciliacionLinea>> Delete([FromBody]ConciliacionLinea _ConciliacionLinea)
        {
            List<ConciliacionLinea> _ConciliacionLineaLIST = new List<ConciliacionLinea>();
            try
            {
                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.PostAsJsonAsync(baseadress + "api/JournalEntryLine/Delete", _JournalEntryLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _JournalEntryLine = JsonConvert.DeserializeObject<JournalEntryLine>(valorrespuesta);
                //}

                _ConciliacionLineaLIST =
                JsonConvert.DeserializeObject<List<ConciliacionLinea>>(HttpContext.Session.GetString("ConciliacionLinea"));

                if (_ConciliacionLineaLIST != null)
                {
                    _ConciliacionLineaLIST = _ConciliacionLineaLIST
                          .Where(q => q.ConciliacionLineaId != _ConciliacionLinea.ConciliacionLineaId)
                          .ToList();

                    HttpContext.Session.SetString("ConciliacionLinea", JsonConvert.SerializeObject(_ConciliacionLineaLIST));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }

            return await Task.Run(() => Ok(_ConciliacionLineaLIST));
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _JournalEntryLine }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult> Insert(ConciliacionLinea _ConciliacionLinea)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ConciliacionLinea.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ConciliacionLinea.FechaCreacion = DateTime.Now;
                _ConciliacionLinea.UsuarioModificacion = HttpContext.Session.GetString("user");
                _ConciliacionLinea.FechaModificacion = DateTime.Now;
                _ConciliacionLinea.Debit = 0;
                _ConciliacionLinea.Credit = 0;
                //_JournalEntryLine.CreditSy = 0;
                //_JournalEntryLine.DebitSy = 0;


                var result = await _client.PostAsJsonAsync(baseadress + "api/ConciliacionLinea/Insert", _ConciliacionLinea);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLinea = JsonConvert.DeserializeObject<ConciliacionLinea>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConciliacionLinea }, Total = 1 });
        }


        [HttpPut("ConciliacionLineaId")]
        public async Task<IActionResult> Update(Int64 ConciliacionLineaId, ConciliacionLinea _ConciliacionLinea)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/ConciliacionLinea/Update", _ConciliacionLinea);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLinea = JsonConvert.DeserializeObject<ConciliacionLinea>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ConciliacionLinea }, Total = 1 });
        }

        public async Task<ActionResult<ConciliacionLinea>> SaveConciliacionLinea([FromBody]ConciliacionLineaDTO _ConciliacionLineaP)
        {
            ConciliacionLinea _ConciliacionLinea = _ConciliacionLineaP;
            try
            {
                //JournalEntry _listItems = new JournalEntry();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaById/" + _ConciliacionLinea.ConciliacionLineaId);
                string valorrespuesta = "";
                _ConciliacionLinea.FechaModificacion = DateTime.Now;
                _ConciliacionLinea.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLinea = JsonConvert.DeserializeObject<ConciliacionLinea>(valorrespuesta);
                }

                if (_ConciliacionLinea == null) { _ConciliacionLinea = new Models.ConciliacionLinea(); }

                if (_ConciliacionLineaP.ConciliacionLineaId == 0)
                {
                    _ConciliacionLineaP.FechaCreacion = DateTime.Now;
                    _ConciliacionLineaP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ConciliacionLineaP);
                }
                else
                {
                    _ConciliacionLineaP.UsuarioCreacion = _ConciliacionLinea.UsuarioCreacion;
                    _ConciliacionLineaP.FechaCreacion = _ConciliacionLinea.FechaCreacion;
                    var updateresult = await Update(_ConciliacionLinea.ConciliacionLineaId, _ConciliacionLineaP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ConciliacionLineaP);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddConciliacionLinea([FromBody]ConciliacionLineaDTO _sarpara)
        {
            ConciliacionLineaDTO _ConciliacionLinea = new ConciliacionLineaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaById/" + _sarpara.ConciliacionLineaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLinea = JsonConvert.DeserializeObject<ConciliacionLineaDTO>(valorrespuesta);

                }

                if (_ConciliacionLinea == null)
                {
                    _ConciliacionLinea = new ConciliacionLineaDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ConciliacionLinea);

        }


        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddConciliacionLineaAjuste([FromBody]ConciliacionLineaDTO _sarpara)
        {
            ConciliacionLineaDTO _ConciliacionLinea = new ConciliacionLineaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaById/" + _sarpara.ConciliacionLineaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ConciliacionLinea = JsonConvert.DeserializeObject<ConciliacionLineaDTO>(valorrespuesta);

                }

                if (_ConciliacionLinea == null)
                {
                    _ConciliacionLinea = new ConciliacionLineaDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_ConciliacionLinea);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetConciliacionLineaLineById(Int64 ConciliacionLineaId)
        {
            ConciliacionLinea _customers = new ConciliacionLinea();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ConciliacionLinea/GetConciliacionLineaById/" + ConciliacionLineaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _customers = JsonConvert.DeserializeObject<ConciliacionLinea>(valorrespuesta);

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
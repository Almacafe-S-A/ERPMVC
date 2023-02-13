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
    public class CreditNoteLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CreditNoteLineController(ILogger<CreditNoteLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwCreditNoteLine([FromBody]CreditNoteLine _CreditNoteLinep)
        {
            CreditNoteLine _CreditNoteLine = new CreditNoteLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNoteLine/GetCreditNoteLineById/" + _CreditNoteLinep.CreditNoteLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNoteLine = JsonConvert.DeserializeObject<CreditNoteLine>(valorrespuesta);

                }

                if (_CreditNoteLine == null)
                {
                    _CreditNoteLine = new CreditNoteLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            //return PartialView(_CreditNoteLine);
            return PartialView("~/Views/CreditNote/pvwCreditNoteDetailMant.cshtml", _CreditNoteLine);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> SetLinesInSession([FromBody]CreditNoteLine _CreditNoteLine)
        {

            try
            {

                List<CreditNoteLine> _GoodsReceivedLine = new List<CreditNoteLine>();
                _GoodsReceivedLine = JsonConvert.DeserializeObject<List<CreditNoteLine>>(HttpContext.Session.GetString("listadoproductosCreditNote"));

                if (_GoodsReceivedLine == null) { _GoodsReceivedLine = new List<CreditNoteLine>(); }
                _GoodsReceivedLine.Add(_CreditNoteLine);              
                HttpContext.Session.SetString("listadoproductosCreditNote", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return await Task.Run(() => Json(_CreditNoteLine));
        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CreditNoteLine> _CreditNoteLine = new List<CreditNoteLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNoteLine/GetCreditNoteLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNoteLine = JsonConvert.DeserializeObject<List<CreditNoteLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CreditNoteLine.ToDataSourceResult(request);

        }

        [HttpGet("[action]")]
        public async Task<ActionResult<DataSourceResult>> GetByCreditLineId([DataSourceRequest]DataSourceRequest request, int CreditNoteId, int InvoiceId)
        {

            List<CreditNoteLine> creditNoteLines= new List<CreditNoteLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                
                string valorrespuesta = "";
                // _CreditNoteLine.FechaModificacion = DateTime.Now;
                //_CreditNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                HttpResponseMessage result;

                if (CreditNoteId>0)
                {
                   
                    result = await _client.GetAsync(baseadress + "api/CreditNoteLine/GetByCreditNoteId/" + CreditNoteId);
                }
                else
                {
                    result = await _client.GetAsync(baseadress + "api/CreditNoteLine/GetByInvoiceId/" + InvoiceId);
                }

                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    creditNoteLines = JsonConvert.DeserializeObject<List<CreditNoteLine>>(valorrespuesta);
                }
                else
                {
                    return BadRequest(await result.Content.ReadAsStringAsync());
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return creditNoteLines.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CreditNoteLine>> SaveCreditNoteLine([FromBody]CreditNoteLine _CreditNoteLine)
        {

            try
            {
                CreditNoteLine _listCreditNoteLine = new CreditNoteLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CreditNoteLine/GetCreditNoteLineById/" + _CreditNoteLine.CreditNoteLineId);
                string valorrespuesta = "";
               // _CreditNoteLine.FechaModificacion = DateTime.Now;
                //_CreditNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCreditNoteLine = JsonConvert.DeserializeObject<CreditNoteLine>(valorrespuesta);
                }

                if (_listCreditNoteLine == null) { _listCreditNoteLine = new CreditNoteLine(); }

                if (_listCreditNoteLine.CreditNoteLineId == 0)
                {
                   // _CreditNoteLine.FechaCreacion = DateTime.Now;
                   // _CreditNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CreditNoteLine);
                }
                else
                {
                    var updateresult = await Update(_CreditNoteLine.CreditNoteLineId, _CreditNoteLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CreditNoteLine);
        }

        // POST: CreditNoteLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CreditNoteLine>> Insert(CreditNoteLine _CreditNoteLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _CreditNoteLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_CreditNoteLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CreditNoteLine/Insert", _CreditNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNoteLine = JsonConvert.DeserializeObject<CreditNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CreditNoteLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CreditNoteLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CreditNoteLine>> Update(Int64 id, CreditNoteLine _CreditNoteLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CreditNoteLine/Update", _CreditNoteLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CreditNoteLine = JsonConvert.DeserializeObject<CreditNoteLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_CreditNoteLine);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _CreditNoteLine }, Total = 1 });
        }

      

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<CreditNoteLine>> Delete([FromBody]CreditNoteLine _CreditNoteLine)
        {
            List<CreditNoteLine> _creditnoteLIST = new List<CreditNoteLine>();
            try
            {
                _creditnoteLIST = JsonConvert.DeserializeObject<List<CreditNoteLine>>(HttpContext.Session.GetString("listadoproductosCreditNote"));

                if (_creditnoteLIST != null)
                {
                    var item = _creditnoteLIST.Find(c => c.CreditNoteLineId == _CreditNoteLine.CreditNoteLineId);
                    _creditnoteLIST.Remove(item);
                    HttpContext.Session.SetString("listadoproductosCreditNote", JsonConvert.SerializeObject(_creditnoteLIST));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return await Task.Run(() => Ok(_CreditNoteLine));
        }
    }
}
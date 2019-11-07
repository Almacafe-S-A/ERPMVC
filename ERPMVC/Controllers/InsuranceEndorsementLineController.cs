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
    public class InsuranceEndorsementLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InsuranceEndorsementLineController(ILogger<InsuranceEndorsementLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwInsuranceEndorsementLine([FromBody]InsuranceEndorsementLine _InvoiceLinep)
        {
            InsuranceEndorsementLine _InvoiceLine = new InsuranceEndorsementLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsementLine/GetInsuranceEndorsementLineById/" + _InvoiceLinep.InsuranceEndorsementLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InsuranceEndorsementLine>(valorrespuesta);

                }

                if (_InvoiceLine == null)
                {
                    _InvoiceLine = new InsuranceEndorsementLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            //return PartialView(_InvoiceLine);
            return PartialView("~/Views/InsuranceEndorsement/pvwInsuranceEndorsementDetailMant.cshtml", _InvoiceLine);


        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetInsuranceEndorsementLineByInvoiceId([DataSourceRequest]DataSourceRequest request, InsuranceEndorsementLine _InvoiceLinep)
        {
            List<InsuranceEndorsementLine> __InvoiceLineList = new List<InsuranceEndorsementLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosInsuranceEndorsement") == null
                   || HttpContext.Session.GetString("listadoproductosInsuranceEndorsement") == "")
                {
                    if (_InvoiceLinep.InsuranceEndorsementId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_InvoiceLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosInsuranceEndorsement", serialzado);
                    }
                }
                else
                {
                    var result = HttpContext.Session.GetString("listadoproductosInsuranceEndorsement");
                    try
                    {
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InsuranceEndorsementLine>>(HttpContext.Session.GetString("listadoproductosInsuranceEndorsement"));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                    }

                }


                if (_InvoiceLinep.InsuranceEndorsementId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsementLine/GetInsuranceEndorsementLineByInvoiceId/" + _InvoiceLinep.InsuranceEndorsementId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InsuranceEndorsementLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosInsuranceEndorsement", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                }
                else
                {

                    List<InsuranceEndorsementLine> _existelinea = new List<InsuranceEndorsementLine>();
                    if (HttpContext.Session.GetString("listadoproductosInsuranceEndorsement") != "" && HttpContext.Session.GetString("listadoproductosInsuranceEndorsement") != null)
                    {
                        __InvoiceLineList = JsonConvert.DeserializeObject<List<InsuranceEndorsementLine>>(HttpContext.Session.GetString("listadoproductosInsuranceEndorsement"));
                        _existelinea = __InvoiceLineList.Where(q => q.InsuranceEndorsementLineId == _InvoiceLinep.InsuranceEndorsementLineId).ToList();
                    }

                    if (_InvoiceLinep.InsuranceEndorsementLineId > 0 && _existelinea.Count == 0)
                    {
                        __InvoiceLineList.Add(_InvoiceLinep);
                        HttpContext.Session.SetString("listadoproductosInsuranceEndorsement", JsonConvert.SerializeObject(__InvoiceLineList).ToString());
                    }
                    else
                    {

                        var obj = __InvoiceLineList.FirstOrDefault(x => x.InsuranceEndorsementLineId == _InvoiceLinep.InsuranceEndorsementLineId);
                        if (obj != null)
                        {
                            obj.AmountLp = _InvoiceLinep.AmountLp;
                            obj.AmountDl = _InvoiceLinep.AmountDl;
                            obj.AssuredDiference = _InvoiceLinep.AssuredDiference;
                            obj.CertificateBalance = _InvoiceLinep.CertificateBalance;
                            obj.WareHouseId = _InvoiceLinep.WareHouseId;
                            obj.WarehouseName = _InvoiceLinep.WarehouseName;


                        }

                        HttpContext.Session.SetString("listadoproductosInsuranceEndorsement", JsonConvert.SerializeObject(__InvoiceLineList).ToString());

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return __InvoiceLineList.ToDataSourceResult(request);


        }



        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsuranceEndorsementLine> _InvoiceLine = new List<InsuranceEndorsementLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsementLine/GetInsuranceEndorsementLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<List<InsuranceEndorsementLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InvoiceLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsuranceEndorsementLine>> SaveInsuranceEndorsementLine([FromBody]InsuranceEndorsementLine _InvoiceLine)
        {

            try
            {
                InsuranceEndorsementLine _listInvoiceLine = new InsuranceEndorsementLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsuranceEndorsementLine/GetInvoiceLineById/" + _InvoiceLine.InsuranceEndorsementLineId);
                string valorrespuesta = "";
                // _InvoiceLine.FechaModificacion = DateTime.Now;
                //_InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInvoiceLine = JsonConvert.DeserializeObject<InsuranceEndorsementLine>(valorrespuesta);
                }

                if (_listInvoiceLine.InsuranceEndorsementLineId == 0)
                {
                    // _InvoiceLine.FechaCreacion = DateTime.Now;
                    //  _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InvoiceLine);
                }
                else
                {
                    var updateresult = await Update(_InvoiceLine.InsuranceEndorsementLineId, _InvoiceLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InvoiceLine);
        }

        // POST: InvoiceLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsuranceEndorsementLine>> Insert(InsuranceEndorsementLine _InvoiceLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                // _InvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                // _InvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceEndorsementLine/Insert", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InsuranceEndorsementLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_InvoiceLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsuranceEndorsementLine>> Update(Int64 id, InsuranceEndorsementLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsuranceEndorsementLine/Update", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InsuranceEndorsementLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

        [HttpDelete("_InvoiceLine")]
        public async Task<ActionResult<InsuranceEndorsementLine>> Delete([FromBody]InsuranceEndorsementLine _InvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/InsuranceEndorsementLine/Delete", _InvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InvoiceLine = JsonConvert.DeserializeObject<InsuranceEndorsementLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InvoiceLine }, Total = 1 });
        }

    }
}
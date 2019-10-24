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
    /*public class Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }*/
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class InsurancesCertificateLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public InsurancesCertificateLineController(ILogger<InsurancesCertificateLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwInsurancesCertificateDetailMant(int InsurancesCertificateLineId = 0)
        {
            InsurancesCertificateLine _InsurancesCertificateLine = new InsurancesCertificateLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificateLine/GetInsurancesCertificateLineById/" + InsurancesCertificateLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateLine = JsonConvert.DeserializeObject<InsurancesCertificateLine>(valorrespuesta);

                }

                if (_InsurancesCertificateLine == null)
                {
                    _InsurancesCertificateLine = new InsurancesCertificateLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView("~/Views/InsurancesCertificate/pvwInsurancesCertificateDetailMant.cshtml", _InsurancesCertificateLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<InsurancesCertificateLine> _InsurancesCertificateLine = new List<InsurancesCertificateLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificateLine/GetInsurancesCertificateLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateLine = JsonConvert.DeserializeObject<List<InsurancesCertificateLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _InsurancesCertificateLine.ToDataSourceResult(request);

        }

        [HttpGet]
        public async Task<ActionResult> GetInsurancesCertificateLineById([DataSourceRequest]DataSourceRequest request, Int64 Id)
        {
            List<InsurancesCertificateLine> _InsurancesCertificateLine = new List<InsurancesCertificateLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificateLine/GetInsurancesCertificateLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateLine = JsonConvert.DeserializeObject<List<InsurancesCertificateLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_InsurancesCertificateLine.ToDataSourceResult(request));

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetSalesOrderLine([DataSourceRequest]DataSourceRequest request, CertificadoLine _CertificadoLine)
        {
            List<CertificadoLine> _CertificadoLinelist = new List<CertificadoLine>();

            try
            {

                if (HttpContext.Session.Get("listadoproductoscertificadodeposito") == null
                    || HttpContext.Session.GetString("listadoproductoscertificadodeposito") == "")
                {
                    if (_CertificadoLine.IdCD > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_CertificadoLine).ToString();
                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", serialzado);
                    }
                }
                else
                {
                    _CertificadoLinelist = JsonConvert.DeserializeObject<List<CertificadoLine>>(HttpContext.Session.GetString("listadoproductoscertificadodeposito"));
                }
                if (_CertificadoLine.IdCD > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //  _client.DefaultRequestHeaders.Add("SalesOrderId", _CertificadoLine.IdCD.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/CertificadoLine/GetCertificadoLineByIdCD/" + _CertificadoLine.IdCD);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _CertificadoLinelist = JsonConvert.DeserializeObject<List<CertificadoLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", JsonConvert.SerializeObject(_CertificadoLinelist).ToString());
                    }
                }
                else
                {

                    List<CertificadoLine> _existelinea = new List<CertificadoLine>();
                    if (HttpContext.Session.GetString("listadoproductoscertificadodeposito") != "")
                    {
                        _CertificadoLinelist = JsonConvert.DeserializeObject<List<CertificadoLine>>(HttpContext.Session.GetString("listadoproductoscertificadodeposito"));
                        _existelinea = _CertificadoLinelist.Where(q => q.CertificadoLineId == _CertificadoLine.CertificadoLineId).ToList();
                    }

                    //if(uid=="")
                    if (_CertificadoLine.CertificadoLineId > 0 && _existelinea.Count == 0)
                    {
                        _CertificadoLinelist.Add(_CertificadoLine);
                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", JsonConvert.SerializeObject(_CertificadoLinelist).ToString());
                    }
                    else
                    {
                        var obj = _CertificadoLinelist.FirstOrDefault(x => x.CertificadoLineId == _CertificadoLine.CertificadoLineId);
                        if (obj != null)
                        {
                            obj.Description = _CertificadoLine.Description;
                            obj.Price = _CertificadoLine.Price;
                            obj.Quantity = _CertificadoLine.Quantity;
                            obj.Amount = _CertificadoLine.Amount;
                            obj.SubProductId = _CertificadoLine.SubProductId;
                            obj.SubProductName = _CertificadoLine.SubProductName;
                            obj.TotalCantidad = _CertificadoLine.TotalCantidad;
                            obj.UnitMeasureId = _CertificadoLine.UnitMeasureId;
                            obj.UnitMeasurName = _CertificadoLine.UnitMeasurName;
                            obj.ValorImpuestos = _CertificadoLine.ValorImpuestos;
                        }

                        HttpContext.Session.SetString("listadoproductoscertificadodeposito", JsonConvert.SerializeObject(_CertificadoLinelist).ToString());



                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CertificadoLinelist.ToDataSourceResult(request);
        }






        [HttpPost("[action]")]
        public async Task<ActionResult<InsurancesCertificateLine>> SaveInsurancesCertificateLine([FromBody]InsurancesCertificateLine _InsurancesCertificateLine)
        {

            try
            {
                InsurancesCertificateLine _listInsurancesCertificateLine = new InsurancesCertificateLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/InsurancesCertificateLine/GetInsurancesCertificateLineById/" + _InsurancesCertificateLine.InsurancesCertificateLineId);
                string valorrespuesta = "";
                // _CertificadoLine.FechaModificacion = DateTime.Now;
                //_CertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listInsurancesCertificateLine = JsonConvert.DeserializeObject<InsurancesCertificateLine>(valorrespuesta);
                }

                if (_InsurancesCertificateLine.InsurancesCertificateLineId == 0)
                {
                    // _CertificadoLine.FechaCreacion = DateTime.Now;
                    //_CertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_InsurancesCertificateLine);
                }
                else
                {
                    var updateresult = await Update(_InsurancesCertificateLine.InsurancesCertificateLineId, _InsurancesCertificateLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_InsurancesCertificateLine);
        }




        // POST: CertificadoLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<InsurancesCertificateLine>> Insert(InsurancesCertificateLine _InsurancesCertificateLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                // _CertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_CertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoLine/Insert", _InsurancesCertificateLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateLine = JsonConvert.DeserializeObject<InsurancesCertificateLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_InsurancesCertificateLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsurancesCertificateLine>> Update(Int64 id, InsurancesCertificateLine _InsurancesCertificateLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/InsurancesCertificateLine/Update", _InsurancesCertificateLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _InsurancesCertificateLine = JsonConvert.DeserializeObject<InsurancesCertificateLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesCertificateLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<InsurancesCertificateLine>> Delete([FromBody]InsurancesCertificateLine _InsurancesCertificateLine)
        {
            try
            {

                List<CertificadoLine> _salesorderLIST =
                JsonConvert.DeserializeObject<List<CertificadoLine>>(HttpContext.Session.GetString("listadoproductoscertificadodeposito"));

                if (_salesorderLIST != null)
                {
                    _salesorderLIST = _salesorderLIST
                            .Where(q => q.CertificadoLineId == _InsurancesCertificateLine.InsurancesCertificateLineId)
                       //    .Where(q => q.Quantity != _InsurancesCertificateLine.Quantity)
                       //    .Where(q => q.Amount != _CertificadoLine.Amount)
                       //    .Where(q => q.TotalCantidad != _CertificadoLine.TotalCantidad)
                       //    .Where(q => q.Price != _CertificadoLine.Price)
                       //    .Where(q => q.SubProductId != _CertificadoLine.SubProductId)
                          .ToList();

                    HttpContext.Session.SetString("listadoproductoscertificadodeposito", JsonConvert.SerializeObject(_salesorderLIST));
                }


                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                //var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoLine/Delete", _CertificadoLine);
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _CertificadoLine = JsonConvert.DeserializeObject<CertificadoLine>(valorrespuesta);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _InsurancesCertificateLine }, Total = 1 });
        }





    }
}
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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProformaInvoiceLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ProformaInvoiceLineController(ILogger<ProformaInvoiceLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwProformaInvoiceLine([FromBody]ProformaInvoiceLine _salesorderline)
        {
            ProformaInvoiceLine _ProformaInvoiceLine = new ProformaInvoiceLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineById/" + _salesorderline.ProformaInvoiceId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);

                }

                if (_ProformaInvoiceLine == null)
                {
                    _ProformaInvoiceLine = new ProformaInvoiceLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return PartialView("~/Views/ProformaInvoice/pvwProformaInvoiceDetailMant.cshtml", _ProformaInvoiceLine);
          //  return PartialView(_ProformaInvoiceLine);

        }



        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetProformaInvoiceLineByProformaInvoiceId([DataSourceRequest]DataSourceRequest request, ProformaInvoiceLine _ProformaInvoiceLinep)
        {
            List<ProformaInvoiceLine> _GoodsReceivedLine = new List<ProformaInvoiceLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosproformainvoice") == null
                   || HttpContext.Session.GetString("listadoproductosproformainvoice") == "")
                {
                    if (_ProformaInvoiceLinep.ProformaInvoiceId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_ProformaInvoiceLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosproformainvoice", serialzado);
                    }
                }
                else
                {
                    var result = HttpContext.Session.GetString("listadoproductosproformainvoice");
                    try
                    {
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(HttpContext.Session.GetString("listadoproductosproformainvoice"));
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                   
                }


                if (_ProformaInvoiceLinep.ProformaInvoiceId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineByProformaInvoiceId/" + _ProformaInvoiceLinep.ProformaInvoiceId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(valorrespuesta);
                    }
                }
                else
                {
                    if (_ProformaInvoiceLinep.Quantity > 0)
                    {
                        _GoodsReceivedLine.Add(_ProformaInvoiceLinep);
                        HttpContext.Session.SetString("listadoproductosproformainvoice", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceivedLine.ToDataSourceResult(request);

        }




        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<ProformaInvoiceLine> _ProformaInvoiceLine = new List<ProformaInvoiceLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<List<ProformaInvoiceLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ProformaInvoiceLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProformaInvoiceLine>> SaveProformaInvoiceLine([FromBody]ProformaInvoiceLine _ProformaInvoiceLine)
        {

            try
            {
                ProformaInvoiceLine _listProformaInvoiceLine = new ProformaInvoiceLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ProformaInvoiceLine/GetProformaInvoiceLineById/" + _ProformaInvoiceLine.ProformaLineId);
                string valorrespuesta = "";
               // _ProformaInvoiceLine.FechaModificacion = DateTime.Now;
                //_ProformaInvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

                if (_listProformaInvoiceLine.ProformaLineId == 0)
                {
                  //  _ProformaInvoiceLine.FechaCreacion = DateTime.Now;
                   // _ProformaInvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ProformaInvoiceLine);
                }
                else
                {
                    var updateresult = await Update(_ProformaInvoiceLine.ProformaLineId, _ProformaInvoiceLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ProformaInvoiceLine);
        }

        // POST: ProformaInvoiceLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ProformaInvoiceLine>> Insert(ProformaInvoiceLine _ProformaInvoiceLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //_ProformaInvoiceLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_ProformaInvoiceLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Insert", _ProformaInvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_ProformaInvoiceLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProformaInvoiceLine>> Update(Int64 id, ProformaInvoiceLine _ProformaInvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Update", _ProformaInvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ProformaInvoiceLine>> Delete([FromBody]ProformaInvoiceLine _ProformaInvoiceLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ProformaInvoiceLine/Delete", _ProformaInvoiceLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ProformaInvoiceLine = JsonConvert.DeserializeObject<ProformaInvoiceLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ProformaInvoiceLine }, Total = 1 });
        }





    }
}
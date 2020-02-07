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
    public class EndososCertificadosLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososCertificadosLineController(ILogger<EndososCertificadosLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwEndososDetailMant([FromBody]EndososCertificadosLine _EndososCertificadosLinep)
        {
            EndososCertificadosLine _EndososCertificadosLine = new EndososCertificadosLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLineById/" + _EndososCertificadosLinep.EndososCertificadosLineId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);

                }

                if (_EndososCertificadosLine == null)
                {
                    _EndososCertificadosLine = new EndososCertificadosLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView("~/Views/EndososCertificados/pvwEndososDetailMant.cshtml", _EndososCertificadosLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososCertificadosLine> _EndososCertificadosLine = new List<EndososCertificadosLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososCertificadosLine.ToDataSourceResult(request);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetEndososCertificadosLineByEndososCertificadosId([DataSourceRequest]DataSourceRequest request, EndososCertificadosLine _EndososCertificadosLineP)
        {
            List<EndososCertificadosLine> _EndososCertificadosLine = new List<EndososCertificadosLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosEndosos") == null
                   || HttpContext.Session.GetString("listadoproductosEndosos") == "")
                {
                    if (_EndososCertificadosLineP.EndososCertificadosId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_EndososCertificadosLineP).ToString();
                        HttpContext.Session.SetString("listadoproductosEndosos", serialzado);
                    }
                }
                else
                {
                    _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(HttpContext.Session.GetString("listadoproductosEndosos"));
                }


                if (_EndososCertificadosLineP.EndososCertificadosId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLineByEndososCertificadosId/" + _EndososCertificadosLineP.EndososCertificadosId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(valorrespuesta);
                        HttpContext.Session.SetString("listadoproductosEndosos", JsonConvert.SerializeObject(_EndososCertificadosLine).ToString());
                    }
                }
                else
                {
                    List<EndososCertificadosLine> _existelinea = new List<EndososCertificadosLine>();
                    if (HttpContext.Session.GetString("listadoproductosEndosos") != "" && HttpContext.Session.GetString("listadoproductosEndosos") != null)
                    {
                        _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(HttpContext.Session.GetString("listadoproductosEndosos"));
                        _existelinea = _EndososCertificadosLine.Where(q => q.EndososCertificadosLineId == _EndososCertificadosLineP.EndososCertificadosLineId).ToList();
                    }

                    if (_EndososCertificadosLineP.Price > 0 && _existelinea.Count == 0)
                    {
                        _EndososCertificadosLine.Add(_EndososCertificadosLineP);
                        HttpContext.Session.SetString("listadoproductosEndosos", JsonConvert.SerializeObject(_EndososCertificadosLine).ToString());
                    }
                    else
                    {

                        var obj = _EndososCertificadosLine.FirstOrDefault(x => x.EndososCertificadosLineId == _EndososCertificadosLineP.EndososCertificadosLineId);
                        if (obj != null)
                        {
                            //obj.CertificadoLineId = _EndososCertificadosLineP.CertificadoLineId;
                            obj.Price = _EndososCertificadosLineP.Price;
                            obj.EndososCertificadosId = _EndososCertificadosLineP.EndososCertificadosId;
                            obj.EndososCertificadosLineId = _EndososCertificadosLineP.EndososCertificadosLineId;
                            obj.Quantity = _EndososCertificadosLineP.Quantity;
                            obj.SubProductId = _EndososCertificadosLineP.SubProductId;
                            obj.SubProductName = _EndososCertificadosLineP.SubProductName;
                            obj.UnitOfMeasureId = _EndososCertificadosLineP.UnitOfMeasureId;
                            obj.UnitOfMeasureName = _EndososCertificadosLineP.UnitOfMeasureName;
                            obj.ValorEndoso = _EndososCertificadosLineP.ValorEndoso;
                        }

                        HttpContext.Session.SetString("listadoproductosEndosos", JsonConvert.SerializeObject(_EndososCertificadosLine).ToString());

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososCertificadosLine.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EndososCertificadosLine>> SetLinesInSession([FromBody]EndososCertificadosLine _EndososCertificadosLineP)
        {
            try
            {
                List<EndososCertificadosLine> _EndososCertificadosLine = new List<EndososCertificadosLine>();
                _EndososCertificadosLine = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(HttpContext.Session.GetString("listadoproductosEndosos"));

                if (_EndososCertificadosLine == null) { _EndososCertificadosLine = new List<EndososCertificadosLine>(); }
                _EndososCertificadosLine.Add(_EndososCertificadosLineP);
                HttpContext.Session.SetString("listadoproductosEndosos", JsonConvert.SerializeObject(_EndososCertificadosLine).ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return await Task.Run(() => Json(_EndososCertificadosLineP));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososCertificadosLine>> SaveEndososCertificadosLine([FromBody]EndososCertificadosLine _EndososCertificadosLine)
        {

            try
            {
                EndososCertificadosLine _listEndososCertificadosLine = new EndososCertificadosLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososCertificadosLine/GetEndososCertificadosLineById/" + _EndososCertificadosLine.EndososCertificadosLineId);
                string valorrespuesta = "";
               // _EndososCertificadosLine.FechaModificacion = DateTime.Now;
               // _EndososCertificadosLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);
                }

                if (_listEndososCertificadosLine.EndososCertificadosLineId == 0)
                {
                //    _EndososCertificadosLine.FechaCreacion = DateTime.Now;
                   // _EndososCertificadosLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EndososCertificadosLine);
                }
                else
                {
                    var updateresult = await Update(_EndososCertificadosLine.EndososCertificadosLineId, _EndososCertificadosLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososCertificadosLine);
        }

        // POST: EndososCertificadosLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososCertificadosLine>> Insert(EndososCertificadosLine _EndososCertificadosLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _EndososCertificadosLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_EndososCertificadosLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificadosLine/Insert", _EndososCertificadosLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososCertificadosLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificadosLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndososCertificadosLine>> Update(Int64 id, EndososCertificadosLine _EndososCertificadosLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososCertificadosLine/Update", _EndososCertificadosLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososCertificadosLine }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EndososCertificadosLine>> Delete([FromBody]EndososCertificadosLine _EndososCertificadosLine)
        {
            List<EndososCertificadosLine> _EndososCertificadosLineLIST = new List<EndososCertificadosLine>();
            try
            {
                if (_EndososCertificadosLine.EndososCertificadosId > 0)
                {
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                    var result = await _client.PostAsJsonAsync(baseadress + "api/EndososCertificadosLine/Delete", _EndososCertificadosLine);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _EndososCertificadosLine = JsonConvert.DeserializeObject<EndososCertificadosLine>(valorrespuesta);
                    }
                }
                _EndososCertificadosLineLIST = JsonConvert.DeserializeObject<List<EndososCertificadosLine>>(HttpContext.Session.GetString("listadoproductosEndosos"));

                if (_EndososCertificadosLineLIST != null)
                {
                    var item = _EndososCertificadosLineLIST.Find(c => c.EndososCertificadosLineId == _EndososCertificadosLine.EndososCertificadosLineId);
                    _EndososCertificadosLineLIST.Remove(item);
                    HttpContext.Session.SetString("listadoproductosEndosos", JsonConvert.SerializeObject(_EndososCertificadosLineLIST));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return await Task.Run(() => Ok(_EndososCertificadosLineLIST));
        }
    }
}
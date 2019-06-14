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
    public class ControlPalletsLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ControlPalletsLineController(ILogger<ControlPalletsLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwControlPalletsLine(Int64 Id = 0)
        {
            ControlPalletsLine _ControlPalletsLine = new ControlPalletsLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPalletsLine/GetControlPalletsLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPalletsLine = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);

                }

                if (_ControlPalletsLine == null)
                {
                    _ControlPalletsLine = new ControlPalletsLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_ControlPalletsLine);

        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetControlPalletsLineByControlPalletId([DataSourceRequest]DataSourceRequest request,ControlPalletsLine _ControlPalletsLinep)
        {
            List<ControlPalletsLine> _ControlPalletsLine = new List<ControlPalletsLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductos") == null
                   || HttpContext.Session.GetString("listadoproductos") == "")
                {
                    if (_ControlPalletsLinep.ControlPalletsId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_ControlPalletsLinep).ToString();
                        HttpContext.Session.SetString("listadoproductos", serialzado);
                    }
                }
                else
                {
                    _ControlPalletsLine = JsonConvert.DeserializeObject<List<ControlPalletsLine>>(HttpContext.Session.GetString("listadoproductos"));
                }


                if (_ControlPalletsLinep.ControlPalletsId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/ControlPalletsLine/GetControlPalletsLineByControlPalletId/" + _ControlPalletsLinep.ControlPalletsId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _ControlPalletsLine = JsonConvert.DeserializeObject<List<ControlPalletsLine>>(valorrespuesta);
                    }
                }
                else
                {
                    if (_ControlPalletsLinep.Ancho > 0)
                    {
                        _ControlPalletsLine.Add(_ControlPalletsLinep);
                        HttpContext.Session.SetString("listadoproductos", JsonConvert.SerializeObject(_ControlPalletsLine).ToString());
                    }
                }

                //string baseadress = config.Value.urlbase;
                //HttpClient _client = new HttpClient();
                //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //var result = await _client.GetAsync(baseadress + "api/ControlPalletsLine/GetControlPalletsLine");
                //string valorrespuesta = "";
                //if (result.IsSuccessStatusCode)
                //{
                //    valorrespuesta = await (result.Content.ReadAsStringAsync());
                //    _ControlPalletsLine = JsonConvert.DeserializeObject<List<ControlPalletsLine>>(valorrespuesta);

                //}


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _ControlPalletsLine.ToDataSourceResult(request);

        }


        public async Task<ActionResult<ControlPalletsLine>> SaveControlPalletsLine([FromBody]ControlPalletsLine _ControlPalletsLine)
        {

            try
            {
                ControlPalletsLine _listControlPalletsLine = new ControlPalletsLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/ControlPalletsLine/GetControlPalletsLineById/" + _ControlPalletsLine.ControlPalletsLineId);
                string valorrespuesta = "";
                _ControlPalletsLine.FechaModificacion = DateTime.Now;
                _ControlPalletsLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listControlPalletsLine = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);
                }

                if (_listControlPalletsLine.ControlPalletsLineId == 0)
                {
                    _ControlPalletsLine.FechaCreacion = DateTime.Now;
                    _ControlPalletsLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ControlPalletsLine);
                }
                else
                {
                    var updateresult = await Update(_ControlPalletsLine.ControlPalletsLineId, _ControlPalletsLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_ControlPalletsLine);
        }

        // POST: ControlPalletsLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ControlPalletsLine>> Insert(ControlPalletsLine _ControlPalletsLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _ControlPalletsLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                _ControlPalletsLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPalletsLine/Insert", _ControlPalletsLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPalletsLine = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPalletsLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ControlPalletsLine>> Update(Int64 id, ControlPalletsLine _ControlPalletsLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/ControlPalletsLine/Update", _ControlPalletsLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPalletsLine = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPalletsLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ControlPalletsLine>> Delete([FromBody]ControlPalletsLine _ControlPalletsLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/ControlPalletsLine/Delete", _ControlPalletsLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _ControlPalletsLine = JsonConvert.DeserializeObject<ControlPalletsLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _ControlPalletsLine }, Total = 1 });
        }





    }
}
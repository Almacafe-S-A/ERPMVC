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
    public class GoodsDeliveryAuthorizationLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsDeliveryAuthorizationLineController(ILogger<GoodsDeliveryAuthorizationLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwGoodsDeliveryAuthorizationLine(Int64 Id = 0)
        {
            GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/GetGoodsDeliveryAuthorizationLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);

                }

                if (_GoodsDeliveryAuthorizationLine == null)
                {
                    _GoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsDeliveryAuthorizationLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsDeliveryAuthorizationLine> _GoodsDeliveryAuthorizationLine = new List<GoodsDeliveryAuthorizationLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/GetGoodsDeliveryAuthorizationLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorizationLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsDeliveryAuthorizationLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> SaveGoodsDeliveryAuthorizationLine([FromBody]GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine)
        {

            try
            {
                GoodsDeliveryAuthorizationLine _listGoodsDeliveryAuthorizationLine = new GoodsDeliveryAuthorizationLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/GetGoodsDeliveryAuthorizationLineById/" + _GoodsDeliveryAuthorizationLine.GoodsDeliveryAuthorizationLineId);
                string valorrespuesta = "";
               // _GoodsDeliveryAuthorizationLine.FechaModificacion = DateTime.Now;
                //_GoodsDeliveryAuthorizationLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listGoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);
                }

                if (_listGoodsDeliveryAuthorizationLine.GoodsDeliveryAuthorizationLineId == 0)
                {
                  //  _GoodsDeliveryAuthorizationLine.FechaCreacion = DateTime.Now;
                    //_GoodsDeliveryAuthorizationLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_GoodsDeliveryAuthorizationLine);
                }
                else
                {
                    var updateresult = await Update(_GoodsDeliveryAuthorizationLine.GoodsDeliveryAuthorizationLineId, _GoodsDeliveryAuthorizationLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_GoodsDeliveryAuthorizationLine);
        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetGoodsDeliveryAuthorizationLineById([DataSourceRequest]DataSourceRequest request, GoodsDeliveryAuthorizationLine _GoodsReceivedLinep)
        {
            List<GoodsDeliveryAuthorizationLine> _GoodsReceivedLine = new List<GoodsDeliveryAuthorizationLine>();
            try
            {
                if (HttpContext.Session.Get("listadoproductosGoodsDeliveryAuthorization") == null
                   || HttpContext.Session.GetString("listadoproductosGoodsDeliveryAuthorization") == "")
                {
                    if (_GoodsReceivedLinep.GoodsDeliveryAuthorizationId > 0)
                    {
                        string serialzado = JsonConvert.SerializeObject(_GoodsReceivedLinep).ToString();
                        HttpContext.Session.SetString("listadoproductosGoodsDeliveryAuthorization", serialzado);
                    }
                }
                else
                {
                    _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorizationLine>>(HttpContext.Session.GetString("listadoproductosGoodsDeliveryAuthorization"));
                }


                if (_GoodsReceivedLinep.GoodsDeliveryAuthorizationId > 0)
                {

                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();

                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _salesorder.SalesOrderId.ToString());
                    //_client.DefaultRequestHeaders.Add("SalesOrderId", _SalesOrderLine.SalesOrderId.ToString());

                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/GetGoodsReceivedLineByGoodsReceivedId/" + _GoodsReceivedLinep.GoodsDeliveryAuthorizationId);
                    string valorrespuesta = "";
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _GoodsReceivedLine = JsonConvert.DeserializeObject<List<GoodsDeliveryAuthorizationLine>>(valorrespuesta);
                    }
                }
                else
                {
                    if (_GoodsReceivedLinep.Quantity > 0)
                    {
                        _GoodsReceivedLine.Add(_GoodsReceivedLinep);
                        HttpContext.Session.SetString("listadoproductosGoodsDeliveryAuthorization", JsonConvert.SerializeObject(_GoodsReceivedLine).ToString());
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

        // POST: GoodsDeliveryAuthorizationLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> Insert(GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                //_GoodsDeliveryAuthorizationLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                //_GoodsDeliveryAuthorizationLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/Insert", _GoodsDeliveryAuthorizationLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_GoodsDeliveryAuthorizationLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorizationLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> Update(Int64 id, GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/Update", _GoodsDeliveryAuthorizationLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorizationLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsDeliveryAuthorizationLine>> Delete([FromBody]GoodsDeliveryAuthorizationLine _GoodsDeliveryAuthorizationLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsDeliveryAuthorizationLine/Delete", _GoodsDeliveryAuthorizationLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsDeliveryAuthorizationLine = JsonConvert.DeserializeObject<GoodsDeliveryAuthorizationLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsDeliveryAuthorizationLine }, Total = 1 });
        }





    }
}
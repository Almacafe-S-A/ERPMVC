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
    public class GoodsReceivedController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public GoodsReceivedController(ILogger<GoodsReceivedController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwGoodsReceived([FromBody]GoodsReceivedDTO _GoodsReceivedDTO)
        {
            GoodsReceivedDTO _GoodsReceived = new GoodsReceivedDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceivedDTO.GoodsReceivedId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceivedDTO>(valorrespuesta);

                }

                if (_GoodsReceived == null)
                {
                    _GoodsReceived = new GoodsReceivedDTO {  DocumentDate=DateTime.Now,OrderDate=DateTime.Now,editar=1 };
                }
                else
                {
                    _GoodsReceived.editar = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_GoodsReceived);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<GoodsReceived> _GoodsReceived = new List<GoodsReceived>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceived");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<List<GoodsReceived>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _GoodsReceived.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
       public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]GoodsReceived _GoodsReceived)
           //public async Task<ActionResult<GoodsReceived>> SaveGoodsReceived([FromBody]dynamic _GoodsReceived)
        {
            try
            {
                if (_GoodsReceived != null)
                {
                    GoodsReceived _listGoodsReceived = new GoodsReceived();
                   // _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(_GoodsReceived.ToString());
                    string baseadress = config.Value.urlbase;
                    HttpClient _client = new HttpClient();
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    var result = await _client.GetAsync(baseadress + "api/GoodsReceived/GetGoodsReceivedById/" + _GoodsReceived.GoodsReceivedId);
                    string valorrespuesta = "";
                    _GoodsReceived.FechaModificacion = DateTime.Now;
                    _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {
                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _listGoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                    }

                    if (_listGoodsReceived == null)
                    {
                        _listGoodsReceived = new GoodsReceived();
                    }

                    if (_listGoodsReceived.GoodsReceivedId == 0)
                    {
                        _GoodsReceived.FechaCreacion = DateTime.Now;
                        _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_GoodsReceived);
                        var value = (insertresult.Result as ObjectResult).Value;
                        _GoodsReceived = ((GoodsReceived)(value));

                        return _GoodsReceived;
                    }
                    else
                    {
                        //var updateresult = await Update(_GoodsReceived.GoodsReceivedId, _GoodsReceived);
                    }

                   
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return BadRequest("No llego correctamente el modelo!");
        }

        // POST: GoodsReceived/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GoodsReceived>> Insert(GoodsReceived _GoodsReceived)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _GoodsReceived.UsuarioCreacion = HttpContext.Session.GetString("user");
                _GoodsReceived.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Insert", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_GoodsReceived);
            //return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GoodsReceived>> Update(Int64 id, GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/GoodsReceived/Update", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<GoodsReceived>> Delete([FromBody]GoodsReceived _GoodsReceived)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/GoodsReceived/Delete", _GoodsReceived);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _GoodsReceived = JsonConvert.DeserializeObject<GoodsReceived>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _GoodsReceived }, Total = 1 });
        }





    }
}
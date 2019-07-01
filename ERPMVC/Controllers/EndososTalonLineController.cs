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
    public class EndososTalonLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososTalonLineController(ILogger<EndososTalonLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwEndososTalonLine(Int64 Id = 0)
        {
            EndososTalonLine _EndososTalonLine = new EndososTalonLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalonLine/GetEndososTalonLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalonLine = JsonConvert.DeserializeObject<EndososTalonLine>(valorrespuesta);

                }

                if (_EndososTalonLine == null)
                {
                    _EndososTalonLine = new EndososTalonLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EndososTalonLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososTalonLine> _EndososTalonLine = new List<EndososTalonLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalonLine/GetEndososTalonLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalonLine = JsonConvert.DeserializeObject<List<EndososTalonLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososTalonLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososTalonLine>> SaveEndososTalonLine([FromBody]EndososTalonLine _EndososTalonLine)
        {

            try
            {
                EndososTalonLine _listEndososTalonLine = new EndososTalonLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososTalonLine/GetEndososTalonLineById/" + _EndososTalonLine.EndososTalonLineId);
                string valorrespuesta = "";
               // _EndososTalonLine.FechaModificacion = DateTime.Now;
                //_EndososTalonLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEndososTalonLine = JsonConvert.DeserializeObject<EndososTalonLine>(valorrespuesta);
                }

                if (_listEndososTalonLine.EndososTalonLineId == 0)
                {
                    //_EndososTalonLine.FechaCreacion = DateTime.Now;
                    //_EndososTalonLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EndososTalonLine);
                }
                else
                {
                    var updateresult = await Update(_EndososTalonLine.EndososTalonLineId, _EndososTalonLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososTalonLine);
        }

        // POST: EndososTalonLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososTalonLine>> Insert(EndososTalonLine _EndososTalonLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
               // _EndososTalonLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _EndososTalonLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososTalonLine/Insert", _EndososTalonLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalonLine = JsonConvert.DeserializeObject<EndososTalonLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososTalonLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalonLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndososTalonLine>> Update(Int64 id, EndososTalonLine _EndososTalonLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososTalonLine/Update", _EndososTalonLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalonLine = JsonConvert.DeserializeObject<EndososTalonLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalonLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososTalonLine>> Delete([FromBody]EndososTalonLine _EndososTalonLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososTalonLine/Delete", _EndososTalonLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososTalonLine = JsonConvert.DeserializeObject<EndososTalonLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososTalonLine }, Total = 1 });
        }





    }
}
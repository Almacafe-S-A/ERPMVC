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
    public class EndososBonoLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososBonoLineController(ILogger<EndososBonoLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwEndososBonoLine(Int64 Id = 0)
        {
            EndososBonoLine _EndososBonoLine = new EndososBonoLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososBonoLine/GetEndososBonoLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososBonoLine = JsonConvert.DeserializeObject<EndososBonoLine>(valorrespuesta);

                }

                if (_EndososBonoLine == null)
                {
                    _EndososBonoLine = new EndososBonoLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EndososBonoLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososBonoLine> _EndososBonoLine = new List<EndososBonoLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososBonoLine/GetEndososBonoLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososBonoLine = JsonConvert.DeserializeObject<List<EndososBonoLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososBonoLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososBonoLine>> SaveEndososBonoLine([FromBody]EndososBonoLine _EndososBonoLine)
        {

            try
            {
                EndososBonoLine _listEndososBonoLine = new EndososBonoLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososBonoLine/GetEndososBonoLineById/" + _EndososBonoLine.EndososBonoLineId);
                string valorrespuesta = "";
                //_EndososBonoLine.FechaModificacion = DateTime.Now;
                //_EndososBonoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEndososBonoLine = JsonConvert.DeserializeObject<EndososBonoLine>(valorrespuesta);
                }

                if (_listEndososBonoLine.EndososBonoLineId == 0)
                {
                   // _EndososBonoLine.FechaCreacion = DateTime.Now;
                   // _EndososBonoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EndososBonoLine);
                }
                else
                {
                    var updateresult = await Update(_EndososBonoLine.EndososBonoLineId, _EndososBonoLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososBonoLine);
        }

        // POST: EndososBonoLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososBonoLine>> Insert(EndososBonoLine _EndososBonoLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
              //  _EndososBonoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
               // _EndososBonoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososBonoLine/Insert", _EndososBonoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososBonoLine = JsonConvert.DeserializeObject<EndososBonoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososBonoLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososBonoLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndososBonoLine>> Update(Int64 id, EndososBonoLine _EndososBonoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososBonoLine/Update", _EndososBonoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososBonoLine = JsonConvert.DeserializeObject<EndososBonoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososBonoLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososBonoLine>> Delete([FromBody]EndososBonoLine _EndososBonoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososBonoLine/Delete", _EndososBonoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososBonoLine = JsonConvert.DeserializeObject<EndososBonoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososBonoLine }, Total = 1 });
        }





    }
}
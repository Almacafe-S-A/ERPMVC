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
    public class EndososLiberacionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EndososLiberacionController(ILogger<EndososLiberacionController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwEndososLiberacion([FromBody]EndososLiberacion _EndososLiberacionp)
        {
            EndososLiberacion _EndososLiberacion = new EndososLiberacion();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososLiberacion/GetEndososLiberacionById/" + _EndososLiberacionp.EndososLiberacionId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);

                }

                if (_EndososLiberacion == null) { _EndososLiberacion = new EndososLiberacion { FechaLiberacion = DateTime.Now }; };

                if (_EndososLiberacion == null)
                {
                    _EndososLiberacion = new EndososLiberacion();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EndososLiberacion);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EndososLiberacion> _EndososLiberacion = new List<EndososLiberacion>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososLiberacion/GetEndososLiberacion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<List<EndososLiberacion>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EndososLiberacion.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetEndososLiberacionByEndososId([DataSourceRequest]DataSourceRequest request, int EndososCertificadosId)
        {
            List<EndososLiberacion> _EndososLiberacion = new List<EndososLiberacion>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososLiberacion/GetEndososLiberacion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<List<EndososLiberacion>>(valorrespuesta);
                    _EndososLiberacion = _EndososLiberacion.Where(p => p.EndososId == EndososCertificadosId).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _EndososLiberacion.ToDataSourceResult(request);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EndososLiberacion>> SaveEndososLiberacion([FromBody]EndososLiberacion _EndososLiberacion)
        {

            try
            {

                if(_EndososLiberacion.Quantity < _EndososLiberacion.Saldo)
                {
                    return await Task.Run(() => BadRequest("El saldo a Liberar no puede ser mayor que la cantidad endosada"));
                }

                //_EndososLiberacion.Saldo = _EndososLiberacion.Saldo - _EndososLiberacion.Quantity;

                EndososLiberacion _listEndososLiberacion = new EndososLiberacion();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EndososLiberacion/GetEndososLiberacionById/" + _EndososLiberacion.EndososLiberacionId);
                string valorrespuesta = "";
                _EndososLiberacion.FechaModificacion = DateTime.Now;
                _EndososLiberacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);
                }

                if(_listEndososLiberacion==null)
                {
                    _listEndososLiberacion = new EndososLiberacion();
                }

                if (_listEndososLiberacion.EndososLiberacionId == 0)
                {
                    if (_EndososLiberacion.Saldo > 0)
                    {
                        _EndososLiberacion.FechaLiberacion = DateTime.Now;
                        _EndososLiberacion.FechaCreacion = DateTime.Now;
                        _EndososLiberacion.UsuarioCreacion = HttpContext.Session.GetString("user");
                        var insertresult = await Insert(_EndososLiberacion);
                    }
                }
                else
                {
                    var updateresult = await Update(_EndososLiberacion.EndososLiberacionId, _EndososLiberacion);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EndososLiberacion);
        }

        // POST: EndososLiberacion/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EndososLiberacion>> Insert(EndososLiberacion _EndososLiberacion)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EndososLiberacion.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EndososLiberacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososLiberacion/Insert", _EndososLiberacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EndososLiberacion);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EndososLiberacion }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<EndososLiberacion>> Update(Int64 id, [FromBody]EndososLiberacion _EndososLiberacion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EndososLiberacion/Update", _EndososLiberacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososLiberacion }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<EndososLiberacion>> Delete([FromBody]EndososLiberacion _EndososLiberacion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EndososLiberacion/Delete", _EndososLiberacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EndososLiberacion = JsonConvert.DeserializeObject<EndososLiberacion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EndososLiberacion }, Total = 1 });
        }





    }
}
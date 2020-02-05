using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.Helpers;
using ERPMVC.Models;
using ERPMVC.DTO;
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
    public class EscalaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EscalaController(ILogger<EscalaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Escala()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> pvwAddEscala([FromBody]EscalaDTO _sarpara)

        {
            EscalaDTO _Escala = new EscalaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Escala/GetEscalaById/" + _sarpara.IdEscala);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<EscalaDTO>(valorrespuesta);

                }

                if (_Escala == null)
                {
                    _Escala = new EscalaDTO ();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Escala);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<Escala> _Escala = new List<Escala>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Escala/GetEscala");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<List<Escala>>(valorrespuesta);
                    _Escala = _Escala.OrderByDescending(e => e.IdEscala).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Escala.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Escala>> SaveEscala([FromBody]EscalaDTO _EscalaS)
        {
            Escala _Escala = _EscalaS;
            try
            {
                Escala _listEscala = new Escala();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Escala/GetEscalaById/" + _Escala.IdEscala);
                string valorrespuesta = "";
                _Escala.FechaModificacion = DateTime.Now;
                _Escala.Usuariomodificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<EscalaDTO>(valorrespuesta);
                }

                if (_Escala == null) { _Escala = new Models.Escala(); }

                if (_EscalaS.IdEscala == 0)
                {
                    _EscalaS.FechaCreacion = DateTime.Now;
                    _EscalaS.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EscalaS);
                }
                else
                {
                    _EscalaS.Usuariocreacion = _Escala.Usuariocreacion;
                    _EscalaS.FechaCreacion = _Escala.FechaCreacion;
                    var updateresult = await Update(_Escala.IdEscala, _EscalaS);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Escala);
        }

        // POST: Escala/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Escala>> Insert(Escala _Escalap)
        {
            Escala _Escala = _Escalap;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Escala.Usuariocreacion = HttpContext.Session.GetString("user");
                _Escala.Usuariomodificacion = HttpContext.Session.GetString("user");
                _Escala.FechaCreacion = DateTime.Now;
                _Escala.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Escala/Insert", _Escala);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<Escala>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Escala }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Escala>> Update(Int64 id, Escala _Escalap)
        {
            Escala _Escala = _Escalap;
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Escala.FechaModificacion = DateTime.Now;
                _Escala.Usuariomodificacion = HttpContext.Session.GetString("user");
                var result = await _client.PutAsJsonAsync(baseadress + "api/Escala/Update", _Escala);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {                    
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<Escala>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Escala }, Total = 1 });
        }

        
        [HttpPost]
        public async Task<ActionResult<Escala>> Delete(Int64 IdEscala, Escala _Escala)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Escala/Delete", _Escala);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<Escala>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Escala }, Total = 1 });
        }





    }
}
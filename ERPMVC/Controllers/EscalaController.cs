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
    public class EscalaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EscalaController(ILogger<EscalaController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwEscala(Int64 Id = 0)
        {
            Escala _Escala = new Escala();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Escala/GetEscalaById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Escala = JsonConvert.DeserializeObject<Escala>(valorrespuesta);

                }

                if (_Escala == null)
                {
                    _Escala = new Escala();
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
        public async Task<ActionResult<Escala>> SaveEscala([FromBody]Escala _Escala)
        {

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
                    _listEscala = JsonConvert.DeserializeObject<Escala>(valorrespuesta);
                }

                if (_listEscala.IdEscala == 0)
                {
                    _Escala.FechaCreacion = DateTime.Now;
                    _Escala.Usuariocreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Escala);
                }
                else
                {
                    var updateresult = await Update(_Escala.IdEscala, _Escala);
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
        public async Task<ActionResult<Escala>> Insert(Escala _Escala)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Escala.Usuariocreacion = HttpContext.Session.GetString("user");
                _Escala.Usuariomodificacion = HttpContext.Session.GetString("user");
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
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Escala);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Escala }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Escala>> Update(Int64 id, Escala _Escala)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

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
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Escala }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Escala>> Delete([FromBody]Escala _Escala)
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
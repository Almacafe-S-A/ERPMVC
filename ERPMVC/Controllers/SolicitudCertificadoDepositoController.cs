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
    public class SolicitudCertificadoDepositoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public SolicitudCertificadoDepositoController(ILogger<SolicitudCertificadoDepositoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwSolicitudCertificadoDeposito(Int64 Id = 0)
        {
            SolicitudCertificadoDeposito _SolicitudCertificadoDeposito = new SolicitudCertificadoDeposito();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoDeposito/GetSolicitudCertificadoDepositoById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoDeposito = JsonConvert.DeserializeObject<SolicitudCertificadoDeposito>(valorrespuesta);

                }

                if (_SolicitudCertificadoDeposito == null)
                {
                    _SolicitudCertificadoDeposito = new SolicitudCertificadoDeposito();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_SolicitudCertificadoDeposito);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<SolicitudCertificadoDeposito> _SolicitudCertificadoDeposito = new List<SolicitudCertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoDeposito/GetSolicitudCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoDeposito = JsonConvert.DeserializeObject<List<SolicitudCertificadoDeposito>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SolicitudCertificadoDeposito.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SolicitudCertificadoDeposito>> SaveSolicitudCertificadoDeposito([FromBody]SolicitudCertificadoDeposito _SolicitudCertificadoDeposito)
        {

            try
            {
                SolicitudCertificadoDeposito _listSolicitudCertificadoDeposito = new SolicitudCertificadoDeposito();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoDeposito/GetSolicitudCertificadoDepositoById/" + _SolicitudCertificadoDeposito.IdCD);
                string valorrespuesta = "";
                _SolicitudCertificadoDeposito.FechaModificacion = DateTime.Now;
                _SolicitudCertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listSolicitudCertificadoDeposito = JsonConvert.DeserializeObject<SolicitudCertificadoDeposito>(valorrespuesta);
                }

                if (_listSolicitudCertificadoDeposito.IdCD == 0)
                {
                    _SolicitudCertificadoDeposito.FechaCreacion = DateTime.Now;
                    _SolicitudCertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_SolicitudCertificadoDeposito);
                }
                else
                {
                    var updateresult = await Update(_SolicitudCertificadoDeposito.IdCD, _SolicitudCertificadoDeposito);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SolicitudCertificadoDeposito);
        }

        // POST: SolicitudCertificadoDeposito/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SolicitudCertificadoDeposito>> Insert(SolicitudCertificadoDeposito _SolicitudCertificadoDeposito)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _SolicitudCertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                _SolicitudCertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/SolicitudCertificadoDeposito/Insert", _SolicitudCertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoDeposito = JsonConvert.DeserializeObject<SolicitudCertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_SolicitudCertificadoDeposito);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoDeposito }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SolicitudCertificadoDeposito>> Update(Int64 id, SolicitudCertificadoDeposito _SolicitudCertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/SolicitudCertificadoDeposito/Update", _SolicitudCertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoDeposito = JsonConvert.DeserializeObject<SolicitudCertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoDeposito }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SolicitudCertificadoDeposito>> Delete([FromBody]SolicitudCertificadoDeposito _SolicitudCertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/SolicitudCertificadoDeposito/Delete", _SolicitudCertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoDeposito = JsonConvert.DeserializeObject<SolicitudCertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoDeposito }, Total = 1 });
        }





    }
}
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
    public class SolicitudCertificadoLineController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public SolicitudCertificadoLineController(ILogger<SolicitudCertificadoLineController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwSolicitudCertificadoLine(Int64 Id = 0)
        {
            SolicitudCertificadoLine _SolicitudCertificadoLine = new SolicitudCertificadoLine();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoLine/GetSolicitudCertificadoLineById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoLine = JsonConvert.DeserializeObject<SolicitudCertificadoLine>(valorrespuesta);

                }

                if (_SolicitudCertificadoLine == null)
                {
                    _SolicitudCertificadoLine = new SolicitudCertificadoLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_SolicitudCertificadoLine);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<SolicitudCertificadoLine> _SolicitudCertificadoLine = new List<SolicitudCertificadoLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoLine/GetSolicitudCertificadoLine");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoLine = JsonConvert.DeserializeObject<List<SolicitudCertificadoLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _SolicitudCertificadoLine.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SolicitudCertificadoLine>> SaveSolicitudCertificadoLine([FromBody]SolicitudCertificadoLine _SolicitudCertificadoLine)
        {

            try
            {
                SolicitudCertificadoLine _listSolicitudCertificadoLine = new SolicitudCertificadoLine();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/SolicitudCertificadoLine/GetSolicitudCertificadoLineById/" + _SolicitudCertificadoLine.CertificadoLineId);
                string valorrespuesta = "";
                //_SolicitudCertificadoLine.FechaModificacion = DateTime.Now;
                //_SolicitudCertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listSolicitudCertificadoLine = JsonConvert.DeserializeObject<SolicitudCertificadoLine>(valorrespuesta);
                }

                if (_listSolicitudCertificadoLine.CertificadoLineId == 0)
                {
                   // _SolicitudCertificadoLine.FechaCreacion = DateTime.Now;
                    //_SolicitudCertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_SolicitudCertificadoLine);
                }
                else
                {
                    var updateresult = await Update(_SolicitudCertificadoLine.CertificadoLineId, _SolicitudCertificadoLine);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_SolicitudCertificadoLine);
        }

        // POST: SolicitudCertificadoLine/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SolicitudCertificadoLine>> Insert(SolicitudCertificadoLine _SolicitudCertificadoLine)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
             //   _SolicitudCertificadoLine.UsuarioCreacion = HttpContext.Session.GetString("user");
              //  _SolicitudCertificadoLine.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/SolicitudCertificadoLine/Insert", _SolicitudCertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoLine = JsonConvert.DeserializeObject<SolicitudCertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_SolicitudCertificadoLine);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoLine }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SolicitudCertificadoLine>> Update(Int64 id, SolicitudCertificadoLine _SolicitudCertificadoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/SolicitudCertificadoLine/Update", _SolicitudCertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoLine = JsonConvert.DeserializeObject<SolicitudCertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoLine }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SolicitudCertificadoLine>> Delete([FromBody]SolicitudCertificadoLine _SolicitudCertificadoLine)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/SolicitudCertificadoLine/Delete", _SolicitudCertificadoLine);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _SolicitudCertificadoLine = JsonConvert.DeserializeObject<SolicitudCertificadoLine>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _SolicitudCertificadoLine }, Total = 1 });
        }





    }
}
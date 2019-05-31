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
    public class CertificadoDepositoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public CertificadoDepositoController(ILogger<CertificadoDepositoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwCertificadoDeposito(Int64 Id = 0)
        {
            CertificadoDeposito _CertificadoDeposito = new CertificadoDeposito();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);

                }

                if (_CertificadoDeposito == null)
                {
                    _CertificadoDeposito = new CertificadoDeposito();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_CertificadoDeposito);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<CertificadoDeposito> _CertificadoDeposito = new List<CertificadoDeposito>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDeposito");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<List<CertificadoDeposito>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _CertificadoDeposito.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CertificadoDeposito>> SaveCertificadoDeposito([FromBody]CertificadoDeposito _CertificadoDeposito)
        {

            try
            {
                CertificadoDeposito _listCertificadoDeposito = new CertificadoDeposito();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/CertificadoDeposito/GetCertificadoDepositoById/" + _CertificadoDeposito.IdCD);
                string valorrespuesta = "";
                _CertificadoDeposito.FechaModificacion = DateTime.Now;
                _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listCertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

                if (_listCertificadoDeposito.IdCD == 0)
                {
                    _CertificadoDeposito.FechaCreacion = DateTime.Now;
                    _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_CertificadoDeposito);
                }
                else
                {
                    var updateresult = await Update(_CertificadoDeposito.IdCD, _CertificadoDeposito);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_CertificadoDeposito);
        }

        // POST: CertificadoDeposito/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CertificadoDeposito>> Insert(CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _CertificadoDeposito.UsuarioCreacion = HttpContext.Session.GetString("user");
                _CertificadoDeposito.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Insert", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_CertificadoDeposito);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CertificadoDeposito>> Update(Int64 id, CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/CertificadoDeposito/Update", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CertificadoDeposito>> Delete([FromBody]CertificadoDeposito _CertificadoDeposito)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/CertificadoDeposito/Delete", _CertificadoDeposito);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _CertificadoDeposito = JsonConvert.DeserializeObject<CertificadoDeposito>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _CertificadoDeposito }, Total = 1 });
        }





    }
}
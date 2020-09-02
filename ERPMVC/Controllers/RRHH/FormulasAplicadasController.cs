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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class FormulasAplicadasController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FormulasAplicadasController(ILogger<FormulasAplicadasController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FormulasAplicadas()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<FormulasAplicadas> _FormulasAplicadas = new List<FormulasAplicadas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasAplicadas/GetFormulasAplicadas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<List<FormulasAplicadas>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FormulasAplicadas.ToDataSourceResult(request);

        }
        public async Task<ActionResult> pvwFormulasAplicadas([FromBody]FormulasAplicadasDTO _sarpara)
        {
            FormulasAplicadasDTO _FormulasAplicadas = new FormulasAplicadasDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasAplicadas/GetFormulasAplicadasById/" + _sarpara.IdFormulaAplicada);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<FormulasAplicadasDTO>(valorrespuesta);

                }

                if (_FormulasAplicadas == null)
                {
                    _FormulasAplicadas = new FormulasAplicadasDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_FormulasAplicadas);

        }
        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetFormulasAplicadasByEmployeeId([DataSourceRequest]DataSourceRequest request, Int64 EmployeeId)
        {
            List<FormulasAplicadas> _FormulasAplicadas = new List<FormulasAplicadas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasAplicadas/GetFormulasAplicadasByEmployeeId/" + EmployeeId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<List<FormulasAplicadas>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FormulasAplicadas.ToDataSourceResult(request);

        }


        [HttpPost("[action]")]
        public async Task<ActionResult<FormulasAplicadas>> SaveFormulasAplicadas([FromBody]FormulasAplicadas _FormulasAplicadas)
        {

            try
            {
                FormulasAplicadas _listFormulasAplicadas = new FormulasAplicadas();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasAplicadas/GetFormulasAplicadasById/" + _FormulasAplicadas.IdFormulaAplicada);
                string valorrespuesta = "";
                _FormulasAplicadas.FechaModificacion = DateTime.Now;
                _FormulasAplicadas.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFormulasAplicadas = JsonConvert.DeserializeObject<FormulasAplicadas>(valorrespuesta);
                }

                if (_listFormulasAplicadas == null) { _listFormulasAplicadas = new FormulasAplicadas(); }

                if (_listFormulasAplicadas.IdFormulaAplicada == 0)
                {
                    _FormulasAplicadas.FechaCreacion = DateTime.Now;
                    _FormulasAplicadas.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_FormulasAplicadas);
                }
                else
                {
                    var updateresult = await Update(_FormulasAplicadas.IdFormulaAplicada, _FormulasAplicadas);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_FormulasAplicadas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<FormulasAplicadas>> Insert(FormulasAplicadas _FormulasAplicadas)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _FormulasAplicadas.UsuarioCreacion = HttpContext.Session.GetString("user");
                _FormulasAplicadas.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasAplicadas/Insert", _FormulasAplicadas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<FormulasAplicadas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_FormulasAplicadas);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasAplicadas }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FormulasAplicadas>> Update(Int64 id, FormulasAplicadas _FormulasAplicadas)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/FormulasAplicadas/Update", _FormulasAplicadas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<FormulasAplicadas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_FormulasAplicadas);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasAplicadas }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<FormulasAplicadas>> Delete(Int64 IdFormulasAplicadas, FormulasAplicadas _FormulasAplicadas)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasAplicadas/Delete", _FormulasAplicadas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasAplicadas = JsonConvert.DeserializeObject<FormulasAplicadas>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasAplicadas }, Total = 1 });
        }

    }
}
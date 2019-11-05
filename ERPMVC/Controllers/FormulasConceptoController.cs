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
    public class FormulasConceptoController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FormulasConceptoController(ILogger<FormulasConceptoController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<FormulasConcepto> _FormulasConcepto = new List<FormulasConcepto>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConcepto/GetFormulasConcepto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConcepto = JsonConvert.DeserializeObject<List<FormulasConcepto>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FormulasConcepto.ToDataSourceResult(request);

        }
        public async Task<ActionResult> pvwFormulasConcepto([FromBody]FormulasConceptoDTO _sarpara)
        {
            FormulasConceptoDTO _FormulasConcepto = new FormulasConceptoDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConcepto/GetFormulasConceptoById/" + _sarpara.IdformulaConcepto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConcepto = JsonConvert.DeserializeObject<FormulasConceptoDTO>(valorrespuesta);

                }

                if (_FormulasConcepto == null)
                {
                    _FormulasConcepto = new FormulasConceptoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_FormulasConcepto);

        }



        [HttpPost("[action]")]
        public async Task<ActionResult<FormulasConcepto>> SaveFormulasConcepto([FromBody]FormulasConcepto _FormulasConcepto)
        {

            try
            {
                FormulasConcepto _listFormulasConcepto = new FormulasConcepto();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConcepto/GetFormulasConceptoById/" + _FormulasConcepto.IdformulaConcepto);
                string valorrespuesta = "";
                _FormulasConcepto.FechaModificacion = DateTime.Now;
                _FormulasConcepto.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFormulasConcepto = JsonConvert.DeserializeObject<FormulasConcepto>(valorrespuesta);
                }

                if (_listFormulasConcepto == null) { _listFormulasConcepto = new FormulasConcepto(); }

                if (_listFormulasConcepto.IdformulaConcepto == 0)
                {
                    _FormulasConcepto.FechaCreacion = DateTime.Now;
                    _FormulasConcepto.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_FormulasConcepto);
                }
                else
                {
                    var updateresult = await Update(_FormulasConcepto.IdformulaConcepto, _FormulasConcepto);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_FormulasConcepto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<FormulasConcepto>> Insert(FormulasConcepto _FormulasConcepto)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _FormulasConcepto.UsuarioCreacion = HttpContext.Session.GetString("user");
                _FormulasConcepto.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasConcepto/Insert", _FormulasConcepto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConcepto = JsonConvert.DeserializeObject<FormulasConcepto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_FormulasConcepto);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConcepto }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FormulasConcepto>> Update(Int64 id, FormulasConcepto _FormulasConcepto)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/FormulasConcepto/Update", _FormulasConcepto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConcepto = JsonConvert.DeserializeObject<FormulasConcepto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_FormulasConcepto);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConcepto }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<FormulasConcepto>> Delete(Int64 IdFormulasConcepto, FormulasConcepto _FormulasConcepto)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasConcepto/Delete", _FormulasConcepto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConcepto = JsonConvert.DeserializeObject<FormulasConcepto>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConcepto }, Total = 1 });
        }

    }
}
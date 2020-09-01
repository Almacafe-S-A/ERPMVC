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
    public class FormulasConFormulasController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FormulasConFormulasController(ILogger<FormulasConFormulasController> logger, IOptions<MyConfig> config)
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
            List<FormulasConFormulas> _FormulasConFormulas = new List<FormulasConFormulas>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConFormulas/GetFormulasConFormulas");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConFormulas = JsonConvert.DeserializeObject<List<FormulasConFormulas>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _FormulasConFormulas.ToDataSourceResult(request);

        }
        public async Task<ActionResult> pvwFormulasConFormulas([FromBody]FormulasConFormulasDTO _sarpara)
        {
            FormulasConFormulasDTO _FormulasConFormulas = new FormulasConFormulasDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConFormulas/GetFormulasConFormulasById/" + _sarpara.IdFormulaconformula);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConFormulas = JsonConvert.DeserializeObject<FormulasConFormulasDTO>(valorrespuesta);

                }

                if (_FormulasConFormulas == null)
                {
                    _FormulasConFormulas = new FormulasConFormulasDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_FormulasConFormulas);

        }



        [HttpPost("[action]")]
        public async Task<ActionResult<FormulasConFormulas>> SaveFormulasConFormulas([FromBody]FormulasConFormulas _FormulasConFormulas)
        {

            try
            {
                FormulasConFormulas _listFormulasConFormulas = new FormulasConFormulas();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/FormulasConFormulas/GetFormulasConFormulasById/" + _FormulasConFormulas.IdFormulaconformula);
                string valorrespuesta = "";
                _FormulasConFormulas.Fechamodificacion = DateTime.Now;
                _FormulasConFormulas.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFormulasConFormulas = JsonConvert.DeserializeObject<FormulasConFormulas>(valorrespuesta);
                }

                if (_listFormulasConFormulas == null) { _listFormulasConFormulas = new FormulasConFormulas(); }

                if (_listFormulasConFormulas.IdFormulaconformula == 0)
                {
                    _FormulasConFormulas.FechaCreacion = DateTime.Now;
                    _FormulasConFormulas.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_FormulasConFormulas);
                }
                else
                {
                    var updateresult = await Update(_FormulasConFormulas.IdFormulaconformula, _FormulasConFormulas);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_FormulasConFormulas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<FormulasConFormulas>> Insert(FormulasConFormulas _FormulasConFormulas)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _FormulasConFormulas.UsuarioCreacion = HttpContext.Session.GetString("user");
                _FormulasConFormulas.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasConFormulas/Insert", _FormulasConFormulas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConFormulas = JsonConvert.DeserializeObject<FormulasConFormulas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_FormulasConFormulas);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConFormulas }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FormulasConFormulas>> Update(Int64 id, FormulasConFormulas _FormulasConFormulas)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/FormulasConFormulas/Update", _FormulasConFormulas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConFormulas = JsonConvert.DeserializeObject<FormulasConFormulas>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_FormulasConFormulas);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConFormulas }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<FormulasConFormulas>> Delete(Int64 IdFormulasConFormulas, FormulasConFormulas _FormulasConFormulas)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/FormulasConFormulas/Delete", _FormulasConFormulas);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _FormulasConFormulas = JsonConvert.DeserializeObject<FormulasConFormulas>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _FormulasConFormulas }, Total = 1 });
        }

    }
}
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
    public class FormulaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public FormulaController(ILogger<FormulaController> logger, IOptions<MyConfig> config)
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
            List<Formula> _Formula = new List<Formula>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Formula/GetFormula");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Formula = JsonConvert.DeserializeObject<List<Formula>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Formula.ToDataSourceResult(request);

        }
        public async Task<ActionResult> pvwFormula([FromBody]FormulaDTO _sarpara)
        {
            FormulaDTO _Formula = new FormulaDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Formula/GetFormulaById/" + _sarpara.IdFormula);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Formula = JsonConvert.DeserializeObject<FormulaDTO>(valorrespuesta);

                }

                if (_Formula == null)
                {
                    _Formula = new FormulaDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_Formula);

        }



        [HttpPost("[action]")]
        public async Task<ActionResult<Formula>> SaveFormula([FromBody]Formula _Formula)
        {

            try
            {
                Formula _listFormula = new Formula();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Formula/GetFormulaById/" + _Formula.IdFormula);
                string valorrespuesta = "";
                _Formula.FechaModificacion = DateTime.Now;
                _Formula.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listFormula = JsonConvert.DeserializeObject<Formula>(valorrespuesta);
                }

                if (_listFormula == null) { _listFormula = new Formula(); }

                if (_listFormula.IdFormula == 0)
                {
                    _Formula.FechaCreacion = DateTime.Now;
                    _Formula.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_Formula);
                }
                else
                {
                    var updateresult = await Update(_Formula.IdFormula, _Formula);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Formula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Formula>> Insert(Formula _Formula)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Formula.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Formula.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/Formula/Insert", _Formula);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Formula = JsonConvert.DeserializeObject<Formula>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }
            return Ok(_Formula);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Formula }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Formula>> Update(Int64 id, Formula _Formula)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Formula/Update", _Formula);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Formula = JsonConvert.DeserializeObject<Formula>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return await Task.Run(() => BadRequest($"Ocurrio un error{ex.Message}"));
            }

            return Ok(_Formula);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _Formula }, Total = 1 });
        }


        [HttpPost]
        public async Task<ActionResult<Formula>> Delete(Int64 IdFormula, Formula _Formula)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Formula/Delete", _Formula);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Formula = JsonConvert.DeserializeObject<Formula>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _Formula }, Total = 1 });
        }

    }
}
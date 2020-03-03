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
    public class ConceptController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public ConceptController(ILogger<BankController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [Authorize(Policy = "RRHH.Concepto")]
        public IActionResult Concept()
        {
            return View();
        }


        public async Task<ActionResult> pvwAddConcept([FromBody]ConceptDTO _sarpara)
        {
            ConceptDTO _Concept = new ConceptDTO();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Concept/GetConceptById/" + _sarpara.ConceptId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<ConceptDTO>(valorrespuesta);
                }
                if (_Concept == null)
                {
                    _Concept = new ConceptDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return PartialView(_Concept);
        }


        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetConcept([DataSourceRequest]DataSourceRequest request)
        {
            List<Concept> _Concept = new List<Concept>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Concept/GetConcept");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<List<Concept>>(valorrespuesta);
                    _Concept = _Concept.OrderByDescending(q => q.ConceptId).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return _Concept.ToDataSourceResult(request);
        }

        public async Task<ActionResult<Concept>> SaveConcept([FromBody]ConceptDTO _ConceptP)
        {
            Concept _Concept = _ConceptP;
            try
            {
                Concept _listAccount = new Concept();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Concept/GetConceptByConceptValues/" + _Concept.ConceptName + "/" + _Concept.Calculation + "/" + _Concept.Value + "/" + _Concept.TypeId);
                string valorrespuesta = "";
                _Concept.FechaModificacion = DateTime.Now;
                _Concept.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<Concept>(valorrespuesta);
                }

                if (_Concept == null) { _Concept = new Models.Concept(); }
                if (_Concept.ConceptId > 0)
                {
                    return await Task.Run(() => BadRequest($"Ya existe un concepto registrado con los mismos valores."));
                }
                if (_ConceptP.ConceptId == 0)
                {
                    _Concept.FechaCreacion = DateTime.Now;
                    _Concept.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_ConceptP);
                }
                else
                {
                    _ConceptP.UsuarioCreacion = _Concept.UsuarioCreacion;
                    _ConceptP.FechaCreacion = _Concept.FechaCreacion;
                    var updateresult = await Update(_Concept.ConceptId, _ConceptP);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }
            return Json(_ConceptP);
        }


        // POST: Concept/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Concept>> Insert(Concept _Concept)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Concept.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Concept.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Concept.FechaCreacion = DateTime.Now;
                _Concept.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Concept/Insert", _Concept);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<Concept>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Concept);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Concept>> Update(Int64 id, Concept _Concept)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PutAsJsonAsync(baseadress + "api/Concept/Update", _Concept);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<Concept>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _Concept }, Total = 1 });
        }



        [HttpPost]
        public async Task<ActionResult<Concept>> Delete(Int64 ConceptId, Concept _Concept)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.PostAsJsonAsync(baseadress + "api/Concept/Delete", _Concept);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Concept = JsonConvert.DeserializeObject<Concept>(valorrespuesta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }
            return new ObjectResult(new DataSourceResult { Data = new[] { _Concept }, Total = 1 });
        }

    }
}
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
    public class EmployeeAbsenceController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public EmployeeAbsenceController(ILogger<EmployeeAbsenceController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwEmployeeAbsence(Int64 Id = 0)
        {
            EmployeeAbsence _EmployeeAbsence = new EmployeeAbsence();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeAbsence/GetEmployeeAbsenceById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeAbsence = JsonConvert.DeserializeObject<EmployeeAbsence>(valorrespuesta);

                }

                if (_EmployeeAbsence == null)
                {
                    _EmployeeAbsence = new EmployeeAbsence();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_EmployeeAbsence);

        }


        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<EmployeeAbsence> _EmployeeAbsence = new List<EmployeeAbsence>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeAbsence/GetEmployeeAbsence");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeAbsence = JsonConvert.DeserializeObject<List<EmployeeAbsence>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _EmployeeAbsence.ToDataSourceResult(request);

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeAbsence>> SaveEmployeeAbsence([FromBody]EmployeeAbsence _EmployeeAbsence)
        {

            try
            {
                EmployeeAbsence _listEmployeeAbsence = new EmployeeAbsence();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/EmployeeAbsence/GetEmployeeAbsenceById/" + _EmployeeAbsence.EmployeeAbsenceId);
                string valorrespuesta = "";
                _EmployeeAbsence.FechaModificacion = DateTime.Now;
                _EmployeeAbsence.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listEmployeeAbsence = JsonConvert.DeserializeObject<EmployeeAbsence>(valorrespuesta);
                }

                if (_listEmployeeAbsence.EmployeeAbsenceId == 0)
                {
                    _EmployeeAbsence.FechaCreacion = DateTime.Now;
                    _EmployeeAbsence.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_EmployeeAbsence);
                }
                else
                {
                    var updateresult = await Update(_EmployeeAbsence.EmployeeAbsenceId, _EmployeeAbsence);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_EmployeeAbsence);
        }

        // POST: EmployeeAbsence/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<EmployeeAbsence>> Insert(EmployeeAbsence _EmployeeAbsence)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _EmployeeAbsence.UsuarioCreacion = HttpContext.Session.GetString("user");
                _EmployeeAbsence.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeAbsence/Insert", _EmployeeAbsence);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeAbsence = JsonConvert.DeserializeObject<EmployeeAbsence>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_EmployeeAbsence);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeAbsence }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeAbsence>> Update(Int64 id, EmployeeAbsence _EmployeeAbsence)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/EmployeeAbsence/Update", _EmployeeAbsence);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeAbsence = JsonConvert.DeserializeObject<EmployeeAbsence>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeAbsence }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<EmployeeAbsence>> Delete([FromBody]EmployeeAbsence _EmployeeAbsence)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/EmployeeAbsence/Delete", _EmployeeAbsence);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _EmployeeAbsence = JsonConvert.DeserializeObject<EmployeeAbsence>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _EmployeeAbsence }, Total = 1 });
        }





    }
}
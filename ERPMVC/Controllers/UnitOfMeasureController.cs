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
    public class UnitOfMeasureController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public UnitOfMeasureController(ILogger<UnitOfMeasureController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult UnitOfMeasure()
        {
            return View();
        }

        public async Task<ActionResult> pvwUnitOfMeasure(Int64 Id = 0)
        {
            UnitOfMeasure _UnitOfMeasure = new UnitOfMeasure();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/UnitOfMeasure/GetUnitOfMeasureById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);

                }

                if (_UnitOfMeasure == null)
                {
                    _UnitOfMeasure = new UnitOfMeasure();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_UnitOfMeasure);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetUoM([DataSourceRequest]DataSourceRequest request)
        {
            List<UnitOfMeasure> _UnitOfMeasure = new List<UnitOfMeasure>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/UnitOfMeasure/GetUnitOfMeasure");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<List<UnitOfMeasure>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _UnitOfMeasure.ToDataSourceResult(request);

        }


        [HttpPost]
        public async Task<ActionResult<UnitOfMeasure>> SaveUnitOfMeasure([FromBody]UnitOfMeasure _UnitOfMeasure)
        {

            try
            {
                UnitOfMeasure _listUnitOfMeasure = new UnitOfMeasure();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/UnitOfMeasure/GetUnitOfMeasureById/" + _UnitOfMeasure.UnitOfMeasureId);
                string valorrespuesta = "";
                _UnitOfMeasure.FechaModificacion = DateTime.Now;
                _UnitOfMeasure.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listUnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);
                }

                if (_listUnitOfMeasure.UnitOfMeasureId == 0)
                {
                    _UnitOfMeasure.FechaCreacion = DateTime.Now;
                    _UnitOfMeasure.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_UnitOfMeasure);
                }
                else
                {
                    var updateresult = await Update(_UnitOfMeasure.UnitOfMeasureId, _UnitOfMeasure);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_UnitOfMeasure);
        }

        // POST: UnitOfMeasure/Insert
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<UnitOfMeasure>> Insert(UnitOfMeasure _UnitOfMeasure)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _UnitOfMeasure.UsuarioCreacion = HttpContext.Session.GetString("user");
                _UnitOfMeasure.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/UnitOfMeasure/Insert", _UnitOfMeasure);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_UnitOfMeasure);
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _UnitOfMeasure }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<UnitOfMeasure>> Update(Int64 id, UnitOfMeasure _UnitOfMeasure)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/UnitOfMeasure/Update", _UnitOfMeasure);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_UnitOfMeasure);
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _UnitOfMeasure }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UnitOfMeasure>> Delete([FromBody]UnitOfMeasure _UnitOfMeasure)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/UnitOfMeasure/Delete", _UnitOfMeasure);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _UnitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }


            return Ok(_UnitOfMeasure);
            //  return new ObjectResult(new DataSourceResult { Data = new[] { _UnitOfMeasure }, Total = 1 });
        }





    }
}
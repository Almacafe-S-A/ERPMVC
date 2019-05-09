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
    public class NumeracionSARController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public NumeracionSARController(ILogger<NumeracionSARController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        public IActionResult NumeracionSAR()
        {
            return View();
        }

        public async Task<ActionResult> pvwNumeracionSAR(Int64 Id = 0)
        {
            NumeracionSAR _NumeracionSAR = new NumeracionSAR();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/NumeracionSAR/GetNumeracionSARById/" + Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<NumeracionSAR>(valorrespuesta);

                }

                if (_NumeracionSAR == null)
                {
                    _NumeracionSAR = new NumeracionSAR();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_NumeracionSAR);

        }


        [HttpGet]
        public async Task<DataSourceResult> GetNumeracioSAR([DataSourceRequest]DataSourceRequest request)
        {
            List<NumeracionSAR> _NumeracionSAR = new List<NumeracionSAR>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/NumeracionSAR/GetNumeracion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<List<NumeracionSAR>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _NumeracionSAR.ToDataSourceResult(request);

        }


        public async Task<ActionResult<NumeracionSAR>> SaveNumeracionSAR([FromBody]NumeracionSAR _NumeracionSAR)
        {

            try
            {
                NumeracionSAR _listNumeracionSAR = new NumeracionSAR();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/NumeracionSAR/GetNumeracionSARById/" + _NumeracionSAR.IdNumeracion);
                string valorrespuesta = "";
                _NumeracionSAR.FechaModificacion = DateTime.Now;
                _NumeracionSAR.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listNumeracionSAR = JsonConvert.DeserializeObject<NumeracionSAR>(valorrespuesta);
                }

                if (_listNumeracionSAR.IdNumeracion == 0)
                {
                    _NumeracionSAR.FechaCreacion = DateTime.Now;
                    _NumeracionSAR.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_NumeracionSAR);
                }
                else
                {
                    var updateresult = await Update(_NumeracionSAR.IdNumeracion, _NumeracionSAR);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_NumeracionSAR);
        }

        // POST: NumeracionSAR/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<NumeracionSAR>> Insert(NumeracionSAR _NumeracionSAR)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _NumeracionSAR.UsuarioCreacion = HttpContext.Session.GetString("user");
                _NumeracionSAR.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/NumeracionSAR/Insert", _NumeracionSAR);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<NumeracionSAR>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _NumeracionSAR }, Total = 1 });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NumeracionSAR>> Update(Int64 id, NumeracionSAR _NumeracionSAR)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/NumeracionSAR/Update", _NumeracionSAR);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<NumeracionSAR>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _NumeracionSAR }, Total = 1 });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<NumeracionSAR>> Delete([FromBody]NumeracionSAR _NumeracionSAR)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/NumeracionSAR/Delete", _NumeracionSAR);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<NumeracionSAR>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _NumeracionSAR }, Total = 1 });
        }





    }
}
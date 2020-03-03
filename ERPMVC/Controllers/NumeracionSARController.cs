                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ERPMVC.DTO;
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
    public class NumeracionSARController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        public NumeracionSARController(ILogger<NumeracionSARController> logger, IOptions<MyConfig> config)
        {
            this.config = config;
            this._logger = logger;
        }

        [Authorize(Policy = "Administracion.Numeracion SAR")]
        public IActionResult NumeracionSAR()
        {
            return View();
        }
   
        [HttpPost("[action]")]
        public async Task<ActionResult> GripEditar([FromBody]DTO_NumeracionSAR _sarpara)
        {
            DTO_NumeracionSAR _NumeracionSAR = new DTO_NumeracionSAR();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/NumeracionSAR/GetNumeracionById/" + _sarpara.IdNumeracion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _NumeracionSAR = JsonConvert.DeserializeObject<DTO_NumeracionSAR>(valorrespuesta);

                }

                if (_NumeracionSAR == null)
                {
                    _NumeracionSAR = new DTO_NumeracionSAR { FechaLimite = DateTime.Now };
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
        public async Task<DataSourceResult> GetNumeracion([DataSourceRequest]DataSourceRequest request)
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
                    _NumeracionSAR = _NumeracionSAR.OrderByDescending(q => q.IdNumeracion).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _NumeracionSAR.ToDataSourceResult(request);

        }

        [HttpPost]
        public async Task<ActionResult<NumeracionSAR>> SaveNumeracionSAR([FromBody]DTO_NumeracionSAR _NumeracionSAR)
        {

            try
            {
               // DTO_NumeracionSAR _liNumeracionSAR = new DTO_NumeracionSAR();
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
                    _NumeracionSAR = JsonConvert.DeserializeObject<DTO_NumeracionSAR>(valorrespuesta);
                }

                if (_NumeracionSAR.IdNumeracion == 0)
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

        [HttpPut("IdNumeracion")]
        public async Task<IActionResult> Update(Int64 IdNumeracion, NumeracionSAR _NumeracionSAR)
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

        [HttpDelete("IdNumeracion")]
        public async Task<ActionResult<NumeracionSAR>> Delete(Int64 IdNumeracion, NumeracionSAR _NumeracionSAR)
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



        [HttpGet]
        public async Task<DataSourceResult> GetPuntoEmisonEnNumeracionSAR([DataSourceRequest]DataSourceRequest request, int IdTipoDocumento, int BranchId)
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
                    _NumeracionSAR = _NumeracionSAR.Where(q => q.Estado == "Activo")
                                                   .Where(q => q.BranchId == BranchId)
                                                   .Where(q => q.DocTypeId == IdTipoDocumento).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _NumeracionSAR.ToDataSourceResult(request);

        }

    }
}
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
using System.Security.Claims;

namespace ERPMVC.Controllers


{
    [Authorize]
    [CustomAuthorization]
    public class MantenimientoImpactoController : Controller
    {

        private readonly IOptions<MyConfig> _config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;

        public MantenimientoImpactoController(ILogger<MantenimientoImpactoController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this._config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;

        }


        public IActionResult MantenimientoImpacto()
        {
            ViewData["permisos"] = _principal;
            return View();
        }


        /// <summary>
        /// Obitiene el listado MantenimientoImpacto!
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest]DataSourceRequest request)
        {
            List<MantenimientoImpactoDTO> _MantenimientoImpacto = new List<MantenimientoImpactoDTO>();

            List<MantenimientoImpactoDTO> mantenimiento = new List<MantenimientoImpactoDTO>();

            try
            {

                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MantenimientoImpacto/GetMantenimientoImpacto");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MantenimientoImpacto = JsonConvert.DeserializeObject<List<MantenimientoImpactoDTO>>(valorrespuesta);
                    _MantenimientoImpacto = _MantenimientoImpacto.OrderByDescending(q => q.MantenimientoImpactoId).ToList();

                    foreach(var asignarcolor in _MantenimientoImpacto)
                    {

                        if(asignarcolor.Rango == "Catastrófico")
                        {
                            asignarcolor.color = "#FF0000";

                        }

                        if (asignarcolor.Rango == "Severo")
                        {
                            asignarcolor.color = "#FF8000";

                        }

                        if (asignarcolor.Rango == "Moderado")
                        {
                            asignarcolor.color = "#FFFF00";

                        }

                        if (asignarcolor.Rango == "Leve")
                        {
                            asignarcolor.color = "#008F39";

                        }



                        mantenimiento.Add(asignarcolor);
                        
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _MantenimientoImpacto.ToDataSourceResult(request);

        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwAddMantenimientoImpacto([FromBody]MantenimientoImpactoDTO _mantenimiento)
        {

            MantenimientoImpactoDTO _MantenimientoImpacto = new MantenimientoImpactoDTO();
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MantenimientoImpacto/GetMantenimientoImpactoById/" + _mantenimiento.MantenimientoImpactoId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MantenimientoImpacto = JsonConvert.DeserializeObject<MantenimientoImpactoDTO>(valorrespuesta);

                }

                if (_MantenimientoImpacto == null)
                {
                    _MantenimientoImpacto = new MantenimientoImpactoDTO();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_MantenimientoImpacto);

        }

        [HttpPost]
        public async Task<ActionResult<MantenimientoImpactoDTO>> SaveMantenimientoImpacto([FromBody]MantenimientoImpactoDTO _MantenimientoP)
        {
            MantenimientoImpacto _Mantenimiento = _MantenimientoP;
            try
            {
                MantenimientoImpacto _listMantenimiento = new MantenimientoImpacto();
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/MantenimientoImpacto/GetMantenimientoImpactoById/" + _Mantenimiento.MantenimientoImpactoId);
                string valorrespuesta = "";
                _Mantenimiento.FechaModificacion = DateTime.Now;
                _Mantenimiento.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Mantenimiento = JsonConvert.DeserializeObject<MantenimientoImpacto>(valorrespuesta);
                }

                if (_Mantenimiento == null) { _Mantenimiento = new Models.MantenimientoImpacto(); }


                if (_MantenimientoP.MantenimientoImpactoId == 0)
                {
                    _MantenimientoP.FechaCreacion = DateTime.Now;
                    _MantenimientoP.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_MantenimientoP);
                }
                else
                {
                    _MantenimientoP.UsuarioCreacion = _Mantenimiento.UsuarioCreacion;
                    _MantenimientoP.FechaCreacion = _Mantenimiento.FechaCreacion;
                    var updateresult = await Update(_Mantenimiento.MantenimientoImpactoId, _MantenimientoP);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_Mantenimiento);
        }


        // POST: MantenimientoImpacto/Insert
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<MantenimientoImpacto>> Insert(MantenimientoImpacto _MantenimientoImpactoP)
        {
            MantenimientoImpacto _MantenimientoImpacto = _MantenimientoImpactoP;
            try
            {
                // TODO: Add insert logic here
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _MantenimientoImpacto.UsuarioCreacion = HttpContext.Session.GetString("user");
                _MantenimientoImpacto.UsuarioModificacion = HttpContext.Session.GetString("user");
                _MantenimientoImpacto.FechaCreacion = DateTime.Now;
                _MantenimientoImpacto.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/MantenimientoImpacto/Insert", _MantenimientoImpacto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MantenimientoImpacto = JsonConvert.DeserializeObject<MantenimientoImpacto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return Ok(_MantenimientoImpacto);
        }

        [HttpPut("MantenimientoImpactoId")]
        public async Task<IActionResult> Update(Int64 MantenimientoImpactoId, MantenimientoImpacto _MantenimientoImpacto)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/MantenimientoImpacto/Update", _MantenimientoImpacto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MantenimientoImpacto = JsonConvert.DeserializeObject<MantenimientoImpacto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult {Data = new[] { _MantenimientoImpacto } });
        }

        [HttpPost]
        public async Task<ActionResult<MantenimientoImpacto>> Delete([FromBody]MantenimientoImpacto _MantenimientoImpacto)
        {
            try
            {
                string baseadress = _config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/MantenimientoImpacto/Delete", _MantenimientoImpacto);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _MantenimientoImpacto = JsonConvert.DeserializeObject<MantenimientoImpacto>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _MantenimientoImpacto }, Total = 1 });
        }

    }
}
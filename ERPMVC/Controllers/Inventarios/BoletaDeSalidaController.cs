using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
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
    public class BoletaDeSalidaController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public BoletaDeSalidaController(ILogger<BoletaDeSalidaController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Boleta de Salida")]
        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            ViewData["permisos"] = _principal;
            return View();
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> pvwBoletaDeSalida([FromBody] BoletaDeSalida _BoletaDeSalidap)
        {
            BoletaDeSalida _BoletaDeSalida = new BoletaDeSalida();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BoletaDeSalida/GetBoletaDeSalidaById/" + _BoletaDeSalidap.BoletaDeSalidaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<BoletaDeSalida>(valorrespuesta);

                }

                if (_BoletaDeSalida == null)
                {
                    _BoletaDeSalida = new BoletaDeSalida();
                    _BoletaDeSalida.DocumentDate = DateTime.Now;
                    _BoletaDeSalida.DocumentoId = 0;
                    _BoletaDeSalida.TonPuerto= 0;
                    _BoletaDeSalida.Tara = 0;
                    _BoletaDeSalida.PesoBruto= 0;
                    _BoletaDeSalida.QQInglesas= 0;
                    _BoletaDeSalida.PNInglesas= 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }



            return PartialView(_BoletaDeSalida);

        }


        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            List<BoletaDeSalida> _BoletaDeSalida = new List<BoletaDeSalida>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BoletaDeSalida/GetBoletaDeSalida");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<List<BoletaDeSalida>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _BoletaDeSalida.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<DataSourceResult> GetBoletaLines([DataSourceRequest] DataSourceRequest request, int BoletaId)
        {
            List<BoletaDeSalidaLine> _BoletaDeSalida = new List<BoletaDeSalidaLine>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BoletaDeSalida/GetBoletaDeSalidaLines/" + BoletaId);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<List<BoletaDeSalidaLine>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _BoletaDeSalida.ToDataSourceResult(request);

        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<BoletaDeSalida>> SaveBoletaDeSalida([FromBody] BoletaDeSalida _BoletaDeSalida)
        {

            try
            {
                BoletaDeSalida _listBoletaDeSalida = new BoletaDeSalida();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/BoletaDeSalida/GetBoletaDeSalidaById/" + _BoletaDeSalida.BoletaDeSalidaId);
                string valorrespuesta = "";
                _BoletaDeSalida.FechaModificacion = DateTime.Now;
                _BoletaDeSalida.UsuarioModificacion = HttpContext.Session.GetString("user");
                if (result.IsSuccessStatusCode)
                {

                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _listBoletaDeSalida = JsonConvert.DeserializeObject<BoletaDeSalida>(valorrespuesta);
                }

                if (_listBoletaDeSalida == null) { _listBoletaDeSalida = new BoletaDeSalida(); }

                if (_listBoletaDeSalida.BoletaDeSalidaId == 0)
                {
                    _BoletaDeSalida.FechaCreacion = DateTime.Now;
                    _BoletaDeSalida.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_BoletaDeSalida);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return await Task.Run(() => BadRequest("Error al guardar"));
                    }
                }
                else
                {
                    var updateresult = await Update(_BoletaDeSalida.BoletaDeSalidaId, _BoletaDeSalida);
                    if (updateresult.Result is BadRequestObjectResult)
                    {
                        return await Task.Run(() => BadRequest("Error al guardar"));
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }

            return Json(_BoletaDeSalida);
        }

        // POST: BoletaDeSalida/Insert
        [HttpPost("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<BoletaDeSalida>> Insert(BoletaDeSalida _BoletaDeSalida)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _BoletaDeSalida.UsuarioCreacion = HttpContext.Session.GetString("user");
                _BoletaDeSalida.UsuarioModificacion = HttpContext.Session.GetString("user");
                var result = await _client.PostAsJsonAsync(baseadress + "api/BoletaDeSalida/Insert", _BoletaDeSalida);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<BoletaDeSalida>(valorrespuesta);
                }
                else
                {
                    return BadRequest("Ha ocurrido un error al guardar los registros");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_BoletaDeSalida);
            // return new ObjectResult(new DataSourceResult { Data = new[] { _BoletaDeSalida }, Total = 1 });
        }

        [HttpPut("[controller]/[action]/{id}")]
        public async Task<ActionResult<BoletaDeSalida>> Update(Int64 id, BoletaDeSalida _BoletaDeSalida)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/BoletaDeSalida/Update", _BoletaDeSalida);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<BoletaDeSalida>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _BoletaDeSalida }, Total = 1 });
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<BoletaDeSalida>> Delete([FromBody] BoletaDeSalida _BoletaDeSalida)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/BoletaDeSalida/Delete", _BoletaDeSalida);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _BoletaDeSalida = JsonConvert.DeserializeObject<BoletaDeSalida>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _BoletaDeSalida }, Total = 1 });
        }


        [HttpGet]
        public ActionResult SFBoletaDeSalida(Int64 id)
        {

            BoletaDeSalida _BoletaDeSalida = new BoletaDeSalida { BoletaDeSalidaId = id, };

            return View(_BoletaDeSalida);
        }


        [HttpGet]
        public ActionResult SFEmitidasDetallado()
        {

            return View();
        }

        [HttpGet]
        public ActionResult SFGuiaRemision(int id)
        {

            GuiaRemision guiaRemision = new GuiaRemision { Id = id, };
            return View(guiaRemision);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult<GuiaRemision>> GenerarGuia([FromBody]GuiaRemision guiaRemision)
        {

            if (guiaRemision == null)
            {
                return BadRequest("No se pudo Obtener el Numero de la Boleta");
            }

            string baseadress = config.Value.urlbase;
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var result = await _client.GetAsync(baseadress + "api/BoletaDeSalida/GenerarGuiaRemision/" + guiaRemision.Id);
            string valorrespuesta = "";
            if (result.IsSuccessStatusCode)
            {
                valorrespuesta = await(result.Content.ReadAsStringAsync());
                guiaRemision = JsonConvert.DeserializeObject<GuiaRemision>(valorrespuesta);

            }
            else
            {
                return BadRequest(await result.Content.ReadAsStringAsync());
                    
            }
                    

            return Ok(guiaRemision);
        }




    }
}
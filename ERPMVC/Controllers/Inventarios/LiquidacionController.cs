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
using System.Security.Claims;

namespace ERPMVC.Controllers
{
    [Authorize]
    [CustomAuthorization]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class LiquidacionController : Controller
    {
        private readonly IOptions<MyConfig> config;
        private readonly ILogger _logger;
        private readonly ClaimsPrincipal _principal;
        public LiquidacionController(ILogger<LiquidacionController> logger, IOptions<MyConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            this.config = config;
            this._logger = logger;
            _principal = httpContextAccessor.HttpContext.User;
        }

        [Authorize(Policy = "Inventarios.Liquidaciones")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> pvwLiquidacion([FromBody] Liquidacion _sarpara)
        {
            Liquidacion _Liquidacion = new Liquidacion();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Liquidaciones/GetLiquidacionById/" + _sarpara.Id);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);

                }
                else {
                    _Liquidacion.FechaLiquidacion = DateTime.Now;
                    _Liquidacion.EstadoId = 5;
                }
                return PartialView(_Liquidacion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                //throw ex;
                return BadRequest("Error al cargar el formulario");

            }
        }
        public async Task<DataSourceResult> LiquidacionesPendientes([DataSourceRequest] DataSourceRequest request, [FromQuery(Name = "Recibos")] int[] recibos, [FromQuery(Name = "Id")] int id)
        {
            List<LiquidacionLine> liquidacionLines = new List<LiquidacionLine>();
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                string requestURl ;
                if (id== 0)
                {
                    string strrecibos = "?";
                    foreach (var item in recibos)
                    {
                        strrecibos += $"Recibos={item}";
                        if (item != recibos.ElementAt(recibos.Count() - 1))
                        {
                            strrecibos += "&&";
                        }
                    }
                    requestURl = $"api/Liquidaciones/GetLiquidacionesPendientesporCliente{strrecibos}";
                }
                else
                {
                    requestURl = $"api/Liquidaciones/LiquidacionDetalle/{id}";
                }
                

                
                var result = await _client.GetAsync(baseadress + requestURl);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    liquidacionLines = JsonConvert.DeserializeObject<List<LiquidacionLine>>(valorrespuesta);
                    liquidacionLines = liquidacionLines.OrderByDescending(e => e.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return  liquidacionLines.ToDataSourceResult(request);



        }



        public async Task<ActionResult> Aprobar([FromBody] Liquidacion _Liquidacion)
        {
            bool aprobacion = _Liquidacion.EstadoId == 6 ;
            
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + $"api/Liquidaciones/Aprobar/{_Liquidacion.Id}/{aprobacion}");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);
                }
                else
                {
                    return BadRequest($"Error este Liquidacion ya habia sido Aprobado o Rechazado.");
                }
            }


            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: { ex.ToString() }");
            }



            return Ok(aprobacion);

        }

        [HttpGet("[action]")]
        public async Task<DataSourceResult> GetLiquidacion([DataSourceRequest] DataSourceRequest request)
        {
            List<Liquidacion> _Liquidacion = new List<Liquidacion>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Liquidaciones/GetLiquidaciones");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<List<Liquidacion>>(valorrespuesta);
                    _Liquidacion = _Liquidacion.OrderByDescending(e => e.Id).ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return _Liquidacion.ToDataSourceResult(request);

        }

        [HttpGet("[controller]/[action]")]
        public async Task<JsonResult> GetJson([DataSourceRequest] DataSourceRequest request)
        {
            List<Liquidacion> _Liquidacion = new List<Liquidacion>();
            try
            {

                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var result = await _client.GetAsync(baseadress + "api/Liquidaciones/GetLiquidacion");
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<List<Liquidacion>>(valorrespuesta);

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                throw ex;
            }


            return Json(_Liquidacion.ToDataSourceResult(request));

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Liquidacion>> SaveLiquidacion([FromBody] Liquidacion _LiquidacionS)
        {
            Liquidacion _Liquidacion = _LiquidacionS;
            try
            {
                Liquidacion _listLiquidacion = new Liquidacion();
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                if (_Liquidacion.Id != 0)
                {
                    var result = await _client.GetAsync(baseadress + "api/Liquidaciones/GetLiquidacionById/" + _Liquidacion.Id);
                    string valorrespuesta = "";
                    _Liquidacion.FechaModificacion = DateTime.Now;
                    _Liquidacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                    if (result.IsSuccessStatusCode)
                    {

                        valorrespuesta = await (result.Content.ReadAsStringAsync());
                        _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);
                    }

                    if (_Liquidacion == null) { _Liquidacion = new Models.Liquidacion(); }

                    if (_Liquidacion.Id > 0)
                    {

                        var updateresult = await Update(_LiquidacionS);
                        if (updateresult.Result is BadRequestObjectResult)
                        {
                            return BadRequest(((BadRequestObjectResult)updateresult.Result).Value);
                        }
                    }
                }
                else
                {
                    _LiquidacionS.FechaCreacion = DateTime.Now;
                    _LiquidacionS.UsuarioCreacion = HttpContext.Session.GetString("user");
                    var insertresult = await Insert(_LiquidacionS);
                    if (insertresult.Result is BadRequestObjectResult)
                    {
                        return BadRequest(((BadRequestObjectResult)insertresult.Result).Value.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
            }

            return Json(_LiquidacionS);
        }





        // POST: Liquidaciones/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Liquidacion>> Insert(Liquidacion _Liquidacion)
        {
            try
            {
                // TODO: Add insert logic here
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                _Liquidacion.UsuarioCreacion = HttpContext.Session.GetString("user");
                _Liquidacion.UsuarioModificacion = HttpContext.Session.GetString("user");
                _Liquidacion.FechaModificacion = DateTime.Now;
                var result = await _client.PostAsJsonAsync(baseadress + "api/Liquidaciones/Insert", _Liquidacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    return BadRequest($"Ocurrio un error: {error}");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }
            return Ok(_Liquidacion);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Liquidacion>> Update(Liquidacion _Liquidacion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PutAsJsonAsync(baseadress + "api/Liquidaciones/Update", _Liquidacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);
                }
                else
                {
                    string error = await result.Content.ReadAsStringAsync();
                    return BadRequest($"Ocurrio un error: {error}");

                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error{ex.Message}");
            }

            return new ObjectResult(new DataSourceResult { Data = new[] { _Liquidacion }, Total = 1 });
        }

        [HttpPost]
        public async Task<ActionResult<Liquidacion>> Delete(Int64 Id, Liquidacion _Liquidacion)
        {
            try
            {
                string baseadress = config.Value.urlbase;
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));

                var result = await _client.PostAsJsonAsync(baseadress + "api/Liquidaciones/Delete", _Liquidacion);
                string valorrespuesta = "";
                if (result.IsSuccessStatusCode)
                {
                    valorrespuesta = await (result.Content.ReadAsStringAsync());
                    _Liquidacion = JsonConvert.DeserializeObject<Liquidacion>(valorrespuesta);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrio un error: { ex.ToString() }");
                return BadRequest($"Ocurrio un error: {ex.Message}");
            }



            return new ObjectResult(new DataSourceResult { Data = new[] { _Liquidacion }, Total = 1 });
        }

        



    }
}
